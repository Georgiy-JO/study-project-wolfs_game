using System;
using System.Collections.Generic;
using System.Text;

namespace Tamagochi
{
    internal class Carnivore:Animal
    {
        public override void Feed(Food food)
        {
            if (IsAlive && food.Kind == Food.FoodKind.fkMeaty)
            {
                _fullness = Math.Min(_fullness + food.Value, MaxFullness);
                Console.WriteLine($"I was fet ({food.Name}), I am:{Fullness}/{MaxFullness}");
            }

        }
    }
}
