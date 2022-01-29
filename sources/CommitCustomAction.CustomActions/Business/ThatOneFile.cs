// WiX Toolset Pills 15mg
// Copyright (C) 2019-2021 Dust in the Wind
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
using System.IO;

namespace CommitCustomAction.CustomActions.Business
{
    internal class ThatOneFile
    {
        private readonly string filePath;
        private readonly string temporaryFilePath;

        public ThatOneFile(string filePath)
        {
            this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

            if (filePath.Length == 0)
                throw new WixToolsetPillException("The one file was not provided.");

            if (!File.Exists(filePath)) 
                CreateFile(filePath);

            string fileName = Path.GetFileName(filePath);
            string temporaryDirectoryPath = Path.GetTempPath();
            temporaryFilePath = Path.Combine(temporaryDirectoryPath, fileName);
        }

        private static void CreateFile(string filePath)
        {
            using (StreamWriter streamWriter = File.CreateText(filePath))
            {
                streamWriter.WriteLine("This file was created by the 'Commit Custom Action' WiX Toolset Pill.");
                streamWriter.WriteLine(new string('=', 100));
                streamWriter.WriteLine();
            }
        }

        public string CreateBackup()
        {
            File.Copy(filePath, temporaryFilePath, true);
            return temporaryFilePath;
        }

        public void AddMessage()
        {
            using (StreamWriter streamWriter = File.AppendText(filePath))
            {
                string message = $"The CommitCustomAction product was executed. Current time is: {DateTime.Now}";
                streamWriter.WriteLine(message);
            }
        }

        public void RestoreFromBackup()
        {
            if (!File.Exists(temporaryFilePath))
                throw new WixToolsetPillWarningException($"Could not rollback the changes to file '{filePath}'. The temporary backup was not found.");

            File.Copy(temporaryFilePath, filePath, true);
        }

        public string DeleteBackup()
        {
            if (File.Exists(temporaryFilePath))
                File.Delete(temporaryFilePath);

            return temporaryFilePath;
        }
    }
}