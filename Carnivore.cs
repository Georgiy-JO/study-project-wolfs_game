using System;
using System.Collections.Generic;
using System.Text;

namespace WolfGame
{
    internal class Carnivore : Animal
    {
        protected Carnivore(uint health, uint fullness, Animal.Size size, (uint x, uint y) coordinates) : base(health, fullness, size, coordinates) { }

        public override void Feed(Food food)
        {
            if (IsAlive && food.Kind == Food.FoodKind.fkMeaty)
            {
                _fullness = Math.Min(_fullness + food.FullnessValue, MaxFullness);
                // Console.WriteLine($"I was fet ({food.Name}), I am:{Fullness}/{MaxFullness}");
            }
        }

        public override void Update()
        {

        }
    }
}