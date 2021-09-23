using System;

namespace FightGame {
    public class Display {
        private PixelBuffer[] _buffer;

        private int _width;
        private int _height;
        

        public Display(int width, int height) {
            _buffer = new PixelBuffer[width * height];
            _width = width;
            _height = height;
        }

        public void DisplayBuffer() {
            for (int i = 0; i < _width; i++) {
                for (int j = 0; j < _height; j++) {

                    PixelBuffer current = _buffer[i + j * _width];
                    
                    Console.SetCursorPosition(i, j);
                    Console.BackgroundColor = current.backgroundColor;
                    Console.ForegroundColor = current.foregroundColor;
                    Console.Write(current.asciiChar);
                    Console.ResetColor();
                }
            }
        }


    }
}