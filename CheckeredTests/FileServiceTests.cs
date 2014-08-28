using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Checkered.Models.Interfaces;
using Checkered.Models;
using Checkered.Services.Interfaces;
using Checkered.Services;
using System.IO;

namespace CheckeredTests
{
    [TestClass]
    public class FileServiceTests
    {
        IApplication testApp;
        IConfigurationService configService;
        [TestInitialize]
        public void Init()
        {
            testApp = new Application()
            {
                DisplayName = "Transaction Engine",
                FileName = "TransactionEngine",
                Folder = @"C:\Versus\server32\",
                Executable = @"Data\Transaction Engine.exe",
                Files = new string[]
                    {
                        @"Data\VIS.config"
                    }
            };
            configService = new XmlConfigService("FileTests.xml");
            configService.CreateNewConfiguration();
            configService.SaveApplication(testApp);
            configService.SetBackupLocation(@"C:\Backup\");
        }
        [TestMethod]
        public void BackupTest()
        {
            DateTime today = DateTime.Today;
            IFileService fileService = new FileService();
            fileService.BackupFiles(testApp, configService.GetBackupLocation());
            Assert.IsTrue(File.Exists(String.Format("{0}{1}{2}{3}.zip", configService.GetBackupLocation(), today.Year.ToString(), today.Month.ToString(), today.Day.ToString())));
        }
    }
}
