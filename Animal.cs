using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Tamagochi
{
    internal abstract class Animal
    {
        public enum Size
        {
            szSmall, szMedium, szLarge
        }
        const int HealthAdd = 1;
        protected uint _health;
        protected uint _fullness;
        protected Size _size;

        public Animal(uint health = 5, uint fullness = 5, Size size=Size.szMedium) { _health = health; _fullness = fullness; _size = size; }

        public bool IsAlive  => _health > 0;
        public void Kill() => _health = 0;
        public uint Fullness => _fullness;
        public bool IsFull => _fullness == MaxFullness;
        public bool IsHealthy => _health == MaxHealth;

        protected virtual uint MaxHealth { get => 5; }
        protected virtual uint MaxFullness { get => 5; }

        public abstract void Feed(Food food);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimePass"></param>
        /// <returns></returns>
        public bool Status(uint TimePass)                       // adjust constants of time spents!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
        {
            for (uint i= TimePass;  IsAlive && i>0; i-- )
            {
                if (_fullness == MaxFullness && _health < MaxHealth && i%2==0)
                        _health= Math.Min(MaxHealth,_health+ HealthAdd);
                if (_fullness == 0 && i%6==0 )
                        _health--;
                if(i%7==0 && _fullness>0)
                        _fullness--;
                Console.Write( $"Health: {_health}/{MaxHealth}; Fullness: {_fullness}/{MaxFullness}\t");
                if(i == 1) Console.WriteLine();
            }

            return IsAlive;
        }
    }
}
