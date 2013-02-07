using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace Platformer.Control
{
    class InputManager
    {
        #region static
        #endregion

        #region fields
        //Must keep track of previous state as well as current state
        //To check if a key is held, just check IsKeyDown for the current state
        //To check if a key was just pressed, check that key was up previously, down currently
        KeyboardState _pastKeyState, _currentKeyState;
        MouseState _pastMouseState, _currentMouseState;
        #endregion

        #region properties
        /// <summary>
        /// Whether the player is holding the left movement key
        /// </summary>
        public bool MoveLeft { get { return _currentKeyState.IsKeyDown(Keys.Left); } }
        /// <summary>
        /// Whether the player is holding the right movement key
        /// </summary>
        public bool MoveRight { get { return _currentKeyState.IsKeyDown(Keys.Right); } }
        /// <summary>
        /// Whether the player has tapped the left movement key
        /// </summary>
        public bool SelectLeft
        {
            get { return _currentKeyState.IsKeyDown(Keys.Left) && _pastKeyState.IsKeyUp(Keys.Left); }
        }
        /// <summary>
        /// Whether the player has tapped the right movement key
        /// </summary>
        public bool SelectRight
        {
            get { return _currentKeyState.IsKeyDown(Keys.Right) && _pastKeyState.IsKeyUp(Keys.Right); }
        }
        /// <summary>
        /// Whether the player has tapped the jump button
        /// </summary>
        public bool MoveUp
        {
            get { return _currentKeyState.IsKeyDown(Keys.Up) && _pastKeyState.IsKeyUp(Keys.Up); }
        }
        /// <summary>
        /// Whether the player has tapped the fire button
        /// </summary>
        public bool Fire
        {
            get { return _currentKeyState.IsKeyDown(Keys.Space) && _pastKeyState.IsKeyUp(Keys.Space); }
        }
        #endregion

        #region constructor
        public InputManager()
        {
            Update();
        }
        #endregion

        #region methods
        /// <summary>
        /// Game1.cs should call this every update cycle before Updating the GameState
        /// </summary>
        public void Update()
        {
            //get new input states and save old ones
            _pastKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();
            _pastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }
        #endregion
    }
}
