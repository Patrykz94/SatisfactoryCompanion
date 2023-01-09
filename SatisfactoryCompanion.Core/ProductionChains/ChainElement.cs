using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SatisfactoryCompanion.Core
{
    public class ChainElement
    {
        public Item Item { get; }
        public float QuantityPerMinute { get; set; }
        public bool IsOverflowAllowed { get; set; }
        public List<ChainElement> Elements { get; private set; }
        public bool IsLastStep { get; set; }
        public Machine? Machine => GetMachine();
        public float MachineCount => GetMachineCount();

        public ChainElement(Item itemToProduce, float quantityPerMinute, bool overflowAllowed = true)
        {
            Item = itemToProduce;
            QuantityPerMinute = quantityPerMinute;
            IsOverflowAllowed = overflowAllowed;

            IsLastStep = Item.IsOre;
            Elements = new List<ChainElement>();

            if (!IsLastStep) GenerateChain();
        }

        public ChainItemsHandler GetAllItems()
        {
            ChainItemsHandler allItems = new ChainItemsHandler();
            if (Elements.Count > 0)
            {
                foreach (ChainElement element in Elements)
                {
                    allItems.Merge(element.GetAllItems());
                }
            }

            allItems.AddItem(Item, QuantityPerMinute, Machine, MachineCount);

            return allItems;
        }

        private Machine? GetMachine()
        {
            if (Item.Recipes?.Count > 0)
            {
                //TODO: Add support for multiple recipes
                Recipe recipe = Item.Recipes[0];
                return recipe.Machine;
            }
            else
            {
                if (Item.IsOre)
                {
                    if (Item.Extractors?.Count > 0)
                    {
                        //TODO: Maybe allow choice as to which level extractor to use
                        return Item.Extractors[0];
                    }
                }
            }
            return null;
        }

        private float GetMachineCount()
        {
            if (Item.Recipes?.Count > 0)
            {
                //TODO: Add support for multiple recipes
                Recipe recipe = Item.Recipes[0];
                return QuantityPerMinute / recipe.GetItemsPerMinute();
            }
            else
            {
                if (Item.IsOre)
                {
                    if (Item.Extractors?.Count > 0)
                    {
                        //TODO: Maybe allow choice as to which level extractor to use
                        return QuantityPerMinute / Item.Extractors[0].GetItemsPerMinute();
                    }
                }
            }

            return 0f;
        }

        private void GenerateChain()
        {
            if (Item.Recipes?.Count > 0)
            {
                //TODO: Add support for multiple recipes
                Recipe recipe = Item.Recipes[0];

                if (recipe.Inputs?.Count > 0)
                {
                    List<RecipeInput> inputList = recipe.Inputs;

                    foreach (RecipeInput input in inputList)
                    {
                        //TODO: Show an error here
                        if (input.Item == null) continue;

                        float inputQuantity = input.Quantity / recipe.Output * QuantityPerMinute;

                        ChainElement newElement = new ChainElement(input.Item, inputQuantity, IsOverflowAllowed);

                        Elements.Add(newElement);
                    }
                }
            }
        }
    }
}
