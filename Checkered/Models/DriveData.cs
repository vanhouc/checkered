using Checkered.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Checkered.Models
{
    public class DriveData : IDriveData
    {
        public DriveData(string label, long freeSpace, long totalSpace)
        {
            Label = label;
            FreeSpace = freeSpace;
            TotalSpace = totalSpace;
        }
        public long FreeSpace { get; set; }
        public long TotalSpace { get; set; }
        public string Label { get; set; }
    }
}
