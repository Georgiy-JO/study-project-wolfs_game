using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Text;
using Wolf_Island.Entities;
using WolfGame;

namespace Wolf_Island.Entities.Animals
{
    internal abstract class Animal
    {
        public enum Size
        {
            szSmall = 1, szMedium = 2, szLarge = 3
        }


        protected readonly uint MaxHealth;
        protected readonly uint MaxFullness;
        //protected virtual uint MaxMoveDistance { get => 1;}       // not used
        public readonly uint MeatValue;
        public readonly uint FeildOfView;
        private readonly uint MaxHungerTimer;
        private readonly uint MaxLifelossTimer;
        protected readonly Size _size;                          // not really used in this game instance (can be fixed)


        //const int HealthAdd = 1;
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
        /// <param name="feildOfView">how far on the field can it see</param>
        /// <param name="size">size of the animal (important for the life chain - who eats who)</param>
        /// <param name="maxHungerTimer">how many moves it can make before becoming hungry</param>
        /// <param name="maxLifelossTimer">how fast will it lose health if it's hungry</param>
        /// <param name="coordinates">spawn location</param>
        public Animal(uint maxHealth = 1, uint maxFullness = 1,
                      uint meatValue = 1, uint feildOfView = 1, Size size = Size.szMedium,
                      uint maxHungerTimer = 2, uint maxLifelossTimer = 2,
                       (uint x, uint y) coordinates = default)
        {
            MaxHealth = maxHealth;
            MaxFullness = maxFullness;
            MeatValue = meatValue;
            FeildOfView = feildOfView;
            MaxHungerTimer = maxHungerTimer;
            MaxLifelossTimer = maxLifelossTimer;
            _hungerTimer = MaxHungerTimer;
            _lifelossTimer = MaxLifelossTimer;
            _health = MaxHealth;
            _fullness = MaxFullness;
            _size = size;
            _coordinates = coordinates;
        }
        /// <summary>
        /// An Animal constructor.
        /// </summary>
        /// <param name="maxHealth">how much health it has (health == 0 - death)</param>
        /// <param name="maxFullness">how hungry it can get until it starts loosing health</param>
        /// <param name="meatValue">how much value will its remains Food:Remains has</param>
        /// <param name="feildOfView">how far on the field can it see</param>
        /// <param name="size">size of the animal (important for the life chain - who eats who)</param>
        /// <param name="maxHungerTimer">how many moves it can make before becoming hungry</param>
        /// <param name="maxLifelossTimer">how fast will it lose health if it's hungry</param>
        /// <param name="health">how much health it has at the moment of the spawn</param>
        /// <param name="fullness">how full it is at the moment of the spawn</param>
        /// <param name="coordinates">spawn location</param>
        public Animal(uint maxHealth = 1, uint maxFullness = 1,
                      uint maxHungerTimer = 2, uint maxLifelossTimer = 2,
                      uint meatValue = 1, uint feildOfView = 1, Size size = Size.szMedium,
                      uint health = 1, uint fullness = 1,
                       (uint x, uint y) coordinates = default)
        {
            MaxHealth = maxHealth;
            MaxFullness = maxFullness;
            MeatValue = meatValue;
            FeildOfView = feildOfView;
            MaxHungerTimer = maxHungerTimer;
            MaxLifelossTimer = maxLifelossTimer;
            _hungerTimer = MaxHungerTimer;
            _lifelossTimer = MaxLifelossTimer;
            _health = health;
            _fullness = fullness;
            _size = size;
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
        public abstract void Update(Game.Data data);
        protected abstract void Movehandler(Game.Data data);
        protected abstract void Feedhandler(Game.Data data);




    }
}
