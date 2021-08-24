@echo off
set MOD_ID=openop2
set MOD_SEARCH_PATHS=./../../mods,./../mods
set ENGINE_DIR=./..
set ENGINE_BIN_DIR=./engine/bin
set TEMPLATE_DIR=%CD%
cd %ENGINE_BIN_DIR%

call OpenRA.Utility.exe %MOD_ID% --create-sequences
pause
exit /b
