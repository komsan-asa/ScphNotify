@echo off
chcp 65001 >nul
cd /d "%~dp0"
title ScphNotify - publish.ps1 -Installer
echo ===== publish.ps1 -Installer =====
echo.
powershell -NoProfile -ExecutionPolicy Bypass -Command "& '.\publish.ps1' -Installer *>&1 | Tee-Object -FilePath '.\build.log'"
set RC=%ERRORLEVEL%
echo.
echo ===== DONE  EXITCODE=%RC% =====
pause
