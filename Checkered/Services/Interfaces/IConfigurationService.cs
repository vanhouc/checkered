using Checkered.Models.Interfaces;
using System.Collections.Generic;
namespace Checkered.Services.Interfaces
{
    public interface IConfigurationService
    {
        IEnumerable<IApplication> GetApplications();
        void CreateNewConfiguration();
        void SaveApplication(IApplication toSave);
        void SetBackupLocation(string path);
        string GetBackupLocation();
        IEnumerable<IConcentrator> GetConcentrators();
        void SetConcentrators(IEnumerable<IConcentrator> concentrators);
        void AddConcentrator(IConcentrator concentrator);
        void SetFacilityName(string facilityName);
        string GetFacilityName();
        string Tech { get; set; }
    }
}