dotnet publish -r linux-x64 --self-contained -c Release
rem dotnet publish -r osx-x64 --self-contained -c Release
del linux-x64.zip
rem del osx-x64.zip
powershell.exe -nologo -noprofile -command "& { Add-Type -A 'System.IO.Compression.FileSystem'; [IO.Compression.ZipFile]::CreateFromDirectory('bin\Release\netcoreapp2.0\linux-x64', '.\linux-x64.zip'); }"
rem powershell.exe -nologo -noprofile -command "& { Add-Type -A 'System.IO.Compression.FileSystem'; [IO.Compression.ZipFile]::CreateFromDirectory('bin\Release\netcoreapp2.0\osx-x64', '.\osx-x64.zip'); }"
git commit -m 'Update binaries for linux' linux-x64.zip
rem git commit -m 'Update binaries for osx' osx-x64.zip