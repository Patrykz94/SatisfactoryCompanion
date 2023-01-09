using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SatisfactoryCompanion.Core
{
    public class ProductionChain
    {
        public Item Item { get; }
        public float QuantityPerMinute { get; set; }
        public bool IsOverflowAllowed { get; set; }
        public ChainElement FirstElement { get; private set; }

        public ProductionChain(Item itemToProduce, float quantityPerMinute, bool overflowAllowed = false)
        {
            Item = itemToProduce;
            QuantityPerMinute = quantityPerMinute;
            IsOverflowAllowed = overflowAllowed;

            FirstElement = new ChainElement(Item, QuantityPerMinute, IsOverflowAllowed);
        }

        public ChainItemsHandler GetTotalItems()
        {
            return FirstElement.GetAllItems();
        }

        public override string ToString()
        {
            return $"Production chain producing {QuantityPerMinute} of {Item} per minute.";
        }
    }
}
