protoc.exe -I=./ --csharp_out=./ ./Protocol.proto 
protoc.exe -I=./ --csharp_out=./ ./Enum.proto
protoc.exe -I=./ --csharp_out=./ ./Struct.proto
if ERRORLEVEL 1 PAUSE


