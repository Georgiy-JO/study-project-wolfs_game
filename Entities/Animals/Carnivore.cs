using System;
using System.Collections.Generic;
using System.Text;
using Wolf_Island.Entities;

namespace Wolf_Island.Entities.Animals
{
    internal abstract class Carnivore : Animal
    {
        protected Carnivore(uint maxHealth, uint maxFullness,
                            uint meatValue, uint FieldOfView, Size size,
                            uint maxHungerTimer, uint maxLifelossTimer,
                            (uint x, uint y) coordinates) :
                                base(maxHealth, maxFullness, meatValue, FieldOfView, size, maxHungerTimer, maxLifelossTimer, coordinates)
        { }
        public override bool Feed(Food food)
        {
            ArgumentNullException.ThrowIfNull(food);
            if (IsAlive && food.Kind == Food.FoodKind.fkMeaty)
            {
                _fullness = Math.Min(_fullness + food.Value, MaxFullness);
                return true;
            }
            return false;
        }
    }
}
