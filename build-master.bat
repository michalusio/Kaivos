@echo off
REM S2Fpdm9zU3VwZXJHcmE
rd /s /q Kaivos-Master-Branch
cd /D %temp%
git config --list | findstr /C:"filter.lfs.smudge" 1>nul
IF errorlevel 1 (
	echo.GIT-LFS not found, falling back to normal _clone_
	git clone --single-branch --no-hardlinks https://github.com/michalusio/Kaivos.git Kaivos-Master-Branch
) ELSE (
	echo.GIT-LFS found, download should be quick
	git lfs clone --single-branch --no-hardlinks https://github.com/michalusio/Kaivos.git Kaivos-Master-Branch
)
CALL :CHECK_FAIL
echo.Building Kaivos using Unity...
md Kaivos-Master-Build
set BuildParams=-batchmode -projectPath ".\Kaivos-Master-Branch\" -logfile "./Kaivos-Master-Build/KaivosBuild.log" -silent-crashes -quit
echo.%BuildParams%
set /p BuildMode="Build in Debug or Release (d/r)? "
IF "%BuildMode%"=="d" (
	echo.Building Kaivos in DEBUG version
	"D:\Programy\UnityEditors\2019.2.13f1\Editor\Unity.exe" %BuildParams% -executeMethod ScriptBatch.BuildGameDebug
) ELSE (
	echo.Building Kaivos in RELEASE version
	"D:\Programy\UnityEditors\2019.2.13f1\Editor\Unity.exe" %BuildParams% -executeMethod ScriptBatch.BuildGameRelease
)
CALL :CHECK_FAIL

rd /s /q Kaivos-Master-Branch
start Kaivos-Master-Build

GOTO :EOF

:: /// check if the app has failed
:CHECK_FAIL
if NOT ["%errorlevel%"]==["0"] (
    pause
    exit /b %errorlevel%
)