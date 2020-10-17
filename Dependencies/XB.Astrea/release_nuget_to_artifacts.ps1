dotnet pack -c Release
dotnet nuget push --source "TeamCrossBorder" --api-key az ./bin/Release/XB.Astrea.Client.1.0.0.nupkg
