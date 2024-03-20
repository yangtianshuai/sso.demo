@echo off
cd /d %~dp0

regasm /u sso.dll /tlb:sso.tlb
regasm sso.dll /tlb:sso.tlb

pause