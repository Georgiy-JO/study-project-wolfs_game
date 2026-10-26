using System;
using System.Collections.Generic;
using System.Text;
using Wolf_Island.Entities.Objects;

namespace Wolf_Island.Entities.Animals
{
    internal static class Service
    {
        public static Dictionary<(long x, long y), int> WeightField(Game.Data data, Animal animal)
        {
            ArgumentNullException.ThrowIfNull(animal);

            Dictionary<(long x, long y), int> weightField = new();
            (long x, long y) beginCoord = ((long)animal.Coord.x - animal.FieldOfView, (long)animal.Coord.y - animal.FieldOfView);
            (long x, long y) endCoord = ((long)animal.Coord.x + animal.FieldOfView, (long)animal.Coord.y + animal.FieldOfView);


            int edgeWeight = -50;           // weight of out of field coordinates
            int pointPositiveWeight = 15;   // weight of food coordinates
            int pointNegativeWeight = -30;  // weight enemies coordinates
            int distantWeight = 1;          // weight of coordinates close to food or an enemy

            List<(long x, long y)> enemies = new();
            List<(long x, long y)> food = new();
            for (long j = beginCoord.y; j <= endCoord.y; j++)
            {
                for (long k = beginCoord.x; k <= endCoord.x; k++)
                {
                    if (j < 0 || k < 0 || j > data.Height || k > data.Width) weightField[(k, j)] = edgeWeight;
                    else
                    {
                        weightField[(k, j)] = 0;
                        if (data.Wolfs.Exists(x => x?.Coord == (k, j) && x is Carnivore && x.Size > animal.Size))          // it shouldn't be called Wolfs though
                            enemies.Add((k, j));
                        if (data.Food.Exists(x => x?.Coord == (k, j) &&
                                    ((animal is Herbivore && x is Grassy) ||
                                     (animal is Carnivore && x is Meaty))))
                            food.Add((k, j));
                    }
                }
            }
            foreach (var en in enemies)
            {
                weightField[(en.x, en.y)] += pointNegativeWeight;
                for (long i = en.y - 1; i <= en.y + 1; i++)
                {
                    for (long j = en.x - 1; j <= en.x + 1; j++)
                    {
                        if (weightField.ContainsKey((j, i))) weightField[(j, i)] -= distantWeight;
                    }
                }
            }
            foreach (var fd in food)
            {
                weightField[(fd.x, fd.y)] += pointPositiveWeight;
                for (long i = fd.y - 1; i <= fd.y + 1; i++)
                {
                    for (long j = fd.x - 1; j <= fd.x + 1; j++)
                    {
                        if (weightField.ContainsKey((j, i))) weightField[(j, i)] += distantWeight;
                    }
                }
            }

            return weightField;

        }
    }
}
