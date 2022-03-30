using Microsoft.Xna.Framework.Input;

namespace FightGameInterface.Utils {

    public class Keyboard
    {
        private static KeyboardState _currentKeyState;
        private static KeyboardState _previousKeyState;

        public static KeyboardState GetState()
        {
            _previousKeyState = _currentKeyState;
            _currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            return _currentKeyState;
        }

        public static bool IsPressed(Keys key)
        {
            return _currentKeyState.IsKeyDown(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
        }
    }
}