using System;
using System.Collections.Generic;
using System.Text;

namespace Tamagochi
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
        public uint Value { get; }
        public FoodKind Kind { get; }
        public (double x, double y)  Coord => _coordinates;


        public Food(uint value = 0, FoodKind kind = FoodKind.fkNone, (double x, double y) coord =default)
        {
            //Name= name;
            Value = value;
            Kind = kind; 
            _coordinates = coord;
        }



    }

    internal class Grass : Food             // check consts in constructor!!!!!!
    {
        //public Grass(uint value = 2, FoodKind kind = FoodKind.fkGrassy, (double x, double y) coord = default) : base(value, kind, coord) { }
        Grass():base(2, FoodKind.fkGrassy, (0.0, 0.0)) { }
    }
}
