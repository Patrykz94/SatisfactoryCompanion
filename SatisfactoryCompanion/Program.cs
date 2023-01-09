using SatisfactoryCompanion;
using SatisfactoryCompanion.Core;

Console.OutputEncoding = System.Text.Encoding.UTF8;

JsonLoader.LoadJsonFile();

ConsoleMenu menu = new ConsoleMenu();
menu.ShowItemSelectionScreen();