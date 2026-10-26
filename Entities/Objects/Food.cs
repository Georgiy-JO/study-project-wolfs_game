using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Wolf_Island.Entities.Objects
{
    internal abstract class Food
    {
        public uint Value { get; }
        public (uint x, uint y) Coord { get; }


        public Food(uint fValue = 0, (uint x, uint y) coord = default)
        {
            Value = fValue;
            Coord = coord;
        }
        public virtual void Update(Game.Data data, List<(uint x, uint y)>? newData) { }
    }

    internal abstract class Grassy : Food  
    {
        public Grassy(uint fValue, (uint x, uint y) coord) : base(fValue, coord) { }
    }
    internal abstract class Meaty : Food
    {
        public Meaty(uint fValue, (uint x, uint y) coord) : base(fValue, coord) { }
    }
}