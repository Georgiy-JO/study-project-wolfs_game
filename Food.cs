using System;
using System.Collections.Generic;
using System.Text;

namespace WolfGame
{
    internal class Food
    {
        public enum FoodKind
        {
            fkMeaty,
            fkGrassy,
            fkNone
        }
        private (uint x, uint y) _coordinates;
        //public string Name { get; }
        public uint MeatValue { get; }
        public FoodKind Kind { get; }
        public (uint x, uint y)  Coord => _coordinates;


        public Food(uint fValue = 0, FoodKind kind = FoodKind.fkNone, (uint x, uint y) coord =default)
        {
            //Name= name;
            MeatValue = fValue;
            Kind = kind; 
            _coordinates = coord;
        }



    }

    internal class Grass : Food             // check consts in constructor!!!!!!
    {
        public Grass():base(2, FoodKind.fkGrassy, (0, 0)) { }
    }

    internal class Remains : Food
    {
        public Remains(uint fValue = 0):base(fValue, FoodKind.fkMeaty,(0, 0) ){}
    }
}