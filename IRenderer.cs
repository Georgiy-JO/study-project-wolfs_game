using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Wolf_Island
{
    public interface IRenderer
    {
        void Render(GameSnapshot snapshot);
    }

    public class GameSnapshot
    {
        private uint _width;
        private uint _height;
        private ICollection<Cell> _cells = new List<Cell>();

        public uint Width => _width;
        public uint Height => _height;
        public IEnumerable<Cell> Cells => _cells;

        public GameSnapshot(uint width, uint height, ICollection<Cell> cells)
        {
            _width = width;
            _height = height;
            _cells = cells;
        }
    }

    public struct Cell
    {
        public enum CellType { Empty, Grass, Rabbit, Wolf }

        private uint _x, _y;
        private CellType _type;

        public uint X => _x;
        public uint Y => _y;
        public CellType Type => _type;

        public Cell(uint x, uint y, CellType type)
        {
            _x = x;
            _y = y;
            _type = type;
        }
    }
}