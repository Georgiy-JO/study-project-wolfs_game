using System;
using System.Collections.Generic;
using System.Text;
using Wolf_Island.Core.Entities;

namespace Wolf_Island.Core.GameSnapshots;

public readonly struct ObjectStyleGameSnapshot
{
    public readonly record struct EntitySnapshot(string Id, (uint x, uint y) Coord);
    public uint Width { get; }
    public uint Height { get; }
    public IReadOnlyList<EntitySnapshot> Entities { get; }

    public ObjectStyleGameSnapshot(uint width, uint height, List<EntitySnapshot> entities)
    {
        Width = width; 
        Height = height; 
        Entities = entities;
    }
}

public readonly struct ListStyleGameSnapshot
{
    public uint Width { get; }
    public uint Height { get; }
    public IReadOnlyDictionary<string, List<(uint x, uint y)>> CoordListRegistry { get; }

    public ListStyleGameSnapshot(uint width, uint height, IReadOnlyDictionary<string, List<(uint x, uint y)>> coordRegistry)
    {
        Width = width; 
        Height = height; 
        CoordListRegistry = coordRegistry;
    }
}



/*
public struct FieldStyleGameSnapshot
{
    public readonly struct FieldValuesRegistry
    {
        public Dictionary<string, int> IdToVal { get; }
        public Dictionary<int, string> ValToId { get; }
        public FieldValuesRegistry(List<string> idList)
        {
            IdToVal = new();
            ValToId = new();
            for (int i = 1; i <= idList.Count; i++)
            {
                IdToVal[idList[i - 1]] = i;
                ValToId[i] = idList[i - 1];
            }
        }
    }
    public uint Width { get; }
    public uint Height { get; }
    public int[][] Field { get; set; }
    public FieldValuesRegistry? ValRegistry { get; }

    public FieldStyleGameSnapshot(uint width, uint height, int[][] field, FieldValuesRegistry? fv)
    {
        Width = width; 
        Height = height;
        Field = field ?? throw new Exception("Field was not alocated or is empty.");
        ValRegistry = fv;
    }
}

public readonly struct FieldStyleGameSnapshot
{
    public readonly struct FieldValuesRegistry
    {
        public IReadOnlyDictionary<string, int> IdToVal { get; }
        public IReadOnlyDictionary<int, string> ValToId { get; }

        public FieldValuesRegistry(IReadOnlyList<string> idList)
        {
            var idToVal = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var valToId = new Dictionary<int, string>();

            for (int i = 0; i < idList.Count; i++)
            {
                int value = i + 1; // 0 = empty
                idToVal[idList[i]] = value;
                valToId[value] = idList[i];
            }

            IdToVal = idToVal;
            ValToId = valToId;
        }
    }

    public uint Width { get; }
    public uint Height { get; }
    public IReadOnlyList<IReadOnlyList<int>> Field { get; }
    public FieldValuesRegistry ValRegistry { get; }

    public FieldStyleGameSnapshot(
        uint width,
        uint height,
        int[][] field,
        FieldValuesRegistry registry)
    {
        Width = width;
        Height = height;
        Field = field;
        ValRegistry = registry;
    }
}

public FieldStyleGameSnapshot GetFieldStyleGameSnapshot()
    {
        int h = (int)_data.Height;
        int w = (int)_data.Width;

        var field = new int[h][];
        for (int y = 0; y < h; y++)
            field[y] = new int[w]; // default 0 = empty

        var reg = new FieldStyleGameSnapshot.FieldValuesRegistry(_factory.GetPossibleIDs());

        void Place(Entity entity)
        {
            var (x, y) = entity.Coord;
            if (y >= _data.Height || x >= _data.Width) return;

            if (reg.IdToVal.TryGetValue(entity.GetType().Name, out int value))
                field[y][x] = value;
        }

        foreach (var animal in _data.Animals) Place(animal);
        foreach (var food in _data.Food) Place(food);

        return new FieldStyleGameSnapshot(_data.Width, _data.Height, field, reg);
    }
    public FieldStyleGameSnapshot GetFieldStyleGameSnapshot(bool reset = false)
    {
        int[][] field = new int[_data.Height][];
        for (int i = 0; i < _data.Height; i++)
        {
            field[i] = new int[_data.Width];
            for (int j = 0; j < _data.Width; j++)
                field[i][j] = 0;
        }
        FieldStyleGameSnapshot.FieldValuesRegistry reg = new(_factory.GetPossibleIDs());

        foreach (var id in _data.Animals)
            if (id.Coord.y < _data.Height && id.Coord.y >= 0 && id.Coord.x < _data.Width && id.Coord.x >= 0)
                field[id.Coord.y][id.Coord.x] = reg.IdToVal.ContainsKey(id.GetType().Name) ? reg.IdToVal[id.GetType().Name] : field[id.Coord.y][id.Coord.x];
        foreach (var id in _data.Food)
            if (id.Coord.y < _data.Height && id.Coord.y >= 0 && id.Coord.x < _data.Width && id.Coord.x >= 0)
                field[id.Coord.y][id.Coord.x] = reg.IdToVal.ContainsKey(id.GetType().Name) ? reg.IdToVal[id.GetType().Name] : field[id.Coord.y][id.Coord.x];

        return new FieldStyleGameSnapshot(_data.Width, _data.Height, field, reg);
    }
    */

