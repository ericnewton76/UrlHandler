@ECHO OFF

setlocal

if "%BUILD_VERSION%" == "" set BUILD_VERSION=%APPVEYOR_BUILD_VERSION%
if "%BUILD_VERSION%" == "" echo No build version!!! && goto :END

msbuild src\UrlHandler\UrlHandler.csproj /P:Configuration=Release /p:OutputPath=%~dp0\Build\NugetPack\lib
if errorlevel 1 echo ERROR! msbuild & goto :END

mkdir %~dp0\Build\NugetPack\Content 2>nul

copy src\UrlHandler\Urls.tt %~dp0\Build\NugetPack\Content

pushd %~dp0\Build\NugetPack

copy ..\..\UrlHandler.nuspec . /Y

echo nuget pack UrlHandler.nuspec -version %BUILD_VERSION%
nuget pack UrlHandler.nuspec -version %BUILD_VERSION%
if errorlevel 1 echo ERROR! nuget pack & goto :END

nuget push UrlHandler.%BUILD_VERSION%.nupkg -Source https://www.nuget.org/

:END
popd