using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Wolf_Island.Entities.Objects;
using static Wolf_Island.Entities.Animals.Animal;

namespace Wolf_Island.Entities.Animals
{

    namespace Service
    {
        internal struct AnimalData      //notdone
        {
            public uint maxHealth;
            public uint maxFullness;
            public uint meatValue;
            public uint fieldOfView;
            public Sizes size;
            public uint maxHungerTimer;
            public uint maxLifelossTimer;

            AnimalData(uint mHealth = 1, uint mFullness = 1,
                      uint meatVal = 1, uint foV = 1, Sizes sz = Sizes.szMedium,
                      uint hungerTimer = 2, uint lifelossTimer = 2)
            {
                maxHealth = mHealth;
                maxFullness = mFullness;
                meatValue = meatVal;
                fieldOfView = foV;
                size = sz;
                maxHungerTimer = hungerTimer;
            }

        }

        internal static class WeightField
        {
            public static long weightDelta = 5;

            public static Dictionary<(long x, long y), long> CreateWeightField(Game.Data data, Animal animal)
            {
                ArgumentNullException.ThrowIfNull(animal);

                Dictionary<(long x, long y), long> weightField = new();
                (long x, long y) beginCoord = ((long)animal.Coord.x - animal.FieldOfView, (long)animal.Coord.y - animal.FieldOfView);
                (long x, long y) endCoord = ((long)animal.Coord.x + animal.FieldOfView, (long)animal.Coord.y + animal.FieldOfView);


                uint weightRadius = (animal.FieldOfView == 0) ? 0 : animal.FieldOfView - 1;
                long[] distantWeight = new long[weightRadius];
                long pointPositiveWeight = 0;
                for (int i = 0; i < weightRadius; i++)
                {
                    distantWeight[i] = weightRadius - i;
                    pointPositiveWeight += distantWeight[i] * 8 * (i + 1);
                }
                pointPositiveWeight += weightDelta;
                long pointNegativeWeight = pointPositiveWeight * 2 * (-1);
                long edgeWeight = -(pointPositiveWeight * 4);



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
                            if (data.Carnivores.Exists(x => x?.Coord == (k, j) && x.CanEat(animal)))
                                enemies.Add((k, j));
                            if (data.Food.Exists(x => x?.Coord == (k, j) && animal.CanEat(x)))
                                food.Add((k, j));
                            if (data.Carnivores.Exists(x => x?.Coord == (k, j) && animal.CanEat(x)))
                                food.Add((k, j));
                        }
                    }
                }
                foreach (var en in enemies)
                {
                    weightField[(en.x, en.y)] += pointNegativeWeight;
                    for (long i = en.y - weightRadius; i <= en.y + weightRadius; i++)
                    {
                        for (long j = en.x - weightRadius; j <= en.x + weightRadius; j++)
                        {
                            if (weightField.ContainsKey((j, i)) && !(i == en.y && j == en.x))
                            {
                                long tempDelta = Math.Max(Math.Abs(en.x - j), Math.Abs(en.y - i));
                                weightField[(j, i)] -= distantWeight[tempDelta - 1];
                            }
                        }
                    }
                }
                foreach (var fd in food)
                {
                    weightField[(fd.x, fd.y)] += pointPositiveWeight;
                    for (long i = fd.y - weightRadius; i <= fd.y + weightRadius; i++)
                    {
                        for (long j = fd.x - weightRadius; j <= fd.x + weightRadius; j++)
                        {
                            if (weightField.ContainsKey((j, i)) && !(i == fd.y && j == fd.x))
                            {
                                long tempDelta = Math.Max(Math.Abs(fd.x - j), Math.Abs(fd.y - i));
                                weightField[(j, i)] += distantWeight[tempDelta - 1];
                            }
                        }
                    }
                }

                return weightField;
            }
        }
    }
}
