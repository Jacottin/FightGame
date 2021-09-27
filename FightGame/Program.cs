using System;
using System.Collections.Generic;
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
            int turn = 1;
            Character player = new Healer("Foo");
            Character cpu = new Healer("Bar");

            Console.WriteLine("FightGame : Le jeu de combat incroyablement passionant");
            Console.WriteLine("Copyright Jacottin, Romaka & TNtube");
            Console.WriteLine("Publié par personne, mais peut-être un jour EA pour payer les updates \n");
            Console.WriteLine("Veuillez entrer votre nom.");
            string playerName = Console.ReadLine();

            // Character select

            Console.WriteLine("+-----------+");
            Console.WriteLine("| FIGHTGAME |");
            Console.WriteLine("+-----------+\n");
            Console.WriteLine("Choisissez votre classe : \n");
            Console.WriteLine("1 - Tank");
            Console.WriteLine("2 - Healer");
            Console.WriteLine("3 - Damager\n");

            string playerChoice = Console.ReadLine();

            while (!new List<string>{"1", "2", "3"}.Contains(playerChoice)) {
                switch (playerChoice)
                {
                    case "1":
                        player = new Tank(playerName);
                        break;
                    case "2":
                        player = new Healer(playerName);
                        break;
                    case "3":
                        player = new Damager(playerName);
                        break;
                    default:
                        Console.WriteLine("Choisissez une classe valide");
                        break;
                }
            }
            

            Console.WriteLine($"Vous avez choisi le {player.getClassName()}.");

            // CPU character select

            int cpuChoice = rng.Next(1, 3);
            switch (cpuChoice)
            {
                case 1:
                    cpu = new Tank("Ordinateur");
                    break;
                case 2:
                    cpu = new Healer("Ordinateur");
                    break;
                case 3:
                    cpu = new Damager("Ordinateur");
                    break;
            }
            
            Console.WriteLine($"L'ordinateur a choisi le {cpu.getClassName()}.");

            // Gameplay loop

            while (gameRunning)
            {
                Console.WriteLine("+--------+");
                Console.WriteLine($"Tour {turn++}");
                Console.WriteLine("+--------+\n");

                Console.WriteLine(player.getUserName());
                Console.WriteLine($"[{player.getLife()} HP]\n");
                Console.WriteLine(cpu.getUserName());
                Console.WriteLine($"[{cpu.getLife()} HP]\n");

                // Action Choice

                Console.WriteLine("Que voulez vous faire ?");
                Console.WriteLine("1 - Attaquer");
                Console.WriteLine("2 - Se défendre");
                Console.WriteLine("3 - CAPACITÉ SPÉCIALE");
                bool validChoice = false;
                while (!validChoice) {
                    string playerAction = Console.ReadLine();
                    switch (playerAction)
                    {
                        case "1":
                            player.Attack(cpu);
                            player.setLastAction("attack");
                            validChoice = true;
                            break;
                        case "2":
                            player.Defend(cpu);
                            player.setLastAction("defend");
                            validChoice = true;
                            break;
                        case "3":
                            player.SpecialCapacity();
                            player.setLastAction("special");
                            validChoice = true;
                            break;
                        default:
                            Console.WriteLine("Veuillez choisir une action valide.");
                            break;
                    }
                }

                int cpuAction = rng.Next(1, 4);
                switch (cpuAction)
                {
                    case 1:
                        cpu.Attack(player);
                        cpu.setLastAction("attack");
                        break;
                    case 2:
                        cpu.Defend(player);
                        cpu.setLastAction("defend");
                        break;
                    case 3:
                        cpu.SpecialCapacity();
                        cpu.setLastAction("special");
                        break;
                }

                player.actionDisplay();
                cpu.actionDisplay();

                player.Update(cpu);
                cpu.Update(player);

                if (player.getLife() <= 0 || cpu.getLife() <= 0)
                    gameRunning = false;
            }

            if (player.getLife() > cpu.getLife())
                Console.WriteLine("Vous avez gagné !");
            else if (player.getLife() == cpu.getLife())
                Console.WriteLine("Égalité");
            else
                Console.WriteLine("Vous avez perdu !");
            Console.ReadKey();
        }
    }
}