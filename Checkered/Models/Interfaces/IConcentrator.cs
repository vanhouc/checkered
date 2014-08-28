using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkered.Models.Interfaces
{
    public interface IConcentrator : IComparable
    {
        string Ip { get; set; }
        double ConcPing { get; set;}
        void UpdatePing(int pingCount);
    }
}
