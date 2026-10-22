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
            List<(uint x, uint y)> enemies;
            List<(uint x, uint y)> food;
            FindEnemyAndFood(data, out enemies, out food);

            if (Fullness == MaxFullness)
            {
                Random rnd = new Random();
                do
                {
                    //here
                } while ();

            }
            else
            {

            }

            return true;
        }
        protected override Dictionary<(long x, long y), int> WeightField(Game.Data data)
        {

            Dictionary<(long x, long y), int> weightField = new ();
            (long x, long y) beginCoord = ((long)Coord.x - 3, (long)Coord.y - 3);
            (long x, long y) endCoord = ((long)Coord.x + 3, (long)Coord.y + 3);

           
                

            for(long j=beginCoord.x; j<=endCoord.x; j++)
            {
                for (long k = beginCoord.x; k <= endCoord.x; k++)
                {

                    if (data.Wolfs.Exists(x => x.Coord == (j, k)))
                        enemies.Add((j, k));
                    if (data.Grass.Exists(x => x.Coord == (j, k)))
                        food.Add((j, k));
                }
            }
            
        }

    }
}

