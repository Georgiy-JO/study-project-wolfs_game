using System;
using System.Collections.Generic;
using System.Text;

namespace Tamagochi
{
    internal class Herbivore:Animal
    {
        public override void Feed(Food food)
        {
            if (IsAlive && food.Kind == Food.FoodKind.fkGrassy) { 
                _fullness = Math.Min(_fullness + food.Value, MaxFullness);
            Console.WriteLine($"I was fet ({food.Name}), I am:{_fullness}/{MaxFullness}");
            }
        }
    }
}
