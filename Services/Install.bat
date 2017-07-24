
sc stop BackServer
sc delete BackServer
ping -n 5 127.0.0.1>nul
Services.exe Install
sc start BackServer
pause