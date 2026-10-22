using System;
using System.Collections.Generic;
using System.Text;

namespace WolfGame
{
    internal class Herbivore : Animal
    {
        protected Herbivore (uint health, uint fullness, Animal.Size size, (uint x, uint y) coordinates) : base(health, fullness, size, coordinates) { }
        public override void Feed(Food food)
        {
            if (IsAlive && food.Kind == Food.FoodKind.fkGrassy) { 
                _fullness = Math.Min(_fullness + food.Value, MaxFullness);
            // Console.WriteLine($"I was fet ({food.Name}), I am:{_fullness}/{MaxFullness}");
            }
        }
    }
}