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

        public static List<Item> SearchItems(string name)
        {
            if (name == string.Empty) return Items;

            string searchName = name.ToLowerInvariant();

            List<Item> startItems = new List<Item>();
            List<Item> containItems = new List<Item>();
            foreach (Item item in Items)
            {
                if (string.IsNullOrEmpty(item.Name)) continue;

                string itemName = item.Name.ToLowerInvariant();

                if (itemName.StartsWith(searchName))
                {
                    startItems.Add(item);
                }
                else if (itemName.Contains(searchName))
                {
                    containItems.Add(item);
                }
            }

            startItems.AddRange(containItems);

            return startItems;
        }

        public static void Initialize(List<Item> items)
        {
            Items = items.OrderBy(x => x.Name).ToList();
        }
    }
}
