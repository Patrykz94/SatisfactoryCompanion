using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryCompanion.Core
{
    public static class JsonLoader
    {
        static string filePath = @"data.json";

        public static void LoadJsonFile()
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException("ERROR: data.json file not found!\n\nThis file is required as it contains all required data.");

            JObject jsonObject = JObject.Parse(File.ReadAllText(filePath));

            var machines = LoadObject<Machine?>(jsonObject["Machines"]);
            var extractors = LoadObject<Extractor?>(jsonObject["Extractors"]);
            var items = LoadObject<Item?>(jsonObject["Items"]);

            MachineManager.Initialize(machines, extractors.Cast<Machine>().ToList());

            ItemManager.Initialize(items);
        }

        private static List<T?> LoadObject<T>(JToken? jsonSection)
        {
            List<T?> list = new();

            List<JToken>? objectsArray = jsonSection?.Children().ToList();

            if (objectsArray == null) return list;

            foreach (JToken o in objectsArray)
            {
                T? obj = o.ToObject<T>();
                list.Add(obj);
            }

            return list;
        }
    }
}
