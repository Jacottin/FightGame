using System;
using System.IO;

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

        public void DrawSprite(Sprite sprite, int x, int y) {
            for (int i = 0; i < sprite.w; i++) {
                for (int j = 0; j < sprite.h; j++) {
                    _buffer[(x + i) + (y * _width + j * _width)] = sprite.spriteData[i + j * sprite.w];
                }
            }
        }
    }

    public class Sprite {
        public int w = 0;
        public int h = 0;
        public PixelBuffer[] spriteData;

        public Sprite(string filename) {
            using (StreamReader file = new StreamReader(filename)) {
                string ln;
                while ((ln = file.ReadLine()) != null) {
                    w = ln.Length > w ? ln.Length : w;
                    h++;
                }
                file.Close();
            }

            spriteData = new PixelBuffer[w * h];
            
            using (StreamReader file = new StreamReader(filename)) {
                string ln;
                int _y = 0;
                while ((ln = file.ReadLine()) != null) {
                    int _x = 0;
                    foreach (char c in ln) {
                        ConsoleColor bg = Color.GetColor(c);
                        spriteData[_x + _y * w] = new PixelBuffer(' ', bg);
                        _x++;
                    }
                    _y++;
                }
                file.Close();
            }
        }
    }
}