using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace WolfGame
{
    using FoodC = Food.FoodKind;
    using Timer = System.Timers.Timer;
    internal class TestGame
    {
        //private static Food[] foodsToFeed=new Food[]{
        //    new Food( "Cabage", 7, FoodC.fkGrassy),
        //    new Food( "Eggplant", 4, FoodC.fkGrassy),
        //    new Food( "Mouse", 2, FoodC.fkMeaty),
        //    new Food( "Stake", 8, FoodC.fkMeaty),
        //    new Food( "Fish", 5, FoodC.fkMeaty),
        //    new Food( "CatMint", 1, FoodC.fkGrassy),
        //};
        //private TimeOnly timer;
        private static Random feeder;
        private Timer simulationTimer;
        //private Cat Musia;


        public TestGame()
        {
            //timer = new TimeOnly(TimeOnly.FromDateTime(DateTime.Now).Hour, TimeOnly.FromDateTime(DateTime.Now).Minute, TimeOnly.FromDateTime(DateTime.Now).Second);
            simulationTimer = new Timer(2000);
            //simulationTimer.Start();
            feeder = new Random();

            simulationTimer.Elapsed += OnSimulationTimerTick;
            simulationTimer.AutoReset = true;
            simulationTimer.Enabled = true;



            //Musia =new Cat();
        }

        ~TestGame()
        {
            simulationTimer.Stop();
            simulationTimer.Dispose();
        }

        private void OnSimulationTimerTick(object? sender, System.Timers.ElapsedEventArgs e)
        {
            //Console.WriteLine($"Time: {simulationTimer.Interval/1000} ");
            //Console.WriteLine($"I'm {(Musia.Status((uint)(simulationTimer.Interval / 1000))?"Alive":"Dead")}");
            //Musia.Feed(foodsToFeed[feeder.Next(0,foodsToFeed.Length)]);

            //simulationTimer.Interval = feeder.Next(1, 10)*1000;
        }
    }
}
