@ECHO OFF
REM SET ProjectPath=%1
REM IF [%1]==[] 
	SET ProjectPath=%~dp0

REM **************************
REM **  User Edit Variables **
REM **************************
REM		\/-- Set Up Here --\/
SET CONFIGURATION=SignalCi
SET ProjectName=BillingSuiteWebApp
SET Flavor=SignalCi
SET "TargetIISUrl=https://SignalCiApp:8172/MSDeployAgentService"
SET "TargetIISUrl=https://SignalCiApp:8172/MSDeploy.axd"
SET OverwriteWebConfig=1
REM SET DeployUser=sgnl\devpublish <-- Not an admin for publishing this way!
SET DeployUser=sgnl\pdelosreyes
SET Password=Aug2016!
REM		/\-- Set Up Here --/\
REM **************************
REM * End User Edit Variables*
REM **************************

ECHO %USERNAME% at %Date% %Time%

SET "MSBuild.exe=%ProgramFiles(x86):"=%\MSBuild\14.0\Bin\MSBuild.exe"

ECHO -- ************************************************************** --
SET "BuildPath=%ProjectPath:"=%..\..\Build\SignalBuild\"
SET "NugetCmd="%BuildPath:"=%nuget.exe" restore "%ProjectPath:"=%\..\..\SignalAll.sln""
ECHO -- ************************************************************** --
ECHO -- ***************  Restore Nuget Packages   ******************** --
ECHO -- ************************************************************** --
@ECHO ON
%NugetCmd%
@ECHO OFF

SET "Clean="%MSBuild.exe:"=%" "%ProjectPath:"=%\%ProjectName:"=%.csproj" /p:Configuration=%CONFIGURATION:"=% /t:Clean"
ECHO ***************************************
ECHO Clean Project / Solution Call:
ECHO %Clean%
ECHO ***************************************
@ECHO ON
%Clean%
@ECHO OFF
ECHO PROJECT CLEAN COMPLETE
 
SET "Build="%MSBuild.exe:"=%" "%ProjectPath:"=%\%ProjectName%.csproj" /p:Configuration=%CONFIGURATION:"=% /t:Build /p:Flavor=%Flavor:"=% /t:SlowCheetah /t:CompileTypeScript"
ECHO ***************************************
ECHO Build Create Deployment Package Call:
ECHO %Build%
ECHO ***************************************
@ECHO ON
%Build%
@ECHO OFF
ECHO Project Compiled 

SET "Package="%MSBuild.exe:"=%" "%ProjectPath:"=%\%ProjectName:"=%.csproj" /p:Configuration=%CONFIGURATION:"=% /p:DeployOnBuild=true /p:CreatePackageOnPublish=True /t:Package /p:PublishProfile="%ProjectPath:"=%Properties\PublishProfiles\App_%Flavor:"=%.pubxml" /p:Password=5Bbubble#$R /p:VisualStudioVersion=14.0"
ECHO ***************************************
ECHO Build Create Deployment Package Call:
ECHO %Package%
ECHO ***************************************
@ECHO ON
%Package%
@ECHO OFF
ECHO DEPLOY PACKAGE CREATED

IF %OverwriteWebConfig%==1 (
	SET "DeployCmd="%ProjectPath:"=%Obj\%CONFIGURATION:"=%\Package\%ProjectName:"=%.deploy.cmd" /y /m:%TargetIISUrl:"=% /u:%DeployUser:"=% /p:%Password:"=% /a:basic -AllowUntrusted"
) ELSE (
	SET "DeployCmd="%ProjectPath:"=%Obj\%CONFIGURATION:"=%\Package\%ProjectName:"=%.deploy.cmd" /y "-skip:objectName=filePath,absolutePath=\\ApplicationFolder\\web\.config$" /m:%TargetIISUrl:"=% /u:%DeployUser:"=% /p:%Password:"=% /a:basic -AllowUntrusted"
)
ECHO ***************************************
ECHO Deployment Call:
ECHO %DeployCmd%
ECHO ***************************************
@ECHO ON
%DeployCmd%
@ECHO OFF

REM pushd "%ProjectPath%"