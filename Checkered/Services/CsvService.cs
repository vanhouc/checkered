using Checkered.Models.Interfaces;
using Checkered.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Checkered.Services
{
    public class CsvService : ISaveService
    {

        public bool SaveData(
            string path,
            string facilityName,
            string tech,
            DateTime date,
            IEnumerable<IDriveData> driveData,
            float totalCPU,
            int processCount,
            float availableMemory,
            IEnumerable<IApplication> applications,
            IEnumerable<IConcentrator> concs)
        {
            if (File.Exists(path))
                return false;
            using (StreamWriter sw = File.CreateText(path))
            {
                //Performance Section
                sw.WriteLine("{0},Current,{1}", facilityName, date.ToShortDateString());
                sw.WriteLine("Tech: {0},Score,Versus is on C:", tech);
                sw.WriteLine("Performance Section,Score,Data");
                sw.WriteLine("CPU Performance(AUTOMATED),{0},{1}%",
                    ScoreCPUUsage(totalCPU),
                    totalCPU.ToString("F2"));
                sw.WriteLine("Available Memory(AUTOMATED),N/A,{0}MB",
                    availableMemory.ToString("F0"));
                sw.WriteLine("Number of Processes(AUTOMATED),N/A,{0}",
                    processCount);
                foreach (IDriveData drive in driveData)
                    sw.WriteLine("Hard Drive Space {0} (Free/Total) {0},{1},{2}GB/{3}GB",
                        drive.Label,
                        (100 * (Convert.ToDouble(drive.FreeSpace) / Convert.ToDouble(drive.TotalSpace))).ToString("F0"),
                        (drive.FreeSpace / (Math.Pow(1024, 3))).ToString("F0"),
                        (drive.TotalSpace / (Math.Pow(1024, 3))).ToString("F0"));
                sw.WriteLine("Core Software Version Verification,N/A,N/A");
                sw.WriteLine("Versus Log File Maintainance,N/A,N/A");
                sw.WriteLine("Versus Config Database Backups,N/A,N/A");
                //Backup Section
                sw.WriteLine("Backup Section,,Backed-Up");
                foreach (string file in applications.Where(a => a.Version != String.Empty).SelectMany(a => a.Files))
                    sw.WriteLine("{0},,x", file);
                //Installed Software Section
                sw.WriteLine("Installed Software Section/Version,Score,Proc/Memory");
                foreach (IApplication app in applications.Where(a => a.Version != String.Empty))
                    sw.WriteLine("{0} {1},{2},{3}/{4}Kb",
                        new string(app.Executable.TakeWhile(c => c != '.').ToArray()), //0
                        app.Version, //1
                        ScoreCPUUsage(app.ProcessUsage), //2
                        (app.ProcessUsage > 0 ? app.ProcessUsage / 100 : 0).ToString("P"), //3
                        (app.MemoryUsage > 0 ? app.MemoryUsage / 1024 : 0).ToString()); //4

                //Versus System Health Section
                sw.WriteLine("Versus System Health, Score, Data");
                sw.WriteLine("Low Battery Report(# Badges),,");
                sw.WriteLine("Event Visit Report (# Patients Previous Day),,");
                sw.WriteLine("Unassigned Names Delete Function,,");
                sw.WriteLine("Number Deleted Names,,");
                sw.WriteLine("Sensor Tester Results,,");
                sw.WriteLine("Collectors Not Reporting Data,,");
                //Concentrator Tests Section
                sw.WriteLine("Concentrator Tests,Score,Data");
                sw.WriteLine("Concentrator Ping Test Results,N/A,{0}ms",
                    concs.Select(c => c.ConcPing).Average().ToString("N1"));
                sw.WriteLine("Concentrators not replying,{0},{1}",
                    100 - (concs.Where(c => c.ConcPing == 0).Count() * 10),
                    String.Join("|", concs.Where(c => c.ConcPing == 0).Select(c => c.Ip.ToString()).ToArray()));
                foreach (IConcentrator conc in concs)
                    sw.WriteLine("{0},N/A,{1}ms", conc.Ip, conc.ConcPing.ToString("F1"));
                return true;
            }
        }
        private int ScoreCPUUsage(float cpuUsage)
        {
            if (cpuUsage >= 0 && cpuUsage <= 3)
                return 100;
            if (cpuUsage >= 3 && cpuUsage <= 5)
                return 95;
            if (cpuUsage >= 5 && cpuUsage <= 10)
                return 90;
            if (cpuUsage > 10 && cpuUsage <= 15)
                return 85;
            if (cpuUsage > 15 && cpuUsage <= 20)
                return 80;
            if (cpuUsage > 20 && cpuUsage <= 25)
                return 75;
            if (cpuUsage > 25 && cpuUsage <= 30)
                return 70;
            if (cpuUsage > 30 && cpuUsage <= 35)
                return 65;
            if (cpuUsage > 35 && cpuUsage <= 40)
                return 60;
            if (cpuUsage > 40 && cpuUsage <= 45)
                return 55;
            if (cpuUsage > 45 && cpuUsage <= 50)
                return 50;
            if (cpuUsage > 50 && cpuUsage <= 55)
                return 45;
            if (cpuUsage > 55 && cpuUsage <= 60)
                return 40;
            if (cpuUsage > 60 && cpuUsage <= 65)
                return 35;
            if (cpuUsage > 65 && cpuUsage <= 70)
                return 30;
            if (cpuUsage > 70 && cpuUsage <= 75)
                return 25;
            if (cpuUsage > 75 && cpuUsage <= 80)
                return 20;
            if (cpuUsage > 80 && cpuUsage <= 85)
                return 15;
            if (cpuUsage > 85 && cpuUsage <= 90)
                return 10;
            if (cpuUsage > 90 && cpuUsage <= 95)
                return 5;
            if (cpuUsage > 95 && cpuUsage <= 100)
                return 0;
            return 0;
        }
    }
}
