@echo off
set MOD_SEARCH_PATHS=./../mods,./mods
set ENGINE_DIR=..
cd ../../engine

bin\OpenRA.Utility.exe openop2 --create-actors

pause
exit /b
