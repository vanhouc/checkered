using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Checkered.Services.Interfaces;
using Checkered.Services;
using Checkered.Models;
using Checkered.Models.Interfaces;
using System.Threading;

namespace CheckeredTests
{
    [TestClass]
    public class ProcessServiceTests
    {
        IApplication testApp;
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
            IConfigurationService configService = new XmlConfigService("ProcessTests.xml");
            configService.CreateNewConfiguration();
            configService.SaveApplication(testApp);
        }
        [TestMethod]
        public void MemoryTest()
        {
            Assert.IsTrue(ProcessService.AvailableMemory() > 0);
        }
        [TestMethod]
        public void ProcessTest()
        {
            float processValue = ProcessService.ProcessCPU(new string(testApp.Executable.Split(new char[] {'\\'}, StringSplitOptions.RemoveEmptyEntries).Last().TakeWhile(c => c != '.').ToArray()));
            float totalUsage = ProcessService.TotalCPU();
            Assert.IsTrue(totalUsage > 0);
        }
    }
}
