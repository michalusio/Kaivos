@echo off
REM S2Fpdm9zU3VwZXJHcmE
rd /s /q Kaivos-Master-Branch
cd /D %temp%
git config --list | findstr /C:"filter.lfs.smudge" 1>nul
IF errorlevel 1 (
	echo. GIT-LFS not found, falling back to normal _clone_
	git clone --single-branch --no-hardlinks https://github.com/michalusio/Kaivos.git Kaivos-Master-Branch
) ELSE (
	echo. GIT-LFS found, download will be quick
	git lfs clone --single-branch --no-hardlinks https://github.com/michalusio/Kaivos.git Kaivos-Master-Branch
)
md Kaivos-Master-Build
"D:\Programy\UnityEditors\2019.2.12f1\Editor\Unity.exe" -batchmode -projectPath ".\Kaivos-Master-Branch\" -logfile "./Kaivos-Master-Build/KaivosBuild.log" -silent-crashes -executeMethod ScriptBatch.BuildGame -quit
rd /s /q Kaivos-Master-Branch
start Kaivos-Master-Build