using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Checkered.Services.Interfaces;
using Checkered.Services;
using Checkered.Models.Interfaces;
using Checkered.Models;
using System.Collections.Generic;

namespace CheckeredTests
{
    [TestClass]
    public class EndToEndTest
    {
        IConfigurationService configService;
        ISaveService saveService;
        IFileService fileService;
        List<IApplication> testApp;
        List<IConcentrator> concs;
        public EndToEndTest()
        {
            configService = new XmlConfigService("C:\\Backup\\E2E.xml");
            saveService = new CsvService();
            fileService = new FileService();
            testApp = new List<IApplication>()
            {
                new Application()
                {
                    DisplayName = "Transaction Engine",
                    FileName = "TransactionEngine",
                    Folder = @"C:\Versus\server32\",
                    Executable = @"Transaction Engine.exe",
                    Files = new string[]
                        {
                            @"Data\VIS.config",
                            @"Data\"
                        }
                },
                new Application()
                {
                    DisplayName = "Reports Plus",
                    FileName = "ReportsPlus",
                    Folder = @"C:\Versus\ReportsPlus\",
                    Executable = "TracklogBuilder.exe",
                    Files = new string[]
                    {
                        @"TracklogBuilder.ini"
                    }
                }
            };
            concs = new List<IConcentrator>()
            {
                new Concentrator("LINKSTATION-VL"),
                new Concentrator("VM1-VERSUSHQ"),
                new Concentrator("VERSUS-VHOST1")
            };
        }
        [TestMethod]
        public void CreateAndPopulateConfig()
        {
            configService.CreateNewConfiguration();
            testApp.ForEach(a => configService.SaveApplication(a));
            configService.SetConcentrators(concs);
            configService.SetBackupLocation(@"C:\Backup\");
            configService.SetFacilityName("Test Facility");
            configService.Tech = "Cameron VanHouzen";
            for (int i = 0; i < 5; i++)
                concs.ForEach(c => c.UpdatePing(3));
            testApp.ForEach(a => a.Version = fileService.GetFileVersion(a.Folder + a.Executable));
            testApp.ForEach(a => fileService.BackupFiles(a, configService.GetBackupLocation()));
            testApp.ForEach(
                a => a.MemoryUsage = ProcessService.ProcessPrivateMemory(
                new string(a.Executable.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)
                    .Last()
                    .TakeWhile(c => c != '.').ToArray())));
            IEnumerable<IDriveData> drives = DriveService.GetDrives();
            bool saved = false;
            int attempts = 0;
            string savePath = "C:\\Backup\\TestOutput.csv";
            while (!saved && attempts < 5)
            {
                attempts++;
                saved = saveService.SaveData(
                    savePath,
                    configService.GetFacilityName(),
                    configService.Tech,
                    DateTime.Today,
                    drives,
                    ProcessService.TotalCPU(),
                    ProcessService.ProcessCount(),
                    ProcessService.AvailableMemory(),
                    testApp,
                    concs);
                if (!saved)
                    savePath = String.Format("C:\\Backup\\TestOutput({0}).csv", attempts);
            }
            if (!saved)
                throw new ApplicationException("Failed to save out data!");
        }
    }
}
