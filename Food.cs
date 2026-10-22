using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Wolf_Island;

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
        public uint Value { get; }
        public FoodKind Kind { get; }
        public (uint x, uint y)  Coord => _coordinates;


        public Food(uint fValue = 0, FoodKind kind = FoodKind.fkNone, (uint x, uint y) coord =default)
        {
            //Name= name;
            Value = fValue;
            Kind = kind; 
            _coordinates = coord;
        }
        public virtual bool Update(Game.Data data, List<(uint x, uint y)>? newData) { return true; }
    }

    internal class Grass : Food             // check consts in constructor!!!!!!
    {
        public Grass():base(2, FoodKind.fkGrassy, (0, 0)) { }
        public Grass((uint x, uint y) coords): base(2, FoodKind.fkGrassy, (coords.x, coords.y)) { }
        public override bool Update(Game.Data data, List<(uint x, uint y)>? newData) {
            IEnumerable<Game.Direction> possiblePlacing = new Game.Direction[] { Game.Direction.dNorth, Game.Direction.dSouth, Game.Direction.dEast, Game.Direction.dWest };
            possiblePlacing.GetShuffledCollections(out possiblePlacing).ToArray();

            ushort placed = 2;
            foreach (var pp in possiblePlacing)
            {
                (long x, long y) coords = Game.GetCoords(pp, this.Coord);
                bool flag = true;
                
                if (coords.y<0 || coords.x<0 || coords.y>=data.Height || coords.x>=data.Width) flag=false;
                for (int i= 0;flag && i<data.Rabbits.Count;i++)
                    if (coords == data.Rabbits[i].Coord) flag = false;
                for (int i = 0; flag && i < data.Wolfs.Count; i++)
                    if (coords == data.Wolfs[i].Coord) flag = false;
                for (int i = 0; flag && i < data.Grass.Count; i++)
                    if (coords == data.Grass[i].Coord) flag = false;

                if (flag)
                {
                    newData?.Add(((uint)coords.x, (uint)coords.y));
                    placed--;
                }
                if(placed==0) break;
            }
            return placed == 0;
        }
    }

    internal class Remains : Food
    {
        public Remains(uint fValue = 0):base(fValue, FoodKind.fkMeaty,(0, 0) ){}
        public Remains((uint x, uint y) coords, uint fValue = 0) : base(fValue, FoodKind.fkMeaty, (coords.x, coords.y)) { }
    }
}