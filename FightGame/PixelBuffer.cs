using System;

namespace FightGame {
    public struct PixelBuffer {
        public char asciiChar;
        public ConsoleColor backgroundColor;
        public ConsoleColor foregroundColor;

        public PixelBuffer(char asciiChar, ConsoleColor backgroundColor = ConsoleColor.Black,
            ConsoleColor foregroundColor = ConsoleColor.White) {
            this.asciiChar = asciiChar;
            this.backgroundColor = backgroundColor;
            this.foregroundColor = foregroundColor;
        }
    }
}