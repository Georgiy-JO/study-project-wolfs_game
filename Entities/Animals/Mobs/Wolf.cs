using System;
using System.Collections.Generic;
using System.Text;

namespace Wolf_Island.Entities.Animals.Mobs
{
    internal class Wolf : Carnivore
    {
        public Wolf((uint x, uint y) coordinates = default) : base(maxHealth: 5, maxFullness: 6, 
                                                                   meatValue: 5, FieldOfView: 2, size: Sizes.szMedium,
                                                                   maxHungerTimer: 2, maxLifelossTimer: 1,
                                                                   coordinates)
        { }

        public override bool Update(Data data)
        {

        }

        protected override Dictionary<(long x, long y), int> WeightField(Data data)
        {
            Dictionary<(long x, long y), int> resWeights = new();

            (long x, long y) center = (Coord.x, Coord.y);
            var positions = new HashSet<(long x, long y)>();

            long minX = (long)MathF.Max(0, center.x - FirldOfView);
            long maxX = (long)MathF.Max(data.Width - 1, center.x + FirldOfView);

            long minY = (long)MathF.Max(0, center.y - FirldOfView);
            long maxY = (long)MathF.Max(data.Height - 1, center.y + FirldOfView);

            for (long x = minX; x <= maxX; x++)
            {
                for (long y = minY; y <= maxY; y++)
                {
                    positions.Add((x, y));
                }
            }

            var viewableRabbits = data.Rabbits.Where((r) => positions.Contains(r.Coord));


        }
    }
}
