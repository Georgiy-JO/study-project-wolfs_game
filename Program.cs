namespace Wolf_Island
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            StartPlay(2000, game);
        }

        static void StartPlay(uint frames, Game game)
        {
            IRenderer view = new ConsoleView();

            while (true)
            {
                game.Tick();
                view.Render(game.GetSnapshot());

                Thread.Sleep((int)frames);
            }
        }
    }
}