using System;
using System.Collections.Generic;
using System.Text;
using Wolf_Island.Entities.Animals.Service;
using Wolf_Island.Entities.Objects;

namespace Wolf_Island.Entities.Animals
{
    internal abstract class Carnivore : Animal
    {
        protected Carnivore(AnimalData dat, (uint x, uint y) coordinates) : base(dat, coordinates)
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
        public override bool Feed(Animal food)
        {
            if (IsAlive && CanEat(food) && !IsFull)
            {
                _fullness = Math.Min(_fullness + food.MeatValue, MaxFullness);
                return true;
            }
            return false;
        }
        public override bool CanEat(Food food)
        {
            ArgumentNullException.ThrowIfNull(food);
            return food is Meaty;
        }
        public override bool CanEat(Animal food)
        {
            ArgumentNullException.ThrowIfNull(food);
            return food.Size < this.Size;
        }
    }
}
