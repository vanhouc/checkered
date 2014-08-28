using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Checkered.Services.Interfaces;
using Checkered.Services;
using Checkered.Models;
using System.Collections.Generic;
using Checkered.Models.Interfaces;
using System.IO;

namespace CheckeredTests
{
    [TestClass]
    public class XMLConfigServiceTests
    {
        IConfigurationService configService;
        IEnumerable<IConcentrator> concs;
        public XMLConfigServiceTests()
        {
            configService = new XmlConfigService("Test.xml");
            configService.CreateNewConfiguration();
            concs = new List<IConcentrator>()
            {
                new Concentrator("LINKSTATION-VL"),
                new Concentrator("VM1-VERSUSHQ"),
                new Concentrator("VERSUS-VHOST1")
            };
        }
        [TestMethod]
        public void CreateNewFile()
        {
            configService.SaveApplication(new Application()
            {
                DisplayName = "Test App",
                FileName = "TestApp",
                Executable = "Cheese.exe",
                Files = new string[] 
                {
                    "Hello.txt",
                    "fake.csv"
                },
                Folder = @"C:\Test\",
                ProcessName = "Testme"
            });
            var apps = configService.GetApplications();
            Assert.IsTrue(apps.Any(a => a.DisplayName == "Test App"));
        }
        [TestMethod]
        public void SetGetConcentrators()
        {
            configService.SetConcentrators(concs);
            IEnumerable<IConcentrator> fileConcs = configService.GetConcentrators();
            CollectionAssert.AreEquivalent(concs.Select(c => c.Ip).ToList(), fileConcs.Select(c => c.Ip).ToList());
        }
        [ClassCleanup]
        public static void Cleanup()
        {
            File.Delete("Test.xml");
        }
    }
}
