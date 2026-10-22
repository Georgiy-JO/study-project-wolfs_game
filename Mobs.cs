using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;

namespace WolfGame
{
    //public struct LocalField{
        

            
    //    private Dictionary<(long _x,long _y), int> _weight;
    //    public static int _size = 7;

    //    public int GetWeight(long x,long y) => _weight[(x,y)];
    //    public void SetWeight(int val) => _weight[(x, y)] = val;
    //    public LocalField(uint x, uint y)
    //    {
    //        _weight = new Dictionary<(long _x, long _y), int>();
    //        for(long i =0;i< _size; i++)
    //        {
    //            for(long j =0;j< _size; j++)
    //            {
    //                _weight[((long)y - i - 3, (long)x - j - 3)]=0;
    //            }
    //        }

    //    }

    //}


    internal  class Wolf: Carnivore
    {
        protected override uint MaxHealth { get => 8; }
        protected override uint MaxFullness { get => 6; }
        protected override uint MaxMoveDistance { get => 2; }
        public override uint MeatValue { get => 5; }

        public Wolf((uint x, uint y) coordinates = default) : base(5,6,Animal.Size.szMedium, coordinates) { }
        public override bool Update(Game.Data data) 
        {

        }

    }
    internal class Rabbit: Herbivore
    {
        protected override uint MaxHealth { get => 4; }
        protected override uint MaxFullness { get => 3; }
        protected override uint MaxMoveDistance { get => 1; }
        public override uint MeatValue { get => 2; }

        public Rabbit((uint x, uint y) coordinates = default) : base(5, 6, Animal.Size.szMedium, coordinates) { }
        public override bool Update(Game.Data data) 
        {
            var weightField = WeightField(data);
            ()

            if (Fullness == MaxFullness)
            {
                // random
            }
            else
            {

            }

            return true;
        }
        protected override Dictionary<(long x, long y), int> WeightField(Game.Data data)
        {

            Dictionary<(long x, long y), int> weightField = new ();
            (long x, long y) beginCoord = ((long)Coord.x - FirldOfView, (long)Coord.y - FirldOfView);
            (long x, long y) endCoord = ((long)Coord.x + FirldOfView, (long)Coord.y + FirldOfView);

            // Food - 15  -  2  -  1
            // wolf - -15  -  -2  -  -1
            // edge -  -50
            int edgeWeight = -50;
            int pointWeight = 15;
            //int closeWeight = 2;
            int distantWeight = 1;

            List<(long x, long y)> enemies = new();
            List<(long x, long y)> food = new();
            for (long j=beginCoord.y; j<=endCoord.y; j++)
            {
                for (long k = beginCoord.x; k <= endCoord.x; k++)
                {
                    if (j < 0 || k < 0 || j > data.Height || k > data.Width) weightField[(k, j)] = edgeWeight;
                    else
                    {
                        weightField[(k, j)] = 0;
                        if (data.Wolfs.Exists(x => x.Coord == (k, j)))
                            enemies.Add((k, j));
                        if (data.Grass.Exists(x => x.Coord == (k, j)))
                            food.Add((k, j));
                    }
                }
            }
            foreach (var en in enemies)
            {
                weightField[(en.x, en.y)] -= pointWeight;
                for(long i= en.y - 1; i <= en.y + 1; i++)
                {
                    for (long j = en.x - 1; j <= en.x + 1; j++)
                    {
                        if(weightField.ContainsKey((j, i)))         weightField[(j, i)] -= distantWeight;
                    }
                }
            }
            foreach (var fd in food)
            {
                weightField[(fd.x, fd.y)] += pointWeight;
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

