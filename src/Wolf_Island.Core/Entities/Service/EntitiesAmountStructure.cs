using System;
using System.Collections.Generic;
using System.Text;
using Wolf_Island.Core.Entities.Animals;

namespace Wolf_Island.Core.Entities.Service
{
    internal class EntitiesAmountStructure
    {
        public Dictionary<string, uint> Amount { get; set; }

        public EntitiesAmountStructure(Dictionary<string, IConfig> registry)
        {
            Amount = new Dictionary<string, uint>();

            foreach (var elem in registry)
                Amount[elem.Key] = 0;
        }

    }
}
