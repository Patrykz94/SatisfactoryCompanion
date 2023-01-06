using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryCompanion.Core
{
    public class RecipeInput
    {
        public string? ItemName { get; set; }
        public float Quantity { get; set; }
        public Item? GetItem() => ItemManager.GetItem(ItemName);
    }
}
