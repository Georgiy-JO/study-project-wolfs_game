using Wolf_Island.Core.Entities.Service;

namespace Wolf_Island.Core.Entities.Animals
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
