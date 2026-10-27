using System;
using System.Collections.Generic;
using System.Text;
using Wolf_Island.Entities.Animals.Service;

namespace Wolf_Island.Entities.Animals
{
    internal class Rabbit : Herbivore
    {
        /// <summary>
        /// Creating a rabbit.
        /// maxHealth - how much health it has (health == 0 - death), 
        /// maxFullness - how hungry it can get until it starts loosing health,
        /// meatValue - how much value will its remains Food:Remains has, 
        /// fieldOfView - how far on the field can it see, 
        /// size:Size.szSmall - size of the animal (important for the life chain - who eats who),
        /// maxHungerTimer - how many moves it can make before becoming hungry, 
        /// maxLifelossTimer - how fast will it lose health if it's hungry, 
        /// coordinates - spawn location
        /// </summary>
        /// <param name="coordinates">spawn location</param>
        public Rabbit((uint x, uint y) coordinates = default) : base(maxHealth: 4, maxFullness: 3,
                                                                     meatValue: 2, FieldOfView: 2, size: Sizes.szSmall,
                                                                     maxHungerTimer: 1, maxLifelossTimer: 1,
                                                                     coordinates)
        { }

    }
    internal class Wolf : Carnivore
    {
        public Wolf((uint x, uint y) coordinates = default) : base(maxHealth: 5, maxFullness: 6,
                                                                   meatValue: 5, FieldOfView: 2, size: Sizes.szMedium,
                                                                   maxHungerTimer: 2, maxLifelossTimer: 1,
                                                                   coordinates)
        { }
    }
}
