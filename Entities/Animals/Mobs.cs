using System;
using System.Collections.Generic;
using System.Text;
using Wolf_Island.Entities.Animals.Service;

namespace Wolf_Island.Entities.Animals
{
    internal class Rabbit : Herbivore
    {
        /// <summary>
        /// Creats a rabbit.
        /// </summary>
        /// <param name="coordinates">Spawn location.</param>
        public Rabbit(AnimalData data, (uint x, uint y) coordinates = default) : base(data,  coordinates)
        { }

    }
    internal class Wolf : Carnivore
    {
        /// <summary>
        /// Creats a cool wolfs.
        /// </summary>
        /// <param name="coordinates">Spawn location.</param>
        public Wolf(AnimalData data, (uint x, uint y) coordinates = default) : base(data, coordinates)
        { }
    }
}
