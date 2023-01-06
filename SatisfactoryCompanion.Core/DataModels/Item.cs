using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryCompanion.Core
{
    public class Item
    {
        public string? Name { get; set; }
        public bool IsOre { get; set; }
        public List<Recipe>? Recipes { get; set; }
        public List<string>? ExtractorNames { get; set; }

        public List<Extractor> GetExtractors()
        {
            var extractors = new List<Extractor>();
            if (ExtractorNames == null) return extractors;

            foreach (string extractorName in ExtractorNames)
            {
                if (MachineManager.GetMachine(extractorName) is Extractor extractor) extractors.Add(extractor);
            }

            return extractors;
        }
    }
}
