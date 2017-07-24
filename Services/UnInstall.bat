sc stop BackServer
ping -n 2 127.0.0.1>nul
sc delete BackServer
pause