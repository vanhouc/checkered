using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Checkered.Models.Interfaces
{
    public interface IDriveData
    {
        string Label { get; set; }
        long FreeSpace { get; set; }
        long TotalSpace { get; set; }
    }
}
