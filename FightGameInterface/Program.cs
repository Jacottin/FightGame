using System;
using FightGame.Characters;

namespace FightGameInterface {
    public static class Program {
        [STAThread]
        static void Main() {
            using (var game = new MainGame())
                game.Run();
        }
    }
}