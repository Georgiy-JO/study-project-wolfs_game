using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Text;

namespace WolfGame
{
    internal abstract class Animal
    {
        public enum Size
        {
            szSmall=1, szMedium=2, szLarge=3
        }
        

        const int HealthAdd = 1;
        protected uint _health;
        protected uint _fullness;
        protected Size _size;
        private (double x, double y) _coordinates;


        public bool IsAlive  => _health > 0;
        public void Kill() => _health = 0;
        public uint Fullness => _fullness;
        public bool IsFull => _fullness == MaxFullness;
        public bool IsHealthy => _health == MaxHealth;
        public (double x, double y) Coord => _coordinates;
        

        protected virtual uint MaxHealth { get => 5; }
        protected virtual uint MaxFullness { get => 5; }
        protected virtual uint MaxMoveDistance { get => 1;}
        public virtual uint FullnessValue { get => 2;} 


        public Animal(uint health = 5, uint fullness = 5, Size size=Size.szMedium, (double x, double y) coordinates=default) 
        { _health = health; _fullness = fullness; _size = size; _coordinates=coordinates; }


        public abstract void Feed(Food food);
        protected void SetCoord(double x, double y) => _coordinates=(x,y);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimePass"></param>
        /// <returns></returns>
        public bool Status(uint TimePass)                       // adjust constants of time spent!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
        {
            for (uint i= TimePass;  IsAlive && i>0; i-- )
            {
                if (_fullness == MaxFullness && _health < MaxHealth && i%2==0)
                        _health= Math.Min(MaxHealth,_health+ HealthAdd);
                if (_fullness == 0 && i%6==0 )
                        _health--;
                if(i%7==0 && _fullness>0)
                        _fullness--;
                // Console.Write( $"Health: {_health}/{MaxHealth}; Fullness: {_fullness}/{MaxFullness}\t");
                if(i == 1) Console.WriteLine();
            }

            return IsAlive;
        }

        public abstract void Update();
    }
}
