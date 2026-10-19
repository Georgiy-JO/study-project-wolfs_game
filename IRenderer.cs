using System;
using System.Collections.Generic;
using System.Text;

namespace Wolf_Island
{
    public interface IRenderer
    {
        void Render(GameSnapshot snapshot);
    }

    public class GameSnapshot
    {
        public uint Width { get; init; }
        public uint Height { get; init; }
        public IReadOnlyList<Cell> Cells { get; init; } = Array.Empty<Cell>();
    }

    public enum CellKind { Empty, Grass, Rabbit, Wolf }

    public struct Cell
    {
        public uint X, Y;
        public CellKind Kind;
    }
}