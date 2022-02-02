:: ====================================================================================================
:: Step 8: Run the installer with error
:: ====================================================================================================
:: 
:: Run the installer and look into the "install-with-error.log" file.
:: Search for the "AddMessageToFile" and "RollbackAddMessageToFile" custom action execution.
:: Note that the "CommitAddMessageToFile" custom action was not executed.
:: 
:: END

msiexec /i CommitCustomAction.msi /l*vx install.log FILE_PATH="c:\Temp\file.txt"