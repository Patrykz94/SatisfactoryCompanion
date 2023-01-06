using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryCompanion.Core
{
    public static class MachineManager
    {
        public static List<Machine> Machines { get; private set; } = new List<Machine>();

        public static Machine? GetMachine(string? name)
        {
            foreach (Machine machine in Machines)
            {
                if (machine.Name == name) return machine;
            }

            return null;
        }

        public static void Initialize(List<Machine> machines)
        {
            Machines = machines;
        }

        public static void Initialize(List<Machine> machines1, List<Machine> machines2)
        {
            Machines = machines1;
            Machines.AddRange(machines2);
        }
    }
}
