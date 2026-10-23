using System;
using System.Collections.Generic;
using System.Text;
using WolfGame;

namespace Wolf_Island.Entities.Animals.Mobs
{
    internal class Rabbit : Herbivore
    {
        //protected override uint MaxHealth { get => 4; }
        //protected override uint MaxFullness { get => 3; }
        //protected override uint MaxMoveDistance { get => 1; }   // not used
        //public override uint MeatValue { get => 2; }
        //protected override uint FieldOfView { get => 1; }


        /// <summary>
        /// Creating a rabbit.
        /// maxHealth - how much health it has (health == 0 - death), 
        /// maxFullness - how hungry it can get until it starts loosing health,
        /// meatValue - how much value will its remains Food:Remains has, 
        /// fieldOfView - how far on the field can it see, 
        /// size:Size.szSmall - size of the animal (important for the life chain - who eats who),
        /// maxHungerTimer - how many moves it can make before becoming hungry, 
        /// maxLifelossTimer - how fast will it lose health if it's hungry, 
        /// coordinates - spawn location
        /// </summary>
        /// <param name="coordinates">spawn location</param>
        public Rabbit((uint x, uint y) coordinates = default) : base(maxHealth: 4, maxFullness: 3,
                                                                     meatValue: 2, FieldOfView: 2, size: Size.szSmall,
                                                                     maxHungerTimer: 1, maxLifelossTimer: 1,
                                                                     coordinates)
        { }


        public override void Update(Game.Data data)
        {
            Movehandler(data);              //  handle moving
            Status();                       // hanger and life handle
            Feedhandler(data);              // handle feeding
        }
        protected override void Movehandler(Game.Data data)
        {
            var weightField = Service.WeightField(data, this);
            var possibleMoves = (Enum.GetValues<Game.Direction>().Select(dir => Game.GetCoords(dir, Coord)).ToArray()).GetShuffledCollections();
            ((long x, long y), int weight) theMove = (Coord, -5);

            foreach (var possibleMove in possibleMoves)
            {
                if (theMove.weight < weightField[possibleMove] && (possibleMove.x >= 0 && possibleMove.y >= 0 && possibleMove.x < data.Width && possibleMove.y < data.Height))       // second check is not really needed
                    theMove = (possibleMove, weightField[possibleMove]);
            }
            SetCoord((uint)theMove.Item1.x, (uint)theMove.Item1.y);
        }
        protected override void Feedhandler(Game.Data data)
        {
            var tmpGrassThere = data.Grass.Find(x => x.Coord == (Coord));
#pragma warning disable CS8604 // Possible null reference argument.
            if (this.Feed(tmpGrassThere)) data.Grass.Remove(tmpGrassThere);
#pragma warning restore CS8604 // Possible null reference argument.
        }

    }
}
