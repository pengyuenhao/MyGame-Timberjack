call refresh.bat
call stop.bat
adb forward tcp:54999 localabstract:Unity-com.port
call start.bat
pause