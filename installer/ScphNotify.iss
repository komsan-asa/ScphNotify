; ตัวติดตั้ง SCPH Notify (Inno Setup 6/7)
;
; สร้างด้วย:  .\publish.ps1 -Installer
; หรือเปิดไฟล์นี้ใน Inno Setup Compiler แล้วกด Build (ต้อง publish ก่อน)
;
; สิ่งที่ตัวติดตั้งทำให้
;   - ลงโปรแกรมที่ Program Files (แบบ self-contained เครื่องปลายทางไม่ต้องลง .NET ก่อน)
;   - วาง appsettings.json ไว้ข้างไฟล์ .exe เพราะโปรแกรมอ่านค่าเริ่มต้นจากตรงนั้น
;     (ดู AppConfig.DefaultFilePath) ส่วนค่าที่ผู้ใช้กดบันทึกเองจะไปอยู่ที่
;     %LOCALAPPDATA%\ScphNotify\settings.json ซึ่งเขียนได้โดยไม่ต้องใช้สิทธิ์ผู้ดูแลระบบ
;   - ตั้งให้เปิดอัตโนมัติเมื่อเข้าสู่ระบบ ถ้าผู้ติดตั้งเลือก

#define AppName "ScphNotify"
#define AppTitle "SCPH Notify"
; publish.ps1 อ่าน <Version> จาก .vbproj แล้วส่งมาทาง /DAppVersion เลขรุ่นจึงมีที่เดียว
; ค่าข้างล่างใช้เฉพาะตอนเปิดไฟล์นี้ใน Inno Setup Compiler แล้วกด Build เอง
#ifndef AppVersion
  #define AppVersion "2.0.0.0"
#endif
#ifndef WithSettings
  #define WithSettings "0"
#endif
#define AppPublisher "โรงพยาบาลสมเด็จพระยุพราชสระแก้ว"
#define AppExe "ScphNotify.exe"
#define SourceDir "..\dist\app"

[Setup]
AppId={{E7B89AE4-6902-48A5-BF18-99BA2D3CC1F0}
AppName={#AppTitle}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
DefaultDirName={autopf}\{#AppName}
DefaultGroupName={#AppTitle}
OutputDir=..\dist
OutputBaseFilename=ScphNotify-Setup-{#AppVersion}
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
; ต้องใช้สิทธิ์ผู้ดูแลระบบ เพราะลงที่ Program Files
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64compatible
ArchitecturesAllowed=x64compatible
UninstallDisplayIcon={app}\{#AppExe}
UninstallDisplayName={#AppTitle}
DisableProgramGroupPage=yes
; ติดตั้งทับตอนโปรแกรมเปิดค้างอยู่ได้ ไม่ต้องให้ผู้ใช้ไปไล่ปิดเอง
; (โปรแกรมเปิดค้างเป็นแถบลอย/ไอคอนถาดระบบ ผู้ใช้มักลืมว่าเปิดอยู่)
CloseApplications=yes
CloseApplicationsFilter=*.exe
RestartApplications=no
; ปิดคำเตือนตอนคอมไพล์เรื่อง "ลง Program Files ด้วยสิทธิ์ผู้ดูแลระบบ แต่ไปเขียน HKCU"
; รู้อยู่แล้วและอธิบายไว้ที่ [Registry] ข้างล่าง ปล่อยไว้จะกลายเป็น error ของ publish.ps1
UsedUserAreasWarning=no

[Languages]
Name: "thai"; MessagesFile: "compiler:Languages\Thai.isl"

[Files]
; appsettings.json แยกออกจาก wildcard เพื่อคุมเงื่อนไขการเขียนทับเอง
Source: "{#SourceDir}\*"; DestDir: "{app}"; Excludes: "\appsettings.json"; \
    Flags: ignoreversion recursesubdirs createallsubdirs
; onlyifdoesntexist = ติดตั้งทับรุ่นเก่าแล้วค่าที่ผู้ดูแลระบบแก้ไว้ในไฟล์นี้ต้องไม่หาย
; (ในไฟล์มีรหัสผ่านฐานข้อมูล ถ้าไม่ต้องการให้ติดไปกับตัวติดตั้ง ใช้ publish.ps1 -NoSettings)
#if WithSettings == "1"
Source: "{#SourceDir}\appsettings.json"; DestDir: "{app}"; Flags: onlyifdoesntexist
#endif

[Registry]
; เปิดโปรแกรมอัตโนมัติเมื่อเข้าใช้งาน Windows — โปรแกรมเริ่มเป็นแถบลอยเล็ก ๆ อยู่แล้ว
; จึงไม่มีสวิตช์ซ่อนหน้าต่างให้ต้องใส่เพิ่ม
;
; ข้อจำกัด: ตัวติดตั้งรันด้วยสิทธิ์ผู้ดูแลระบบ HKCU ที่เขียนจึงเป็นของบัญชีที่ยกสิทธิ์
; ถ้ายกสิทธิ์ด้วยบัญชี admin คนละตัวกับคนที่ใช้เครื่อง ค่าจะไปลงผิด hive
; กรณีนั้นให้ผู้ใช้จริงลากทางลัดไปวางที่โฟลเดอร์ Startup (shell:startup) แทน
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; \
    ValueName: "{#AppName}"; ValueData: """{app}\{#AppExe}"""; \
    Flags: uninsdeletevalue; Tasks: startup

[Icons]
Name: "{group}\{#AppTitle}"; Filename: "{app}\{#AppExe}"
Name: "{group}\ตั้งค่าฐานข้อมูล ({#AppTitle})"; Filename: "{app}\{#AppExe}"; Parameters: "--dbconfig"; \
    Comment: "เปิดหน้าตั้งค่าฐานข้อมูลโดยไม่ต้องล็อกอิน (ใช้ตอนต่อฐานข้อมูลไม่ได้)"
Name: "{group}\ถอนการติดตั้ง {#AppTitle}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#AppTitle}"; Filename: "{app}\{#AppExe}"; Tasks: desktopicon

[Tasks]
Name: "startup"; Description: "เปิดโปรแกรมอัตโนมัติเมื่อเข้าใช้งาน Windows"; GroupDescription: "ตัวเลือก:"
Name: "desktopicon"; Description: "สร้างไอคอนบนหน้าจอ"; GroupDescription: "ตัวเลือก:"; Flags: unchecked

[Run]
Filename: "{app}\{#AppExe}"; Description: "เปิด {#AppTitle} เลย"; Flags: nowait postinstall skipifsilent
