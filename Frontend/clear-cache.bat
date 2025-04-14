@echo off
echo Closing any running Electron processes...
taskkill /F /IM electron.exe /T 2>nul
taskkill /F /IM electron-vite.exe /T 2>nul

echo Clearing Electron cache...
rmdir /S /Q "%APPDATA%\Electron" 2>nul
rmdir /S /Q "%APPDATA%\side-by-side-translator" 2>nul
rmdir /S /Q "%LOCALAPPDATA%\side-by-side-translator" 2>nul
rmdir /S /Q "%LOCALAPPDATA%\Electron" 2>nul

echo Cache cleared. 
echo Run the app with admin privileges using: "npm run dev"
echo. 