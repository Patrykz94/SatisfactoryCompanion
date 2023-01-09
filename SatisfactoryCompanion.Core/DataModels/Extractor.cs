using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryCompanion.Core
{
    public class Extractor : Machine
    {
        public float ExtractionTime { get; set; }

        public float GetItemsPerMinute()
        {
            return 60 / ExtractionTime;
        }
    }
}
