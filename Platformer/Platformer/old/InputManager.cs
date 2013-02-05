using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace old
{
    class InputManager
    {
        KeyboardState _currentKeyState, _previousKeyState;

        public InputManager()
        {
            Update();
        }

        public bool KeyDown(Keys key)
        {
            return _currentKeyState.IsKeyDown(key);
        }

        public bool KeyUp(Keys key)
        {
            return _currentKeyState.IsKeyUp(key);
        }

        /*there is a key (no pun intended) distinction between pressing and holding a key.
         *The KeyDown method just checks if a key is being held.
         *The KeyPressed method checks if a key was previously up, but now is down
         */
        public bool KeyPressed(Keys key)
        {
            return _currentKeyState.IsKeyUp(key) && _previousKeyState.IsKeyDown(key);
        }

        public void Update()
        {
            //save previous key state and get the new one
            _previousKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();
        }
    }
}
