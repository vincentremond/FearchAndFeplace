$ErrorActionPreference = "Stop"

dotnet tool restore
dotnet build

AddToPath .\FearchAndFeplace\bin\Debug
