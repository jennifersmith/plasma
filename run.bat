@echo off
SET PATH=%PATH%;%WINDIR%\Microsoft.Net\Framework\v4.0.30319;%PROGRAMFILES(X86)%\Git\bin
CALL MSBuild.exe Plasma.proj /verbosity:n  %*
IF ERRORLEVEL 1 goto RedBuild
IF ERRORLEVEL 0 goto GreenBuild

:RedBuild
REM color 4f
more tools\buildflags\failed.txt
goto TheEnd

:GreenBuild
more tools\buildflags\passed.txt
REM color 2F

:TheEnd
pause

REM color 07