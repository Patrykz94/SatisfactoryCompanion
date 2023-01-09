using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryCompanion.Core
{
    public class ChainItemsHandler
    {
        public List<ChainItem> Items { get; private set; } = new List<ChainItem>();

        public void AddItem(Item item, float quantityPerMinute, Machine? machine, float machineCount)
        {
            bool wasInList = false;
            foreach (var chainItem in Items)
            {
                if (chainItem.Item == item && chainItem.Machine == machine)
                {
                    chainItem.TotalQuantityPerMinute += quantityPerMinute;
                    chainItem.TotalMachineCount += machineCount;
                    wasInList = true;
                    break;
                }
            }

            if (!wasInList)
            {
                ChainItem chainItem = new ChainItem(item, quantityPerMinute, machine, machineCount);
                Items.Add(chainItem);
            }
        }

        private void AddItem(ChainItem chainItem)
        {
            AddItem(chainItem.Item, chainItem.TotalQuantityPerMinute, chainItem.Machine, chainItem.TotalMachineCount);
        }

        public void Merge(ChainItemsHandler itemsHandler)
        {
            foreach (var chainItem in itemsHandler.Items)
            {
                AddItem(chainItem);
            }
        }
    }
}
