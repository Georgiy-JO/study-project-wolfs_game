using System;
using System.Collections.Generic;
using System.Text;

namespace WolfGame
{
    internal class Carnivore:Animal
    {
        public override void Feed(Food food)
        {
            if (IsAlive && food.Kind == Food.FoodKind.fkMeaty)
            {
                _fullness = Math.Min(_fullness + food.FullnessValue, MaxFullness);
                // Console.WriteLine($"I was fet ({food.Name}), I am:{Fullness}/{MaxFullness}");
            }

        }
    }
}
