using System;
using FightGame.Characters;

namespace FightGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initializations

            Random rng = new Random();
            bool gameRunning = true;
            Character player = new Healer("Joueur");
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
                    // player = new Healer("Joueur");
                    break;
                case "3":
                    //player = new Damager();
                    break;
            }

            Console.WriteLine($"Vous avez choisi {player.getClassName()}.");

            // CPU character select

            int cpuChoice = rng.Next(1, 3);
            switch (cpuChoice)
            {
                case 1:
                    //cpu = new Tank();
                    break;
                case 2:
                    // cpu = new Healer();
                    break;
                case 3:
                    //cpu = new Damager();
                    break;
            }

            // Gameplay loop

            while (gameRunning)
            {
                Console.WriteLine("+--------+");
                Console.WriteLine($"Tour {turn}");
                Console.WriteLine("+--------+");
                Console.WriteLine("");

                Console.WriteLine(player._className);
                //Console.WriteLine($"[{player._lifePoints} HP]");
                Console.WriteLine("");
                Console.WriteLine(cpu._className);
                //Console.WriteLine($"[{cpu._lifePoints} HP]");
                Console.WriteLine("");

                // Action Choice

                Console.WriteLine("Que voulez vous faire ?");
                string playerAction = Console.ReadLine();

                switch (playerAction)
                {
                    case "1":
                        player.Attack(cpu);
                        break;
                    case "2":
                        player.Defend(cpu);
                        break;
                    case "3":
                        player.SpecialCapacity();
                        break;
                }

                int cpuAction = rng.Next(1, 3);
                switch (cpuAction)
                {
                    case 1:
                        cpu.Attack(player);
                        break;
                    case 2:
                        cpu.Defend(player);
                        break;
                    case 3:
                        cpu.SpecialCapacity();
                        break;
                }

                Console.WriteLine("Vous"); // TODO : gérer les annonces d'actions
                Console.WriteLine($"Le {player._className} ");

                player.Update(cpu);
                cpu.Update(player);

                if (player._lifePoints == 0 || cpu._lifePoints)
                    gameRunning = false;
            }
        }
    }
}