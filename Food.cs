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
        private (double x, double y) _coordinates;
        //public string Name { get; }
        public uint FullnessValue { get; }
        public FoodKind Kind { get; }
        public (double x, double y)  Coord => _coordinates;


        public Food(uint fValue = 0, FoodKind kind = FoodKind.fkNone, (double x, double y) coord =default)
        {
            //Name= name;
            FullnessValue = fValue;
            Kind = kind; 
            _coordinates = coord;
        }



    }

    internal class Grass : Food             // check consts in constructor!!!!!!
    {
        Grass():base(2, FoodKind.fkGrassy, (0.0, 0.0)) { }
    }

    internal class Remains : Food
    {
        Remains(uint fValue = 0):base(fValue, FoodKind.fkMeaty,(0.0, 0.0) ){}
    }
}
