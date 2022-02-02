:: ====================================================================================================
:: Step 7: Run the installer with error
:: ====================================================================================================
:: 
:: Run the installer and look into the "install.log" file.
:: Search for the "AddMessageToFile" and "CommitAddMessageToFile" custom action execution.
:: Note that the "RollbackAddMessageToFile" custom action was not executed.
:: 
:: NEXT: install.bat

msiexec /i CommitCustomAction.msi /l*vx install-with-error.log FILE_PATH="c:\Temp\file.txt" WIXFAILWHENDEFERRED=1