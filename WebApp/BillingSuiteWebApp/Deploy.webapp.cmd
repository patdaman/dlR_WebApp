 @ECHO OFF
SET ProjectPath=%~dp0

REM **************************
REM **  User Edit Variables **
REM **************************

REM ** Options
REM ** DEBUG		
REM ** RELEASE

REM		\/-- Set Up Here --\/
SET CONFIGURATION=SignalCi
SET ProjectName=BillingSuiteWebApp
SET Flavor=SignalCi
REM SET "TargetIISUrl=http://SignalCi:8172/MSDeploy.axd"
SET "TargetIISUrl=https://SignalCiApp:8172/MSDeploy.axd"
REM SET DeployUser=sgnl\devpublish <-- Not an admin for publishing this way!
SET DeployUser=sgnl\pdelosreyes
SET Password=Aug2016!
REM		/\-- Set Up Here --/\
REM **************************
REM * End User Edit Variables*
REM **************************

ECHO %USERNAME% at %Date% %Time%

@ECHO OFF
REM pause

REM Set the Path to MSBuild.exe
REM pushd "%ProgramFiles(x86)%\MSBuild\14.0\Bin"
REM SET MSBuild.exe=MSBuild.exe
REM SET "MSBuild.exe=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"

REM @ECHO ON
REM "%MSBuild.exe:"=%" "%ProjectPath:"=%\%ProjectName%.csproj" /p:Configuration=%CONFIGURATION% /t:Clean
REM ECHO PROJECT CLEAN COMPLETE
REM @ECHO OFF
REM pause

REM @ECHO ON
REM Project being compiled and built 
REM "%MSBuild.exe:"=%" "%ProjectPath:"=%\%ProjectName%.csproj" /p:Configuration=%CONFIGURATION% /t:Build /p:Flavor=%Flavor% /t:SlowCheetah /t:CompileTypeScript
REM ECHO Project Compiled 
REM @ECHO OFF
REM pause

REM @ECHO ON
REM Project being deployed
REM "%MSBuild.exe:"=%" "%ProjectPath%\%ProjectName%.csproj" /p:Configuration=%CONFIGURATION% /p:DeployOnBuild=true /p:CreatePackageOnPublish=True /t:Package /p:PublishProfile=%ProjectPath%Properties\PublishProfiles\App_%Flavor%.pubxml /p:Password=5Bbubble#$R /p:VisualStudioVersion=14.0
REM ECHO DEPLOY PACKAGE CREATED
REM @ECHO OFF
REM pause

REM pushd "%ProjectPath%\Obj\%CONFIGURATION%\Package"

REM @ECHO ON
SET "DeployCmd=%ProjectPath:"=%Obj\%CONFIGURATION:"=%\Package\%ProjectName:"=%.deploy.cmd /y "-skip:objectName=filePath,absolutePath=\\ApplicationFolder\\web\.config$" /m:%TargetIISUrl:"=% /u:%DeployUser:"=% /p:%Password:"=% /a:basic -AllowUntrusted"
ECHO %DeployCmd%
ECHO -
ECHO ***************************************
ECHO -
@ECHO ON
%DeployCmd%
@ECHO OFF
REM ECHO DEPLOYED

REM pushd "%ProjectPath%"