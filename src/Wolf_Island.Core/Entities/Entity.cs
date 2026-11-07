using System;
using System.Collections.Generic;
using System.Text;

namespace Wolf_Island.Core.Entities
{
    internal abstract class Entity
    {
        public (uint x, uint y) Coord { get; protected set; }
    }
}
