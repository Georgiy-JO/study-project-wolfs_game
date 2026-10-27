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
        /// <summary>
        /// An Animal constructor.
        /// </summary>
        /// <param name="maxHealth">how much health it has (health == 0 - death)</param>
        /// <param name="maxFullness">how hungry it can get until it starts loosing health</param>
        /// <param name="meatValue">how much value will its remains Food:Remains has</param>
        /// <param name="fieldOfView">how far on the field can it see</param>
        /// <param name="size">size of the animal (important for the life chain - who eats who)</param>
        /// <param name="hungerTimer">how many moves it can make before becoming hungry</param>
        /// <param name="lifelossTimer">how fast will it lose health if it's hungry</param>
        record struct AnimalData(uint MaxHealth = 1, uint MaxFullness = 1, uint MeatValue = 1, uint FieldOfView = 1, Sizes Size = Sizes.szMedium, uint HungerTimer = 2, uint LifelossTimer = 2);

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
                            if (data.Food.Exists(x => x?.Coord == (k, j) && animal.CanEat(x)))
                                food.Add((k, j));
                            if (data.Animals.Exists(x => x?.Coord == (k, j) && animal.CanEat(x)))
                                food.Add((k, j));
                            if (data.Animals.Exists(x => x?.Coord == (k, j) && x.CanEat(animal)))
                                enemies.Add((k, j));
                            //if (data.Carnivores.Exists(x => x?.Coord == (k, j) && x.CanEat(animal)))
                            //    enemies.Add((k, j));
                            //if (data.Carnivores.Exists(x => x?.Coord == (k, j) && animal.CanEat(x)))
                            //    food.Add((k, j));
                            //if (data.Herbivores.Exists(x => x?.Coord == (k, j) && animal.CanEat(x)))
                            //    food.Add((k, j));
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

        internal static class ConstantsLoader
        {
            static public AnimalData Rabbit()
            {
                return new AnimalData(MaxHealth: 4, MaxFullness: 3, MeatValue: 2, FieldOfView: 2, Size: Sizes.szSmall, HungerTimer: 1, LifelossTimer: 1);

            }
            static public AnimalData Wolf()
            {
                return new AnimalData(MaxHealth: 6, MaxFullness: 6, MeatValue: 5, FieldOfView: 3, Size: Sizes.szMedium, HungerTimer: 2, LifelossTimer: 1);

            }
            
        }
    }
}
