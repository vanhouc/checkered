using Checkered.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkered.Services.Interfaces
{
    public interface IFileService
    {
        void BackupFiles(IApplication toBackup, string backupPath);
        string GetFileVersion(string path);
    }
}
