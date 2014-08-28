using Checkered.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkered.Services.Interfaces
{
    public interface ISaveService
    {
        bool SaveData(string path,
            string facilityName,
            string tech,
            DateTime date,
            IEnumerable<IDriveData> driveData,
            float totalCPU,
            int processCount,
            float availableMemory,
            IEnumerable<IApplication> applications,
            IEnumerable<IConcentrator> concs);
    }
}
