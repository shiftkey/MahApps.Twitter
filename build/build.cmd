del *.nupkg

NuGet.exe pack Package.nuspec -Version %system.build.number%

%forfiles /m *.nupkg /c "cmd /c NuGet.exe push @FILE <your-key>"

