using System;
using System.Collections.Generic;
using System.Text;
using Wolf_Island.Entities.Objects;

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
            if (IsAlive && CanEat(food) && !IsFull)
            {
                _fullness = Math.Min(_fullness + food.Value, MaxFullness);
                return true;
            }
            return false;
        }
        public override bool Feed(Animal food) => false;
        public override bool CanEat(Food food)
        {
            ArgumentNullException.ThrowIfNull(food);
            return food is Grassy;
        }
        public override bool CanEat(Animal food) => false;
    }
}