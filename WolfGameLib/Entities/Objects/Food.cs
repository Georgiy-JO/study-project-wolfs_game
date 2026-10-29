using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Wolf_Island.Entities.Objects
{
    public abstract class Food
    {
        public uint Value { get; }          // Can be made changeable - that would allow to make animal eat part of food and allow it to restore
        public (uint x, uint y) Coord { get; }

        protected readonly uint MaxChangeTimer;
        protected uint _changeTimer;


        public Food(uint fValue = 0, uint changeTimer=4, uint curChangeTimer = 4, (uint x, uint y) coord = default)
        {
            Value = fValue;
            Coord = coord;
            MaxChangeTimer = changeTimer;
            _changeTimer = curChangeTimer;
        }
        public Food(uint fValue = 0, uint changeTimer = 4, (uint x, uint y) coord = default)
        {
            Value = fValue;
            Coord = coord;
            MaxChangeTimer = changeTimer;
            _changeTimer = MaxChangeTimer;
        }
        public virtual void Update(Game.Data data, List<(uint x, uint y)>? newData) { }
    }

    public abstract class Grassy : Food
    {
        public Grassy(uint fValue, uint changeTimer, uint curChangeTimer, (uint x, uint y) coord) : base(fValue, changeTimer, curChangeTimer, coord) { }
        public Grassy(uint fValue, uint changeTimer, (uint x, uint y) coord) : base(fValue, changeTimer, coord) { }
    }
    public abstract class Meaty : Food
    {
        public Meaty(uint fValue, uint changeTimer, uint curChangeTimer, (uint x, uint y) coord) : base(fValue, changeTimer, curChangeTimer, coord) { }
        public Meaty(uint fValue, uint changeTimer, (uint x, uint y) coord) : base(fValue, changeTimer, coord) { }
    }
}