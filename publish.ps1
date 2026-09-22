<#
    สร้างชุดแจกจ่ายของ SCPH Notify

        .\publish.ps1                 สร้างโฟลเดอร์โปรแกรม (dist\app)
        .\publish.ps1 -Installer      สร้างตัวติดตั้ง .exe ต่อให้ด้วย (ต้องมี Inno Setup)
        .\publish.ps1 -Installer -NoSettings   ไม่เอา appsettings.json ติดไปกับตัวติดตั้ง

    ผลลัพธ์เป็นแบบ self-contained คือรวม .NET ไว้ในตัว เครื่องปลายทางไม่ต้องลง
    .NET Desktop Runtime ก่อน — ลงแล้วเปิดใช้ได้เลย

    หมายเหตุ: ห้ามเปิด PublishTrimmed เพราะ WinForms กับ MySqlConnector ใช้ reflection
    ตัดแล้วจะพังตอนรันจริงเท่านั้น หาสาเหตุยาก
#>
param(
    [string]$Configuration = "Release",
    [string]$Runtime = "win-x64",
    [switch]$Installer,
    [switch]$NoSettings
)

$ErrorActionPreference = "Stop"
$root = $PSScriptRoot
$project = Join-Path $root "ScphNotify\ScphNotify.vbproj"
$outDir = Join-Path $root "dist\app"

Write-Host "== publish ==" -ForegroundColor Cyan

# เปิดโปรแกรมจากโฟลเดอร์นี้ค้างไว้จะลบไฟล์ไม่ได้ บอกให้ชัดดีกว่าปล่อยให้ error งง ๆ
$running = Get-Process ScphNotify -ErrorAction SilentlyContinue |
    Where-Object { $_.Path -and $_.Path.StartsWith($outDir, [StringComparison]::OrdinalIgnoreCase) }
if ($running) {
    throw "มี ScphNotify ที่เปิดจาก $outDir ค้างอยู่ (PID $($running.Id -join ', ')) — ปิดก่อนแล้วรันใหม่"
}

if (Test-Path $outDir) { Remove-Item $outDir -Recurse -Force }

dotnet publish $project `
    -c $Configuration `
    -r $Runtime `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:EnableCompressionInSingleFile=true `
    -p:DebugType=none `
    -o $outDir

if ($LASTEXITCODE -ne 0) { throw "publish ไม่สำเร็จ" }

# ไอคอนโปรแกรมฝังเป็น EmbeddedResource อยู่ใน .exe แล้ว (AppSession.AppIcon อ่านจากตรงนั้น)
# จึงไม่ต้องก๊อปไฟล์ .ico ตามมาให้เหมือนโปรเจกต์อื่น

$settings = Join-Path $outDir "appsettings.json"
if (-not (Test-Path $settings)) {
    Write-Warning "ไม่พบ appsettings.json ใน $outDir — โปรแกรมจะใช้ค่าเริ่มต้นในโค้ดจนกว่าจะตั้งค่าเอง"
}

$size = [math]::Round(((Get-ChildItem $outDir -Recurse | Measure-Object Length -Sum).Sum / 1MB), 1)
Write-Host "ได้โฟลเดอร์โปรแกรมที่ $outDir ($size MB)" -ForegroundColor Green
Get-ChildItem $outDir | Select-Object Name, @{n='MB';e={[math]::Round($_.Length/1MB,1)}} | Format-Table

if (-not $Installer) { return }

Write-Host "== installer ==" -ForegroundColor Cyan
$iscc = @(
    "$env:ProgramFiles\Inno Setup 7\ISCC.exe",
    "${env:ProgramFiles(x86)}\Inno Setup 7\ISCC.exe",
    "$env:ProgramFiles\Inno Setup 6\ISCC.exe",
    "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe"
) | Where-Object { Test-Path $_ } | Select-Object -First 1

if (-not $iscc) {
    Write-Warning "ไม่พบ Inno Setup — ติดตั้งจาก https://jrsoftware.org/isdl.php แล้วรันใหม่ด้วย -Installer"
    return
}

# เลขรุ่นมีที่เดียวคือ <Version> ใน .vbproj แล้วส่งต่อให้ Inno
# เขียนซ้ำสองที่แล้วจะเพี้ยนกันเองสักวัน
[xml]$proj = Get-Content $project -Raw
$version = ($proj.Project.PropertyGroup.Version | Where-Object { $_ }) | Select-Object -First 1
if (-not $version) { throw "ไม่พบ <Version> ใน $project" }
Write-Host "รุ่น $version" -ForegroundColor Cyan

$withSettings = (-not $NoSettings) -and (Test-Path $settings)
if ($withSettings) {
    Write-Warning "ตัวติดตั้งจะมี appsettings.json ติดไปด้วย — ในไฟล์มีรหัสผ่านฐานข้อมูล"
    Write-Warning "แจกให้เฉพาะผู้ที่ควรเห็นค่าเหล่านี้ ถ้าไม่ต้องการให้ติดไปด้วย รันใหม่ด้วย -NoSettings"
} elseif (-not $NoSettings) {
    Write-Warning "ไม่พบ appsettings.json — ตัวติดตั้งจะไม่มีไฟล์ตั้งค่า ต้องไปตั้งเองที่หน้าตั้งค่าฐานข้อมูลหลังติดตั้ง"
} else {
    Write-Host "ข้าม appsettings.json ตามที่สั่ง (-NoSettings) — หลังติดตั้งให้ตั้งค่าฐานข้อมูลในโปรแกรมเอง" -ForegroundColor Yellow
}

$flag = if ($withSettings) { "1" } else { "0" }
& $iscc "/DAppVersion=$version" "/DWithSettings=$flag" (Join-Path $root "installer\ScphNotify.iss")
if ($LASTEXITCODE -ne 0) { throw "สร้างตัวติดตั้งไม่สำเร็จ" }

Get-ChildItem (Join-Path $root "dist") -Filter *.exe |
    ForEach-Object { Write-Host "ได้ตัวติดตั้งที่ $($_.FullName) ($([math]::Round($_.Length/1MB,1)) MB)" -ForegroundColor Green }
