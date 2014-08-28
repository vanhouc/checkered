using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Checkered.Models;
using Checkered.Services.Interfaces;
using Checkered.Models.Interfaces;

namespace Checkered.Services
{
    public class XmlConfigService : IConfigurationService
    {
        private string path;
        public XmlConfigService(string path)
        {
            this.path = path;
        }
        public IEnumerable<IApplication> GetApplications()
        {
            try
            {
                XDocument doc = XDocument.Load(path);
                Application[] appList = (from app in doc.Root.Element("Applications").Elements()
                                         select new Application()
                                         {
                                             DisplayName = app.Element("Display").Value,
                                             Folder = app.Element("Folder").Value,
                                             Executable = app.Element("Executable").Value,
                                             Files = app.Elements("File").Select(x => x.Value).ToArray(),
                                         }).ToArray();
                return appList;
            }
            catch
            {
                return new Application[0];
            }
        }
        public void SaveApplication(IApplication toSave)
        {
            XDocument doc = XDocument.Load(path);
            XElement toRemove = doc.Element(toSave.FileName);
            if (toRemove != null)
                toRemove.Remove();
            if (doc.Root.Element("Applications") == null)
                doc.Root.Add(new XElement("Applications"));
            XElement[] files = toSave.Files.Select(f => new XElement("File", f)).ToArray();
            doc.Root.Element("Applications").Add(
                new XElement(toSave.FileName,
                    new XElement("Display", toSave.DisplayName),
                    new XElement("Folder", toSave.Folder),
                    new XElement("Executable", toSave.Executable),
                    files));
            doc.Save(path);
        }
        public void CreateNewConfiguration()
        {
            XDocument doc = new XDocument(
                new XElement("Configuration",
                    new XElement("FacilityName"),
                    new XElement("Tech"),
                    new XElement("Applications",
                        new XComment(
                            Environment.NewLine +
                            new XElement("ExampleApp",
                                new XElement("Display", "Example Application"),
                                new XElement("Folder", "C:\\Versus\\ExampleApp\\"),
                                new XElement("Executable", "example.exe"),
                                new XElement("Files",
                                    new XElement("File", "important.ini"),
                                    new XElement("File", "Stuff\\veryimportant.txt"),
                                    new XElement("File", "Data\\"))).ToString(SaveOptions.None) +
                            Environment.NewLine)),
                    new XElement("Concentrators",
                        new XComment(
                            Environment.NewLine +
                            new XElement("Concentrator", "EXAMPLE-DNSHOST").ToString(SaveOptions.None) +
                            Environment.NewLine +
                            new XElement("Concentrator", "127.0.0.1").ToString(SaveOptions.None) +
                            Environment.NewLine))));
            doc.Save(path);
        }
        public void SetBackupLocation(string backupPath)
        {
            XDocument doc = XDocument.Load(path);
            doc.Root.Add(new XElement("Backup", backupPath));
            doc.Save(path);
        }
        public string GetBackupLocation()
        {
            XDocument doc = XDocument.Load(path);
            return doc.Root.Element("Backup").Value;
        }
        /// <summary>
        /// Returns value of the specified child for all children of an XML element
        /// </summary>
        /// <param name="elementChain">Logical heirarchy with the desired element being the last in the chain</param>
        /// <param name="childProperty"></param>
        /// <returns>array of values from the children with the specified name</returns>
        private string[] GetChildValues(XDocument doc, string[] elementChain, string childProperty)
        {
            IEnumerable<XElement> scope = doc.Descendants();
            for (int i = 0; i < elementChain.Length - 1; i++)
            {
                scope = scope.Single(x => x.Name == elementChain[i]).Descendants();
            }
            return scope.Elements(childProperty).Select(x => x.Value).ToArray();
        }

        /// <summary>
        /// Returns the value of a unique XML element
        /// </summary>
        /// <param name="elementChain">Logical heirarchy with the desired element being the last in the chain</param>
        /// <returns>array of the specified element</returns>
        private string GetElementValue(XDocument doc, string[] elementChain)
        {
            IEnumerable<XElement> scope = doc.Descendants();
            for (int i = 0; i < elementChain.Length - 1; i++)
            {
                scope = scope.Single(x => x.Name == elementChain[i]).Descendants();
            }
            return scope.Single(x => x.Name == elementChain[elementChain.Length - 1]).Value;
        }


        public IEnumerable<IConcentrator> GetConcentrators()
        {
            XDocument doc = XDocument.Load(path);
            return doc.Root.Element("Concentrators").Elements("Concentrator").Select<XElement, IConcentrator>(c => new Concentrator(c.Value));
        }

        public void SetConcentrators(IEnumerable<IConcentrator> concentrators)
        {
            XDocument doc = XDocument.Load(path);
            if (doc.Element("Concentrators") != null)
                doc.Element("Concentrators").Remove();
            doc.Root.Add(new XElement("Concentrators"));
            doc.Root.Element("Concentrators").Add(concentrators.Select(c => new XElement("Concentrator", c.Ip)));
            doc.Save(path);
        }

        public void AddConcentrator(IConcentrator concentrator)
        {
            XDocument doc = XDocument.Load(path);
            doc.Root.Element("Concentrators").Add(new XElement("Concentrator", concentrator.Ip));
            doc.Save(path);
        }
        public void SetFacilityName(string facilityName)
        {
            XDocument doc = XDocument.Load(path);
            if (doc.Root.Element("FacilityName") != null)
                doc.Root.Element("FacilityName").Remove();
            doc.Root.Add(new XElement("FacilityName", facilityName));
            doc.Save(path);
        }
        public string GetFacilityName()
        {
            XDocument doc = XDocument.Load(path);
            if (doc.Root.Element("FacilityName") != null)
                return doc.Root.Element("FacilityName").Value;
            else
                return String.Empty;
        }
        public string Tech 
        { 
            get
            {
                XDocument doc = XDocument.Load(path);
                if (doc.Root.Element("Tech") != null)
                    return doc.Root.Element("Tech").Value;
                else
                    return String.Empty;
            }
            set
            {
                XDocument doc = XDocument.Load(path);
                if (doc.Root.Element("Tech") != null)
                    doc.Root.Element("Tech").Remove();
                doc.Root.Add(new XElement("Tech", value));
                doc.Save(path);
            }
        }
    }
}