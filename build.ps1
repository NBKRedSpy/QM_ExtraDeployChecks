
 
try {

    mkdir package -ErrorAction SilentlyContinue  
    Remove-Item package\* -Recurse -Force -ErrorAction SilentlyContinue
    $packageFolder = "./package/QM_ExtraDeployChecks"
    mkdir $packageFolder | Out-Null

    # ---- Build the projects.  The projects will automatically deploy to the Steam Workshop folder.

    "Bootstrap"
    dotnet clean ./src\QM_ExtraDeployChecks_Bootstrap.csproj
    dotnet build -c Release ./src\QM_ExtraDeployChecks_Bootstrap.csproj -o $packageFolder
    
    "Stable"
    dotnet clean ..\main-repo\src\QM_ExtraDeployChecks.csproj
    dotnet build -c Release ..\main-repo\src\QM_ExtraDeployChecks.csproj -o $packageFolder\Stable
    # dotnet build has a bug where it always copies any project references.
    del $packageFolder/Stable/QM_ExtraDeployChecks_Bootstrap.*

    "Beta"
    dotnet clean ..\beta\src\QM_ExtraDeployChecks.csproj
    dotnet build -c Release ..\beta\src\QM_ExtraDeployChecks.csproj -o $packageFolder\Beta
    # dotnet build has a bug where it always copies any project references.
    del $packageFolder/Beta/QM_ExtraDeployChecks_Bootstrap.*

    # ---- Create the package zip file.
    Copy-Item ../main-repo/media/thumbnail.png $packageFolder
    Copy-Item ../main-repo/README.md $packageFolder
    Copy-Item version-info.json $packageFolder
    Copy-Item modmanifest.json $packageFolder

    Compress-Archive -Path $packageFolder\* -DestinationPath ./QM_ExtraDeployChecks.zip -Force

    # Add the beta text if beta is not disabled.
    # Otherwise remove?

    "Build completed"
} catch {
    Write-Error "Build Failure: $_"
    exit 1
}



