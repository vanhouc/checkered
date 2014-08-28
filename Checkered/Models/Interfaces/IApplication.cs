using System;
namespace Checkered.Models.Interfaces
{
    public interface IApplication
    {
        string DisplayName { get; set; }
        string Executable { get; set; }
        string FileName { get; set; }
        string[] Files { get; set; }
        string Folder { get; set; }
        float MemoryUsage { get; set; }
        string ProcessName { get; set; }
        float ProcessUsage { get; set; }
        string Version { get; set; }
    }
}
