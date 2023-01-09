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
        public Item? Item => ItemManager.GetItem(ItemName);
        public float Quantity { get; set; }
    }
}
