using System;
using FightGame.Characters;

namespace FightGame {
    class Program {
        static void Main(string[] args)
        {
            // Initializations

            Random rng = new Random();
            bool gameRunning = true;
            Character player;
            Character cpu;
            int turn = 1;
            
            // Character select
            
            Console.WriteLine("+-----------+");
            Console.WriteLine("| FIGHTGAME |");
            Console.WriteLine("+-----------+");
            Console.WriteLine("");
            Console.WriteLine("Choisissez votre classe : ");
            Console.WriteLine("");
            Console.WriteLine("1 - Tank");
            Console.WriteLine("2 - Healer");
            Console.WriteLine("3 - Damager");
            Console.WriteLine("");
            
            string playerChoice = Console.ReadLine();

            switch (playerChoice)
            {
                case "1":
                    //player = new Tank();
                    break;
                case "2":
                    player = new Healer();
                    break;
                case "3":
                    //player = new Damager();
                    break;
            }
            
            Console.WriteLine($"Vous avez choisi {player}.");
            
            // CPU character select
            
            int cpuChoice = rng.Next(1, 3);
            switch (cpuChoice)
            {
                case 1:
                    //cpu = new Tank();
                    break;
                case 2:
                    cpu = new Healer();
                    break;
                case 3:
                    //cpu = new Damager();
                    break;
            }
            
            // Gameplay loop

            while (gameRunning == true)
            {
                Console.WriteLine("+--------+");
                Console.WriteLine($"Tour {turn}");
                Console.WriteLine("+--------+");
                Console.WriteLine("");
                
            }

        }
    }
}