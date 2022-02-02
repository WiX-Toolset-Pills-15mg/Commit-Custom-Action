// WiX Toolset Pills 15mg
// Copyright (C) 2019-2022 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using DustInTheWind.CommitCustomAction.CustomActions.Business;
using DustInTheWind.CommitCustomAction.CustomActions.Utils;
using Microsoft.Deployment.WindowsInstaller;

namespace DustInTheWind.CommitCustomAction.CustomActions
{
    public class RollbackAddMessageToFileCustomAction
    {
        [CustomAction("RollbackAddMessageToFile")]
        public static ActionResult Execute(Session session)
        {
            return CustomActionExecution.Run(session, () =>
            {
                try
                {
                    string filePath = session.CustomActionData["FilePath"];

                    ThatOneFile thatOneFile = new ThatOneFile(filePath);

                    thatOneFile.RestoreFromBackup();
                    session.Log($"The file '{filePath}' was successfully rolled back to its initial state.");

                    string backupFilePath = thatOneFile.DeleteBackup();
                    session.Log($"The backup file '{backupFilePath}' was successfully deleted.");
                }
                catch (WixToolsetPillWarningException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new WixToolsetPillWarningException("Failed to rollback", ex);
                }
            });
        }
    }
}