@echo.服务启动......  
@echo off  
@echo 当前路径 %~dp0\HangfireService.Core.exe
@sc create HangFireService binPath= "%~dp0\HangfireService.Core.exe"  
@net start HangFireService   
@sc config HangFireService  start= AUTO  
@echo off  
@echo.服务启动完毕！  
@pause