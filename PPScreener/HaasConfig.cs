using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace PPScreener
{
    public class HaasConfig
    {
        // Default Configuration Settings
        public string IPAddress { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 8096;
        public string Secret { get; set; } = "SomeSecretHere";
        public string AccountGUID { get; set; } = "ReplaceMeWithGuid";

        // Custom Configuration Settings
        public int RetryCount = 10;
        public decimal KeepThreshold = 2.0m;
        public int BackTestDelayInMiliseconds = 3000;
        public decimal Fee { get; set; } = 0.1m;
        public bool PersistBots { get; set; } = true;
        public int BackTestLength { get; set; } = 1440;
        public string PrimaryCurrency { get; set; } = "BTC";
        public bool WriteResultsToFile { get; set; } = false;
       

    }
}
