using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryCompanion.Core
{
    public class ChainItem
    {
        public Item Item { get; }
        public float TotalQuantityPerMinute { get; set; }
        public Machine? Machine { get; }
        public float TotalMachineCount { get; set; }

        public ChainItem(Item item, float totalQuantityPerMinute, Machine? machine, float totalMachineCount)
        {
            Item = item;
            TotalQuantityPerMinute = totalQuantityPerMinute;
            Machine = machine;
            TotalMachineCount = totalMachineCount;
        }
    }
}
