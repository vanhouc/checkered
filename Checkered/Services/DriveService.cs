using Checkered.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Checkered.Models;

namespace Checkered.Services
{
    public static class DriveService
    {
        public static IEnumerable<IDriveData> GetDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed).ToArray();
            return drives.Select<DriveInfo, IDriveData>(d => new DriveData(d.VolumeLabel, d.AvailableFreeSpace, d.TotalSize));
        }
    }
}
