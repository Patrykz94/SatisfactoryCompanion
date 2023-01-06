using SatisfactoryCompanion;
using SatisfactoryCompanion.Core;

JsonLoader.LoadJsonFile();

Item? item = ItemManager.GetItem("Smart Plating");

float multiplier = 1;
float requirement = 0;

// Positive colour (gaining resources)
ConsoleColor p = ConsoleColor.Green;
// Negative colour (loosing resources)
ConsoleColor n = ConsoleColor.Red;
// Highlight colour (text to stand out)
ConsoleColor h = ConsoleColor.Yellow;

while (item?.IsOre == false)
{
    Recipe recipe = item.Recipes[0];
    ConsoleWriter.WriteLine($"Item Produced: {{FC={p}}}{item.Name, -15}{{/FC}} Qty: {{FC={p}}}{recipe.GetItemsPerMinute() * multiplier} items/m{{/FC}}");
    foreach (var input in recipe.Inputs)
    {
        requirement = input.Quantity / recipe.Output * recipe.GetItemsPerMinute() * multiplier;
        ConsoleWriter.WriteLine($"Item Used:     {{FC={n}}}{input.ItemName, -15}{{/FC}} Qty: {{FC={n}}}{requirement} items/m{{/FC}}");
    }
    float machinesRequired = requirement / recipe.GetItemsPerMinute() * recipe.Output;
    ConsoleWriter.WriteLine($"Machine Used:  {{FC={h}}}{Math.Ceiling(machinesRequired)}x {recipe.Machine?.Name} {(Math.Ceiling(machinesRequired) != machinesRequired ? $"({machinesRequired:0.##}x)" : "" )}{{/FC}}");
    ConsoleWriter.WriteLine();
    item = recipe.Inputs[0].GetItem();
    if (item != null && !item.IsOre) multiplier = 1 / (item.Recipes[0].GetItemsPerMinute() / requirement);
}

ConsoleWriter.WriteLine();