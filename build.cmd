@ECHO OFF

dotnet tool restore
dotnet build -- %*

AddToPath .\FearchAndFeplace\bin\Debug
