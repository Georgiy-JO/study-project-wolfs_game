using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;

namespace WolfGame
{
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
        private void FindEnemyAndFood(Game.Data data, out List<(uint x, uint y)> enemies, out List<(uint x, uint y)> food)
        {
            enemies = new List<(uint x, uint y)>();
            food = new List<(uint x, uint y)>();
            for (int i = 1; i <= 3; i++)
            {
                (uint x, uint y) beginCoord = ((uint)(Math.Max((long)Coord.x - i, 0)), (uint)(Math.Max((long)Coord.y - i, 0)));
                (uint x, uint y) endCoord = ((uint)(Math.Min((long)Coord.x + i, 0)), (uint)(Math.Min((long)Coord.y + i, 0)));

                for(uint j=beginCoord.x; j<=endCoord.x; j++)
                {
                    for (uint k = beginCoord.x; k <= endCoord.x; k++)
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
}

