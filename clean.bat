@echo off
for /r %%i in (debug,ipch) do rd /s /q "%%i"
for /r %%i in (*.sdf) do del /s /f /q "%%i"
pause