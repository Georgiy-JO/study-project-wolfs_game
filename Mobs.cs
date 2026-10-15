using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;

namespace WolfGame
{
    internal  class Wolf: Carnivore
    {
        protected override uint MaxHealth { get => 8; }
        protected override uint MaxFullness { get => 6; }
        protected override uint MaxMoveDistance { get => 2; }
        public override uint MeatValue { get => 5; }

        public Wolf((uint x, uint y) coordinates = default) : base(5,6,Animal.Size.szMedium, coordinates) { }
    }
    internal class Rabbit: Herbivore
    {
        protected override uint MaxHealth { get => 4; }
        protected override uint MaxFullness { get => 3; }
        protected override uint MaxMoveDistance { get => 1; }
        public override uint MeatValue { get => 2; }

        public Rabbit((uint x, uint y) coordinates = default) : base(5, 6, Animal.Size.szMedium, coordinates) { }

    }
}

