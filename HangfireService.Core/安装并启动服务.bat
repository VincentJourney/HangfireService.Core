@echo.��������......  
@echo off  
@echo ��ǰ·�� %~dp0\HangfireService.Core.exe
@sc create HangFireService binPath= "%~dp0\HangfireService.Core.exe"  
@net start HangFireService   
@sc config HangFireService  start= AUTO  
@echo off  
@echo.����������ϣ�  
@pause