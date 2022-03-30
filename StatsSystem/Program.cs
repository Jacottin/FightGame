using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FightGame.Characters;

namespace StatsSystem {
    class Program {
        static Random random = new Random();
        
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: FightGame.Characters.Tank")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: FightGame.Characters.Healer")]
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: FightGame.Characters.Damager")]
        static void Main(string[] args) {
            Console.WriteLine("How many simulation do you want ?");
            int nbSimulation;

            while (true) {
                try {
                    nbSimulation = int.Parse(Console.ReadLine()!);
                    break;
                }
                catch (Exception) {
                    Console.WriteLine("Please entry a valid number.");
                }
            }

            Console.WriteLine("Processing...");

            List<string> types = new List<string>{ "H", "D", "T", "A" };
            Dictionary<string, int> results = new Dictionary<string, int>();
            types.ForEach(x => types.ForEach(y => results.Add(x + y, 0)));
            

            for (int i = 0; i < nbSimulation; i++) {
                // HEALER VS DAMAGER
                if (IsWinning(new Healer("P1"), new Damager("P2"))) {
                    results["HD"]++;
                }
                // HEALER VS TANK
                if (IsWinning(new Healer("P1"), new Tank("P2"))) {
                    results["HT"]++;
                }
                // HEALER VS ANALYST
                if (IsWinning(new Healer("P1"), new Analyst("P2"))) {
                    results["HA"]++;
                }
                // DAMAGER VS HEALER
                if (IsWinning(new Damager("P1"), new Healer("P2"))) {
                    results["DH"]++;
                }
                // DAMAGER VS TANK
                if (IsWinning(new Damager("P1"), new Tank("P2"))) {
                    results["DT"]++;
                }
                // DAMAGER VS ANALYST
                if (IsWinning(new Damager("P1"), new Analyst("P2"))) {
                    results["DA"]++;
                }
                // TANK VS HEALER
                if (IsWinning(new Tank("P1"), new Healer("P2"))) {
                    results["TH"]++;
                }
                // TANK VS DAMAGER
                if (IsWinning(new Tank("P1"), new Damager("P2"))) {
                    results["TD"]++;
                }
                // TANK VS ANALYST
                if (IsWinning(new Tank("P1"), new Analyst("P2"))) {
                    results["TA"]++;
                }
                // ANALYST VS HEALER
                if (IsWinning(new Analyst("P1"), new Healer("P2"))) {
                    results["AH"]++;
                }
                // ANALYST VS DAMAGER
                if (IsWinning(new Analyst("P1"), new Damager("P2"))) {
                    results["AD"]++;
                }
                // ANALYST VS TANK
                if (IsWinning(new Analyst("P1"), new Tank("P2"))) {
                    results["AT"]++;
                }
            }

            DisplayResult(results, types, nbSimulation);

            Console.ReadLine();

        }

        private static bool IsWinning(Character player1, Character player2) {
            while (player1.getLife() > 0 && player2.getLife() > 0) {
                switch (random.Next(1, 4)) {
                    case 1:
                        player1.Attack(player2);
                        break;
                    case 2:
                        player1.Defend(player2);
                        break;
                    case 3:
                        player1.SpecialCapacity();
                        break;
                }
                switch (random.Next(1, 4)) {
                    case 1:
                        player2.Attack(player1);
                        break;
                    case 2:
                        player2.Defend(player1);
                        break;
                    case 3:
                        player2.SpecialCapacity();
                        break;
                }
                player1.Update(player2);
                player2.Update(player1);
                player1.ComputeDamages();
                player2.ComputeDamages();
            }

            int result = player1.getLife();
            return result > 0;
        }

        private static void DisplayResult(Dictionary<string, int> results, List<string> types, int nbTotal) {
            Console.WriteLine(new string(' ', 6) + string.Join("     ", types));
            Console.WriteLine("   ┌─────" + string.Join("─────", Enumerable.Repeat("┬", types.Count-1)) + "─────┐");
            foreach (var type1 in types) {
                Console.Write($" {type1} │");
                foreach (var type2 in types) {
                    Console.Write($" {results[type1 + type2]* 100 / nbTotal:D2}% │");
                }
                if (type1 != types.Last())
                    Console.WriteLine("\n   ├─────" + string.Join("─────", Enumerable.Repeat("┼", types.Count-1)) + "─────┤");
            }
            Console.WriteLine("\n   └─────" + string.Join("─────", Enumerable.Repeat("┴", types.Count-1)) + "─────┘");
        }
    }
}