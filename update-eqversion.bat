copy /Y ..\DevOps\EqPsUtils\SetEqVersionsForFolder.ps1 .
copy /Y ..\DevOps\EqPsUtils\SetEqNugetVersionInCsproj.ps1 .
copy /Y ..\DevOps\EqPsUtils\SetEqScriptsVersionInCshtml.ps1 .
copy /Y ..\DevOps\EqPsUtils\SetEqNpmVersionInPackageJson.ps1 .

PowerShell.exe -File ".\SetEqVersionsForFolder.ps1" 5.1.2 5.1.6
