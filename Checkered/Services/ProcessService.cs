using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Checkered.Services
{
    public static class ProcessService
    {
        public static float AvailableMemory()
        {
            PerformanceCounter memCounter = new PerformanceCounter("Memory", "Available MBytes");
            return memCounter.NextValue();
        }

        public static float ProcessCPU(string processName)
        {
            try
            {
                PerformanceCounter cpuCounter = new PerformanceCounter();
                cpuCounter.CategoryName = "Process";
                cpuCounter.CounterName = "% Processor Time";
                cpuCounter.InstanceName = processName;
                cpuCounter.NextValue();
                Thread.Sleep(1000);
                return cpuCounter.NextValue();
            }
            catch
            {
                return 0;
            }
        }

        public static float ProcessPrivateMemory(string processName)
        {
            try
            {
                Process proc = Process.GetProcessesByName(processName).First();
                return proc.PrivateMemorySize64;
            }
            catch
            {
                return 0;
            }
        }

        public static float TotalCPU()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
            cpuCounter.NextValue();
            Thread.Sleep(1000);
            return cpuCounter.NextValue();
        }
        public static int ProcessCount()
        {
            Process[] allProcs = Process.GetProcesses();
            return allProcs.Length;
        }
    }
}
