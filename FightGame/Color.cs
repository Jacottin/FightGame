using System;

namespace FightGame {
    public class Color {
        public static ConsoleColor GetColor(char c) {
            switch (c) {
                case 'R':
                    return ConsoleColor.Red;
                case 'B':
                    return ConsoleColor.Blue;
                case 'b':
                    return ConsoleColor.Black;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}