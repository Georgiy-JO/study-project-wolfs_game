using System;
using System.Collections.Generic;
using System.Text;
using Wolf_Island.Entities;

namespace Wolf_Island.Entities.Animals
{
    internal abstract class Herbivore : Animal
    {
        protected Herbivore(uint maxHealth, uint maxFullness,
                            uint meatValue, uint FieldOfView, Sizes size,
                            uint maxHungerTimer, uint maxLifelossTimer,
                            (uint x, uint y) coordinates) :
                                    base(maxHealth, maxFullness, meatValue, FieldOfView, size, maxHungerTimer, maxLifelossTimer, coordinates)
        { }
        public override bool Feed(Food food)
        {
            ArgumentNullException.ThrowIfNull(food);
            if (IsAlive && food.Kind == Food.FoodKind.fkGrassy)
            {
                _fullness = Math.Min(_fullness + food.Value, MaxFullness);
                return true;
            }
            return false;
        }
    }
}