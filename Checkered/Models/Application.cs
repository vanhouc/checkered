using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Checkered.Models.Interfaces;

namespace Checkered.Models
{
    public class Application : IApplication
    {
        public Application()
        {
            DisplayName = "Application";
            Files = new string[0];
            Folder = String.Empty;
            Version = String.Empty;
            MemoryUsage = 0;
            ProcessUsage = 0;
            Executable = String.Empty;
            FileName = "Application";
        }

        public Application(string displayName)
            : this()
        {
            DisplayName = displayName;
        }

        public string DisplayName { get; set; }

        public string Executable { get; set; }

        public string FileName { get; set; }

        public string[] Files { get; set; }

        public string Folder { get; set; }

        public float MemoryUsage { get; set; }
        public string ProcessName { get; set; }

        private Queue<float> _processUsage = new Queue<float>();
        public float ProcessUsage
        {
            get
            {
                return _processUsage.Count > 0 ? _processUsage.Average() : 0;
            }
            set
            {
                if (_processUsage.Count > 30)
                    _processUsage.Dequeue();
                _processUsage.Enqueue(value);
            }
        }

        public string Version { get; set; }
    }
}