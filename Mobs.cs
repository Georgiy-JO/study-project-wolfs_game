using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Text;

namespace WolfGame
{
    internal  class Wolf: Carnivore
    {
        public Wolf((uint x, uint y) coordinates = default) : base(5, 6, Animal.Size.szMedium, coordinates) { }

    }
    internal class Rabbit: Herbivore
    {
        public Rabbit((uint x, uint y) coordinates = default) : base(5, 6, Animal.Size.szMedium, coordinates) { }

    }
}