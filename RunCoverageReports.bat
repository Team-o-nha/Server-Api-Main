@echo off
setlocal
rem pushd %~dp0..

dotnet test /p:CollectCoverage=true /p:CoverletOutput=BuildReports/Coverage/ /p:CoverletOutputFormat=cobertura /p:Exclude='[xunit.*]*'

reportgenerator -reports:Tests/**/BuildReports/Coverage/coverage.cobertura.xml -targetdir:Tests/BuildReports/Coverage -reporttypes:"HTML;HTMLSummary"

start Tests\BuildReports\Coverage\index.htm