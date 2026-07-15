@echo off
setlocal

set "SCRIPT_DIR=%~dp0"
set "SOLUTION=%SCRIPT_DIR%BrighterTools.Messaging.sln"
set "NUGET_CONFIG=%SCRIPT_DIR%NuGet.config"
set "OUTPUT_DIR=%SCRIPT_DIR%artifacts\nuget"
set "CONFIGURATION=Release"
set "VERSION=%~1"

if not exist "%NUGET_CONFIG%" (
    echo NuGet.config not found: %NUGET_CONFIG%
    exit /b 1
)

if not exist "%OUTPUT_DIR%" (
    mkdir "%OUTPUT_DIR%"
)

echo Restoring BrighterTools.Messaging...
dotnet restore "%SOLUTION%" --configfile "%NUGET_CONFIG%"
if errorlevel 1 exit /b %errorlevel%

echo Building BrighterTools.Messaging...
dotnet build "%SOLUTION%" -c %CONFIGURATION% --no-restore
if errorlevel 1 exit /b %errorlevel%

echo Packing BrighterTools.Messaging packages...
for %%P in (
    "src\BrighterTools.Messaging\BrighterTools.Messaging.csproj"
    "src\BrighterTools.Messaging.MailKit\BrighterTools.Messaging.MailKit.csproj"
    "src\BrighterTools.Messaging.Postmark\BrighterTools.Messaging.Postmark.csproj"
    "src\BrighterTools.Messaging.SendGrid\BrighterTools.Messaging.SendGrid.csproj"
    "src\BrighterTools.Messaging.Twilio\BrighterTools.Messaging.Twilio.csproj"
) do (
    if "%VERSION%"=="" (
        dotnet pack "%SCRIPT_DIR%%%~P" -c %CONFIGURATION% --no-build --output "%OUTPUT_DIR%" --configfile "%NUGET_CONFIG%"
    ) else (
        dotnet pack "%SCRIPT_DIR%%%~P" -c %CONFIGURATION% --no-build --output "%OUTPUT_DIR%" --configfile "%NUGET_CONFIG%" /p:Version=%VERSION%
    )
    if errorlevel 1 exit /b %errorlevel%
)

echo.
echo Package output:
echo   %OUTPUT_DIR%
echo.
echo Publish command:
echo   Use the GitHub Actions publish-tool workflow with Trusted Publishing to publish the generated BrighterTools.Messaging packages to nuget.org.

endlocal
