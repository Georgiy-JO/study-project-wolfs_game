using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Text;
using Wolf_Island.Entities.Objects;

namespace Wolf_Island.Entities.Animals
{
    internal abstract class Animal
    {
        public enum Sizes
        {
            szSmall = 1, szMedium = 2, szLarge = 3
        }


        protected uint MaxHealth { get; }
        protected uint MaxFullness { get; }
        //protected virtual uint MaxMoveDistance { get => 1;}       // not used
        public uint MeatValue { get; }
        public uint FieldOfView { get; }
        private readonly uint MaxHungerTimer;
        private readonly uint MaxLifelossTimer;
        public Sizes Size { get; }


        protected uint _health;
        protected uint _fullness;
        private (uint x, uint y) _coordinates;
        private uint _hungerTimer;
        private uint _lifelossTimer;


        public bool IsAlive => _health > 0;
        public void Kill() => _health = 0;
        public uint Fullness => _fullness;
        public bool IsFull => _fullness == MaxFullness;
        public bool IsHealthy => _health == MaxHealth;
        public (uint x, uint y) Coord => _coordinates;

        /// <summary>
        /// An Animal constructor.
        /// </summary>
        /// <param name="maxHealth">how much health it has (health == 0 - death)</param>
        /// <param name="maxFullness">how hungry it can get until it starts loosing health</param>
        /// <param name="meatValue">how much value will its remains Food:Remains has</param>
        /// <param name="FieldOfView">how far on the field can it see</param>
        /// <param name="size">size of the animal (important for the life chain - who eats who)</param>
        /// <param name="maxHungerTimer">how many moves it can make before becoming hungry</param>
        /// <param name="maxLifelossTimer">how fast will it lose health if it's hungry</param>
        /// <param name="coordinates">spawn location</param>
        public Animal(uint maxHealth = 1, uint maxFullness = 1,
                      uint meatValue = 1, uint fieldOfView = 1, Sizes size = Sizes.szMedium,
                      uint maxHungerTimer = 2, uint maxLifelossTimer = 2,
                       (uint x, uint y) coordinates = default)
        {
            MaxHealth = maxHealth;
            MaxFullness = maxFullness;
            MeatValue = meatValue;
            FieldOfView = fieldOfView;
            MaxHungerTimer = maxHungerTimer;
            MaxLifelossTimer = maxLifelossTimer;
            _hungerTimer = MaxHungerTimer;
            _lifelossTimer = MaxLifelossTimer;
            _health = MaxHealth;
            _fullness = MaxFullness;
            Size = size;
            _coordinates = coordinates;
        }
        /// <summary>
        /// An Animal constructor.
        /// </summary>
        /// <param name="maxHealth">how much health it has (health == 0 - death)</param>
        /// <param name="maxFullness">how hungry it can get until it starts loosing health</param>
        /// <param name="meatValue">how much value will its remains Food:Remains has</param>
        /// <param name="FieldOfView">how far on the field can it see</param>
        /// <param name="size">size of the animal (important for the life chain - who eats who)</param>
        /// <param name="maxHungerTimer">how many moves it can make before becoming hungry</param>
        /// <param name="maxLifelossTimer">how fast will it lose health if it's hungry</param>
        /// <param name="health">how much health it has at the moment of the spawn</param>
        /// <param name="fullness">how full it is at the moment of the spawn</param>
        /// <param name="coordinates">spawn location</param>
        public Animal(uint maxHealth = 1, uint maxFullness = 1,
                      uint maxHungerTimer = 2, uint maxLifelossTimer = 2,
                      uint meatValue = 1, uint fieldOfView = 1, Sizes size = Sizes.szMedium,
                      uint health = 1, uint fullness = 1,
                       (uint x, uint y) coordinates = default)
        {
            MaxHealth = maxHealth;
            MaxFullness = maxFullness;
            MeatValue = meatValue;
            FieldOfView = fieldOfView;
            MaxHungerTimer = maxHungerTimer;
            MaxLifelossTimer = maxLifelossTimer;
            _hungerTimer = MaxHungerTimer;
            _lifelossTimer = MaxLifelossTimer;
            _health = health;
            _fullness = fullness;
            Size = size;
            _coordinates = coordinates;
        }

        public void SetCoord(uint x, uint y) => _coordinates = (x, y);
        public void SetCoord((uint x, uint y) newCoords) => _coordinates = newCoords;
        /// <summary>
        /// Handles _fullness and _health.
        /// Uses _lifelossTimer and _hungerTimer for that.
        /// </summary>
        /// <returns>If the animal is alive</returns>
        public bool Status()
        {
            if (_hungerTimer == 0)
                _hungerTimer = MaxHungerTimer;
            if (_fullness == MaxFullness && _health < MaxHealth)
                _health = Math.Min(MaxHealth, _health + 1);
            if (_fullness == 0)
            {
                if (_lifelossTimer == 0)
                {
                    _health--;
                    _lifelossTimer = MaxLifelossTimer;
                }
                else
                    _lifelossTimer--;
            }

            if (_fullness > 0 && _hungerTimer == 0)
                _fullness--;

            _hungerTimer--;

            return IsAlive;
        }


        public abstract bool Feed(Food food);
        public abstract bool Feed(Animal food);
        public abstract bool CanEat(Food food);
        public abstract bool CanEat(Animal food);
        public void Update(Game.Data data)
        {
            Movehandler(data);              // handle moving
            Feedhandler(data);              // handle feeding
            Status();                       // hanger and life handle
        }
        protected void Movehandler(Game.Data data)
        {
            var weightField = Service.WeightField(data, this);
            var possibleMoves = Enum.GetValues<Game.Direction>().Select(dir => Game.GetCoords(dir, Coord)).ToArray().GetShuffledCollections();
            ((long x, long y), int weight) theMove = (Coord, -(int)Service.weightDelta);       // issue#Movehandler_0: if the field size comes close to max int and FieldOfView comes close to global we are fucked

            foreach (var possibleMove in possibleMoves)
            {
                if (theMove.weight < weightField[possibleMove])       // (possibleMove.x >= 0 && possibleMove.y >= 0 && possibleMove.x < data.Width && possibleMove.y < data.Height)
                    theMove = (possibleMove, (int)weightField[possibleMove]);                   // issue#Movehandler_0
            }
            SetCoord((uint)theMove.Item1.x, (uint)theMove.Item1.y);
        }
        protected void Feedhandler(Game.Data data)
        {
            var FoodThere = data.Food.Find(x => x.Coord == Coord);
            if (this.Feed(FoodThere))
            {
                data.Food.Remove(FoodThere);
                return;
            }

            var AnimalHerbivoreFoodThere = data.Herbivores.Find(x => x.Coord == Coord);
            if (this.Feed(AnimalHerbivoreFoodThere))
            {
                AnimalHerbivoreFoodThere.Kill();
                return;
            }

            var AnimalCarnivoreFoodThere = data.Carnivores.Find(x => x.Coord == Coord);
            if (this.Feed(AnimalCarnivoreFoodThere))
            {
                AnimalCarnivoreFoodThere.Kill();
                return;
            }
        }
    }
}
