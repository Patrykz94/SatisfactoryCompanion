using SatisfactoryCompanion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryCompanion
{
    internal class ConsoleMenu
    {
        Item? selectedItem = null;
        float quantity = 0f;

        ConsoleColor RBC = ConsoleColor.Gray;
        ConsoleColor RFC = ConsoleColor.Black;
        ConsoleColor BC = ConsoleColor.Black;
        ConsoleColor FC = ConsoleColor.Gray;
        ConsoleColor IC = ConsoleColor.Green;
        ConsoleColor MC = ConsoleColor.Yellow;

        public void ShowItemSelectionScreen()
        {
            int maxResultsToDisplay = Console.WindowHeight - 6;
            bool confirmed = false;
            string searchName = "";
            int resultSelected = 0;

            while (!confirmed)
            {
                Console.Clear();
                ConsoleWriter.WriteLine($"You can use {{BC={RBC}}}{{FC={RFC}}} Up Arrow {{/FC}}{{/BC}} and {{BC={RBC}}}{{FC={RFC}}} Down Arrow {{/FC}}{{/BC}} keys to select the item, and press {{BC={RBC}}}{{FC={RFC}}} Enter {{/FC}}{{/BC}} to confirm.");
                Console.WriteLine();
                string topLine = $"Start typing item name to search: ";
                ConsoleWriter.WriteLine($"{topLine}{{FC=Green}}{searchName}{{/FC}}");

                List<Item> itemsFound = ItemManager.SearchItems(searchName);
                if (itemsFound.Count > maxResultsToDisplay)
                    Console.WriteLine($"\nFound {itemsFound.Count} results. Displaying top {maxResultsToDisplay}:");
                else if (itemsFound.Count > 0)
                    Console.WriteLine($"\nFound {itemsFound.Count} results:");
                else
                    Console.WriteLine("\nNo results found! Please change your search input.");

                resultSelected = Math.Max(0, Math.Min(Math.Min(itemsFound.Count - 1, maxResultsToDisplay - 1), resultSelected));

                for (int i = 0; i < Math.Min(itemsFound.Count, maxResultsToDisplay); i++)
                {
                    if (i == resultSelected)
                    {
                        ConsoleWriter.WriteLine($"{{BC={RBC}}}{{FC={RFC}}} {itemsFound[i].Name} {{/FC}}{{/BC}}");
                    }
                    else
                    {
                        Console.WriteLine($" {itemsFound[i].Name} ");
                    }
                }

                Console.SetCursorPosition(topLine.Length + searchName.Length, 2);

                ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    if (itemsFound.Count > 0)
                    {
                        selectedItem = itemsFound[resultSelected];
                        ShowQuantityEntryScreen();
                        searchName = "";
                        resultSelected = 0;
                    }
                }
                else if ((keyPressed.Key >= ConsoleKey.D0 && keyPressed.Key <= ConsoleKey.Z)
                    || (keyPressed.Key >= ConsoleKey.NumPad0 && keyPressed.Key <= ConsoleKey.NumPad9)
                    || keyPressed.Key == ConsoleKey.Decimal
                    || keyPressed.Key == ConsoleKey.OemPeriod
                    || keyPressed.Key == ConsoleKey.OemComma
                    || keyPressed.Key == ConsoleKey.Spacebar)
                {
                    searchName += keyPressed.KeyChar;
                }
                else if (keyPressed.Key == ConsoleKey.Backspace && searchName.Length > 0)
                {
                    searchName = searchName.Remove(searchName.Length - 1);
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    resultSelected--;
                }
                else if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    resultSelected++;
                }
            }
        }

        private void ShowQuantityEntryScreen()
        {
            bool returnToItemSelection = false;
            float quantityEntered = 0f;
            string quantityEnteredString = "";

            while (!returnToItemSelection)
            {
                Console.Clear();
                ConsoleWriter.WriteLine($"Press {{BC={RBC}}}{{FC={RFC}}} Enter {{/FC}}{{/BC}} to confirm.");
                Console.WriteLine();
                ConsoleWriter.WriteLine($"Selected Item: {{FC=Green}}{selectedItem?.Name}{{/FC}}");
                string topLine = $"Enter quantity: {quantityEnteredString}";
                Console.WriteLine(topLine);

                Console.SetCursorPosition(topLine.Length, 3);

                ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                if (keyPressed.Key == ConsoleKey.Enter)
                {
                    if (quantityEntered > 0f)
                    {
                        quantity = quantityEntered;
                        returnToItemSelection = ShowProductionChain();
                    }
                }
                else if (keyPressed.Key == ConsoleKey.Backspace && quantityEnteredString.Length > 0)
                {
                    quantityEnteredString = quantityEnteredString.Remove(quantityEnteredString.Length - 1);
                }
                else if (keyPressed.Modifiers == 0)
                {
                    if ((keyPressed.Key >= ConsoleKey.D0 && keyPressed.Key <= ConsoleKey.D9)
                        || (keyPressed.Key >= ConsoleKey.NumPad0 && keyPressed.Key <= ConsoleKey.NumPad9))
                    {
                        quantityEnteredString += keyPressed.KeyChar;
                    }
                    else if (keyPressed.Key == ConsoleKey.Decimal
                        || keyPressed.Key == ConsoleKey.OemPeriod)
                    {
                        if (!quantityEnteredString.Contains('.'))
                        {
                            quantityEnteredString += keyPressed.KeyChar;
                        }
                    }
                }

                string correctedString = quantityEnteredString;
                if (quantityEnteredString.EndsWith(",") || quantityEnteredString.EndsWith("."))
                {
                    correctedString = quantityEnteredString.Remove(quantityEnteredString.Length - 1);
                }

                float.TryParse(correctedString, out quantityEntered);
            }
        }

        /// <summary>
        /// Function that displays the production chain info
        /// </summary>
        /// <returns>True if we want to change the item, or False if we just want to go back to quantity selection</returns>
        private bool ShowProductionChain()
        {
            if (selectedItem == null) return true;

            Console.Clear();
            ConsoleWriter.WriteLine($"Press {{BC={RBC}}}{{FC={RFC}}} Q {{/FC}}{{/BC}} to change quantity, {{BC={RBC}}}{{FC={RFC}}} S {{/FC}}{{/BC}} to search for a new item or {{BC={RBC}}}{{FC={RFC}}} E {{/FC}}{{/BC}} to exit");
            Console.WriteLine();
            Console.WriteLine("Production Chain (Tree View):");
            Console.WriteLine();

            ProductionChain chain = new ProductionChain(selectedItem, quantity);

            PrintElementTree(chain.FirstElement);

            Console.WriteLine();
            ConsoleWriter.WriteLine($"Total {{FC={IC}}}Items{{/FC}} and {{FC={MC}}}Machines{{/FC}} required:");
            Console.WriteLine();
            PrintTotals(chain.GetTotalItems());

            Console.WriteLine();
            ConsoleWriter.WriteLine($"Use mouse {{BC={RBC}}}{{FC={RFC}}} Scroll Wheel {{/FC}}{{/BC}} to navigate up and down this page");
            ConsoleWriter.Write($"Press {{BC={RBC}}}{{FC={RFC}}} Q {{/FC}}{{/BC}} to change quantity, {{BC={RBC}}}{{FC={RFC}}} S {{/FC}}{{/BC}} to search for a new item or {{BC={RBC}}}{{FC={RFC}}} E {{/FC}}{{/BC}} to exit");
            
            while (true)
            {
                ConsoleKeyInfo keyPressed = Console.ReadKey(true);
                if (keyPressed.Key == ConsoleKey.Q)
                {
                    return false;
                }
                else if (keyPressed.Key == ConsoleKey.S)
                {
                    return true;
                }
                else if (keyPressed.Key == ConsoleKey.E)
                {
                    Environment.Exit(0);
                }
            }
        }


        private void PrintElementTree(ChainElement element, bool isRootItem = true, string indent = "", bool isLastSibling = true)
        {
            List<ChainElement> children = element.Elements;

            if (isRootItem)
            {
                // prepare the strings
                string itemName = element.Item.Name ?? "";
                string machineName = element.Machine?.Name ?? "";
                int strLen = Math.Max(itemName.Length, machineName.Length);
                itemName = itemName.PadRight(strLen);
                machineName = machineName.PadRight(strLen);
                ConsoleWriter.WriteLine($"{{BC={BC}}}{{FC={FC}}}Item:    {{FC={IC}}}{itemName}{{/FC}} Qty: {{FC={IC}}}{element.QuantityPerMinute:0.##}{{/FC}}{{/FC}}{{/BC}}");
                ConsoleWriter.WriteLine($"{{BC={BC}}}{{FC={FC}}}Machine: {{FC={MC}}}{machineName}{{/FC}} Qty: {{FC={MC}}}{element.MachineCount:0.##}{{/FC}}{{/FC}}{{/BC}}");
            }
            else
            {
                // prepare the strings
                string itemName = element.Item.Name ?? "";
                string machineName = element.Machine?.Name ?? "";
                int strLen = Math.Max(itemName.Length, machineName.Length);
                itemName = itemName.PadRight(strLen);
                machineName = machineName.PadRight(strLen);

                string futureIndent = indent + (isLastSibling ? "        " : "┃       ");
                string tempIndent = indent + "┃       ";
                ConsoleWriter.WriteLine($"{{FC={FC}}}{tempIndent}{{/FC}}");
                ConsoleWriter.WriteLine($"{{FC={FC}}}{indent}{(isLastSibling ? "┗━━━━━━╸" : "┣━━━━━━╸")}{{BC={BC}}}Item:    {{FC={IC}}}{itemName}{{/FC}} Qty: {{FC={IC}}}{element.QuantityPerMinute:0.##}{{/FC}}{{/BC}}{{/FC}}");
                ConsoleWriter.WriteLine($"{{FC={FC}}}{futureIndent}{{BC={BC}}}Machine: {{FC={MC}}}{machineName}{{/FC}} Qty: {{FC={MC}}}{element.MachineCount:0.##}{{/FC}}{{/BC}}{{/FC}}");
                if (isLastSibling && children.Count == 0)
                {
                    ConsoleWriter.WriteLine($"{{FC={FC}}}{indent}{{/FC}}");
                }

                indent = futureIndent;
            }

            for (int i = 0; i < children.Count; i++)
                PrintElementTree(children[i], false, indent, i == children.Count - 1);
        }

        private void PrintTotals(ChainItemsHandler chainItems)
        {
            int itemLength, qtyLength, machineLength, machineNum;
            itemLength = qtyLength = machineLength = machineNum = 0;

            foreach (ChainItem chainItem in chainItems.Items)
            {
                if (chainItem.Item.Name == null || chainItem.Machine == null || chainItem.Machine.Name == null) continue;
                itemLength = Math.Max(itemLength, chainItem.Item.Name.Length);
                machineLength = Math.Max(machineLength, chainItem.Machine.Name.Length);
                qtyLength = Math.Max(qtyLength, chainItem.TotalQuantityPerMinute.ToString().Length);
                machineNum = Math.Max(machineNum, Math.Ceiling(chainItem.TotalMachineCount).ToString().Length);
            }

            foreach (ChainItem chainItem in chainItems.Items)
            {
                float roundedNumOfMachines = (float)Math.Ceiling(chainItem.TotalMachineCount);
                string numOfMachinesString = $"{roundedNumOfMachines}".PadRight(machineNum) + (roundedNumOfMachines == chainItem.TotalMachineCount ? "" : $" ({chainItem.TotalMachineCount:0.##})");
                ConsoleWriter.WriteLine($"{{BC={BC}}}Item: {{FC={IC}}}{chainItem.Item.Name?.PadRight(itemLength)}{{/FC}} " +
                    $"Total Qty: {{FC={IC}}}{chainItem.TotalQuantityPerMinute.ToString().PadRight(qtyLength)}{{/FC}} " +
                    $"Machine Used: {{FC={MC}}}{chainItem.Machine?.Name?.ToString().PadRight(machineLength) ?? "UNKNOWN".PadRight(machineLength)}{{/FC}} " +
                    $"Number of Machines: {{FC={MC}}}{numOfMachinesString}{{/FC}}{{/BC}}");
            }
        }
    }
}
