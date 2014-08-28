using Checkered.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;

namespace Checkered.Models
{
    public class Concentrator : IConcentrator
    {
        public Concentrator(string ip)
        {
            Ip = ip;
        }
        public string Ip { get; set; }
        private Queue<double> _concPing = new Queue<double>();
        public double ConcPing
        {
            get
            {
                if (_concPing.Count > 0)
                    return _concPing.Average();
                else
                    return 0;
            }
            set
            {
                if (_concPing.Count > 30)
                    _concPing.Dequeue();
                _concPing.Enqueue(value);
            }
        }
        public void UpdatePing(int pingCount)
        {
            try
            {
                for (int i = 0; i < pingCount; i++)
                {
                    Ping ping = new Ping();
                    PingReply reply = ping.Send(Ip, 1000);
                    ConcPing = Convert.ToInt32(reply.RoundtripTime);
                }
            }
            catch { }
        }
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            IConcentrator conc = obj as IConcentrator;
            if (conc != null)
            {
                if (this.Ip == conc.Ip)
                    return 0;
                else
                    return this.Ip.Length > conc.Ip.Length ? 1 : -1;
            }
            else
                throw new ArgumentException("Object is not an IConcentrator");
        }
        public override string ToString()
        {
            return Ip;
        }
    }
}
