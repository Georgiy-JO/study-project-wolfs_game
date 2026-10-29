using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Text;
using Wolf_Island.Entities.Objects;
using Wolf_Island.Entities.Animals.Service;

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
        public uint FieldOfView { get; }
        public uint MeatValue { private set; get; }
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
        /// <param name="dat">Set constants and other animal data.</param>
        /// <param name="coordinates">Spawn location.</param>
        public Animal(AnimalData dat, (uint x, uint y) coordinates = default)
        {
            MaxHealth = dat.MaxHealth;
            MaxFullness = dat.MaxFullness;
            MeatValue = dat.MeatValue;
            FieldOfView = dat.FieldOfView;
            MaxHungerTimer = dat.HungerTimer;
            MaxLifelossTimer = dat.LifelossTimer;
            _hungerTimer = MaxHungerTimer;
            _lifelossTimer = MaxLifelossTimer;
            _health = MaxHealth;
            _fullness = MaxFullness;
            Size = dat.Size;
            _coordinates = coordinates;
        }
        /// <summary>
        /// An Animal constructor.
        /// </summary>
        /// <param name="dat">Set constants and other animal data.</param>
        /// <param name="health">How much health it has at the moment of the spawn.</param>
        /// <param name="fullness">How full it is at the moment of the spawn.</param>
        /// <param name="coordinates">Spawn location.</param>
        public Animal(AnimalData dat,
                      uint health = 1, uint fullness = 1,
                       (uint x, uint y) coordinates = default)
        {
            MaxHealth = dat.MaxHealth;
            MaxFullness = dat.MaxFullness;
            MeatValue = dat.MeatValue;
            FieldOfView = dat.FieldOfView;
            MaxHungerTimer = dat.HungerTimer;
            MaxLifelossTimer = dat.LifelossTimer;
            _hungerTimer = MaxHungerTimer;
            _lifelossTimer = MaxLifelossTimer;
            _health = health;
            _fullness = fullness;
            Size = dat.Size;
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
            else
                _hungerTimer--;
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

            return IsAlive;
        }


        public abstract bool Feed(Food food);
        public abstract bool Feed(Animal food);
        public abstract bool CanEat(Food food);
        public abstract bool CanEat(Animal food);
        public virtual void Update(Game.Data data)
        {
            Movehandler(data);              // handle moving
            Feedhandler(data);              // handle feeding
            Status();                       // hanger and life handle
        }
        protected virtual void Movehandler(Game.Data data)
        {
            var weightField = WeightField.CreateWeightField(data, this);
            var possibleMoves = Enum.GetValues<Game.Direction>().Select(dir => Game.GetCoords(dir, Coord)).ToArray().GetShuffledCollections();
            ((long x, long y), int weight) theMove = (Coord, -(int)WeightField.weightDelta);       // issue#Movehandler_0: if the field size comes close to max int and FieldOfView comes close to global we are fucked

            foreach (var possibleMove in possibleMoves)
            {
                if (theMove.weight < weightField[possibleMove])       // (possibleMove.x >= 0 && possibleMove.y >= 0 && possibleMove.x < data.Width && possibleMove.y < data.Height)
                    theMove = (possibleMove, (int)weightField[possibleMove]);                   // issue#Movehandler_0
            }
            SetCoord((uint)theMove.Item1.x, (uint)theMove.Item1.y);
        }
        protected virtual void Feedhandler(Game.Data data)
        {
            var FoodThere = data.Food.Find(x => x.Coord == Coord);
            if (this.Feed(FoodThere))
            {
                data.Food.Remove(FoodThere);                    // Can avoid removing grass here if we add an ability to change grass's value 
                return;
            }

            var AnimalFoodThere = data.Animals.Find(x => x.Coord == Coord);
            if (this.Feed(AnimalFoodThere))
            {
                AnimalFoodThere.Kill();
                AnimalFoodThere.MeatValue = 0;
                return;
            }

            //var AnimalCarnivoreFoodThere = data.Carnivores.Find(x => x.Coord == Coord);
            //if (this.Feed(AnimalCarnivoreFoodThere))
            //{
            //    AnimalCarnivoreFoodThere.Kill();
            //    return;
            //}
        }
    }
}
