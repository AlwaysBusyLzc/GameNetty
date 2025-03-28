@echo off
setlocal

:: 定义 导出到服务器的目录
set server_path=../../Server/Model/Generate/Message

:: 导出本目录所有 proto 文件成cs代码到指定目录
for %%i in (*.proto) do (
    protoc --proto_path=./ --csharp_out=%server_path% %%i
)

endlocal
echo export finish!
pause