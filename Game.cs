using static WolfGame.Game;
using Timer = System.Timers.Timer;

namespace WolfGame;

public enum Direction { dNorth, dSouth, dEast, dWest, dNorthEast, dNorthWest, dSouthEast, dSouthWest }

internal class Game
{
    public struct Data
    {
        private readonly uint _fieldWidth;
        private readonly uint _fieldHeight;
        private readonly List<Wolf> _wolfs;
        private readonly List<Rabbit> _rabbits;
        private readonly List<Grass> _grass;

        public uint FieldWidth => _fieldWidth;
        public uint FieldHeight => _fieldHeight;
        public List<Wolf> Wolfs => _wolfs;
        public List<Rabbit> Rabbits => _rabbits;
        public List<Grass> Grass => _grass;

        public Data(uint fieldWidth, uint fieldHeight, List<Wolf> wolfs, List<Rabbit> rabbits, List<Grass> grass)
        {
            _fieldWidth = fieldWidth;
            _fieldHeight = fieldHeight;
            _wolfs = wolfs;
            _rabbits = rabbits;
            _grass = grass;
        }
    }

    protected static Random _rnd = new Random();
    protected Timer _time;
    protected Data _data;

    public Game(uint fieldWidth = 20, uint fieldHeight = 20, uint countWolfs = 5, 
        uint countRabbit = 5, uint countGrass = 10, uint frames = 2000)
    {
        _data = new Data(fieldWidth, fieldHeight, CreateAllWolfs(countWolfs), CreateAllRabit(countRabbit), CreateAllGrass(countGrass));
        StartPlay(frames);
    }

    public virtual void StartPlay(float frames)
    {
        _time = new Timer(frames);
        _time.Elapsed += Update;
        _time.AutoReset = true;
        _time.Enabled = true;
    }

    public virtual void DrawGraphics()
    {

    }

    protected List<Wolf> CreateAllWolfs(uint countWolfs)
    {
        List<Wolf> res = new List<Wolf>();

        for (int i = 0; i < countWolfs; i++)
            res.Add(new Wolf());

        return res;
    }

    protected List<Rabbit> CreateAllRabit(uint countRabbit)
    {
        List<Rabbit> res = new List<Rabbit>();

        for (int i = 0; i < countRabbit; i++)
            res.Add(new Rabbit());

        return res;
    }

    protected List<Grass> CreateAllGrass(uint countGrass)
    {
        List<Grass> res = new List<Grass>();

        for (int i = 0; i < countGrass; i++)
            res.Add(new Grass());

        return res;
    }

    protected void Update(object? sender, System.Timers.ElapsedEventArgs e)
    {
        Console.WriteLine($"Игровой цикл идет: {_time.Interval / 1000}");

        foreach (var item in _data.Rabbits)
        {

        }

        foreach (var item in _data.Wolfs)
        {

        }

        foreach (var item in _data.Grass)
        {

        }

        DrawGraphics();
    }
}