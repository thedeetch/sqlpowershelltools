PowerShell Tools for SSRS
=========
PowerShell cmdlets to make interacting with SQL Server Reporting Services simpler.

Installing
=========
Two ways:

1. Use the Import-Module cmdlet to import the SrsPowerShellTools.dll directly

  ```PowerShell
  Import-Module "<MyDirectory>\SrsPowerShellTools.dll"
  ```
2. Put the SrsPowerShellTools.dll and SrsPowerShellTools.psd1 files in one of the $env:PSModulePath directories and use Import-Module

  ```PowerShell
  Import-Module SrsPowerShellTools
  ```

Cmdlets
=========
Invoke-SrsReport
---------
```PowerShell
Invoke-SrsReport "http://myserver/ReportServer/ReportExecution2005.asmx" "/MyFolder/MyReport" "PDF" @{ "Parameter1"="Value1", "Parameter2"="Value2"} 
```

License
=========
PowerShell Tools for SSRS is released under the [MIT License](http://www.opensource.org/licenses/MIT).
