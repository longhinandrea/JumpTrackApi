#!/bin/sh
dotnet build -c Release
dotnet publish -c Release -o out
dotnet JumpTrackApi.dll --urls=http://0.0.0.0:${PORT:-8080}