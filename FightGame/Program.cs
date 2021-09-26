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
            int turn = 1;
            Character player = new Healer("");
            Character cpu = new Healer("");

            Console.WriteLine("FightGame : Le jeu de combat incroyablement passionant");
            Console.WriteLine("Copyright Jacottin, Romaka & TNtube");
            Console.WriteLine("Publié par personne, mais peut-être un jour EA pour payer les updates \n");
            Console.WriteLine("Veuillez entrer votre nom.");
            string playerName = Console.ReadLine();

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
                    //player = new Healer(playerName);
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
                    //cpu = new Healer("Ordinateur");
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

                Console.WriteLine(player.getClassName());
                //Console.WriteLine($"[{player._lifePoints} HP]");
                Console.WriteLine("");
                Console.WriteLine(cpu.getClassName());
                //Console.WriteLine($"[{cpu._lifePoints} HP]");
                Console.WriteLine("");

                // Action Choice

                Console.WriteLine("Que voulez vous faire ?");
                Console.WriteLine("1 - Attaquer");
                Console.WriteLine("2 - Se défendre");
                Console.WriteLine("3 - CAPACITÉ SPÉCIALE");
                string playerAction = Console.ReadLine();
                string playerLastAction;
                switch (playerAction)
                {
                    case "1":
                        player.Attack(cpu);
                        player.setLastAction("attack");
                        break;
                    case "2":
                        player.Defend(cpu);
                        player.setLastAction("defend");
                        break;
                    case "3":
                        player.SpecialCapacity();
                        player.setLastAction("special");
                        break;
                }

                int cpuAction = rng.Next(1, 3);
                string cpuLastAction;
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
            else
                Console.WriteLine("Vous avez perdu !");
            Console.ReadKey();
        }
    }
}