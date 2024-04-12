using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTrackingV2
{
    internal class Asset
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Office { get; set; }
        public string PurchDate { get; set; }
        public int inUSD { get; set; }
        public string LocCurr { get; set; }
        public int LocPrice { get; set; }

    }
}
