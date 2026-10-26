using System;
using System.Collections.Generic;
using System.Text;

namespace Wolf_Island.Entities.Objects.FoodKinds
{
    internal class Grass : Grassy
    {
        public Grass() : base(2, (0, 0)) { }
        public Grass((uint x, uint y) coords) : base(2, (coords.x, coords.y)) { }
        public override void Update(Game.Data data, List<(uint x, uint y)>? newData)
        {
            if (newData == null) return;

            Game.Direction[] possiblePlacing = { Game.Direction.dNorth, Game.Direction.dSouth, Game.Direction.dEast, Game.Direction.dWest };
            possiblePlacing = possiblePlacing.GetShuffledCollections();

            ushort placed = 2;
            foreach (var pp in possiblePlacing)
            {
                (long x, long y) coords = Game.GetCoords(pp, this.Coord);
                bool flag = true;

                if (coords.y < 0 || coords.x < 0 || coords.y >= data.Height || coords.x >= data.Width) flag = false;
                for (int i = 0; flag && i < data.Herbivores.Count; i++)
                    if (coords == data.Herbivores[i].Coord) flag = false;
                for (int i = 0; flag && i < data.Carnivores.Count; i++)
                    if (coords == data.Carnivores[i].Coord) flag = false;
                for (int i = 0; flag && i < data.Food.Count; i++)
                    if (coords == data.Food[i].Coord) flag = false;

                if (flag)
                {
                    newData.Add(((uint)coords.x, (uint)coords.y));
                    placed--;
                }
                if (placed == 0) break;
            }
        }
    }

    internal class Remains : Meaty
    {
        public Remains(uint fValue = 0) : base(fValue, (0, 0)) { }
        public Remains((uint x, uint y) coords, uint fValue = 0) : base(fValue, (coords.x, coords.y)) { }
    }
}
