IF EXIST "%VS140COMNTOOLS%\vsvars32.bat" CALL "%VS140COMNTOOLS%\vsvars32.bat"
IF EXIST "%VS120COMNTOOLS%\vsvars32.bat" CALL "%VS120COMNTOOLS%\vsvars32.bat"
IF EXIST "%VS110COMNTOOLS%\vsvars32.bat" CALL "%VS110COMNTOOLS%\vsvars32.bat"
msbuild ..\Fluent-Json.NET-Configuration.sln /p:Configuration=Release /p:Platform="Any CPU"
nuget pack Fluent-Json.NET-Configuration.csproj -Prop Configuration=Release -Prop Platform=AnyCPU
nuget pack Fluent-Json.NET-Configuration.csproj -Prop Configuration=Release -Prop Platform=AnyCPU -Symbols
