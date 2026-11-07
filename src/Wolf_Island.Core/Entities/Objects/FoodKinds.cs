using Wolf_Island.Core.Entities.Service;
using Wolf_Island.Core.Extensions;

namespace Wolf_Island.Core.Entities.Objects.FoodKinds
{
    internal sealed class Grass : Grassy
    {
        public Grass(FoodData data, (uint x, uint y) coord = default) : base(data, coord) { }
        //public Grass(FoodDataShort data, uint curChangeTimer = default, (uint x, uint y) coord = default) : base(data, curChangeTimer,coord) { }
        //public Grass(FoodDataShort data, (uint x, uint y) coord = default) : base(data, coord) { }
        public override void Update(Game.Data data, List<(uint x, uint y)>? newData)
        {
            if (newData == null) return;

            if (_changeTimer != 0)
            {
                _changeTimer--;
                return;
            }
            _changeTimer = MaxChangeTimer;

            Game.Direction[] possiblePlacing = { Game.Direction.dNorth, Game.Direction.dSouth, Game.Direction.dEast, Game.Direction.dWest };
            possiblePlacing = possiblePlacing.GetShuffledCollections();

            ushort placed = 2;
            foreach (var pp in possiblePlacing)
            {
                (long x, long y) coords = Game.GetCoords(pp, this.Coord);
                bool flag = true;

                if (coords.y < 0 || coords.x < 0 || coords.y >= data.Height || coords.x >= data.Width) flag = false;
                for (int i = 0; flag && i < data.Animals.Count; i++)
                    if (coords == data.Animals[i].Coord) flag = false;
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

    internal sealed class Remains : Meaty
    {
        public Remains(FoodData data, (uint x, uint y) coord = default) : base(data, coord) { }
        //public Remains(FoodDataShort data, uint curChangeTimer = default, (uint x, uint y) coord = default) : base(data, curChangeTimer, coord) { }
        //public Remains(FoodDataShort data, (uint x, uint y) coord = default) : base(data, coord) { }
        public override void Update(Game.Data data, List<(uint x, uint y)>? newData)
        {
            if (_changeTimer != 0)
            {
                _changeTimer--;
                return;
            }
            _changeTimer = MaxChangeTimer;

            // Add here the dissapearing logic (Game.cs, Food.cs and Animals.cs should be adjusted as well)
        }
    }
}
