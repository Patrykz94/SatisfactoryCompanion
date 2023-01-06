using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryCompanion.Core
{
    public class Recipe
    {
        public List<RecipeInput>? Inputs { get; set; }
        public float Output { get; set; }
        public float Time { get; set; }
        public string? MachineName { get; set; }
        public Machine? Machine
        {
            get { return MachineManager.GetMachine(MachineName); }
        }
        public float GetItemsPerMinute(float clockSpeed = 100) => 60 * Output / Time * (clockSpeed/100);
    }
}
