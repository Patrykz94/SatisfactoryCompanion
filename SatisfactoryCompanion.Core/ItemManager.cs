using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryCompanion.Core
{
    public static class ItemManager
    {
        public static List<Item> Items { get; private set; } = new List<Item>();

        public static Item? GetItem(string? name)
        {
            foreach (Item item in Items)
            {
                if (item.Name == name) return item;
            }

            return null;
        }

        public static void Initialize(List<Item> items)
        {
            Items = items;
        }
    }
}
