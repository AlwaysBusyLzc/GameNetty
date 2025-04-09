@echo off

pyinstaller --onefile ./src/generate.py
::pyinstaller --onefile ./src/modify.py

:: 拷贝到输出目录
copy /Y ".\dist\generate.exe" "..\..\Config\Proto\generate.exe"

echo execute success!

pause