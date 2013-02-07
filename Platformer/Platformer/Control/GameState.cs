using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Control
{
    abstract class GameState
    {
        #region static
        #endregion

        #region fields
        #endregion

        #region properties
        /// <summary>
        /// When this is null, the state should continue running
        /// When the state wants to transition to another state,
        /// It will set this to an initialized instance of the new state.
        /// Game1.cs will read this and set the new gamestate
        /// </summary>
<<<<<<< HEAD
        public GameState NewState;
        /// <summary>
        /// Whether the user has requested to exit the game (close everything)
        /// </summary>
        public bool RequestExit;
=======
        GameState NewState;
        /// <summary>
        /// Whether the user has requested to exit the game (close everything)
        /// </summary>
        bool RequestExit;
>>>>>>> 7a81e6891a1fdcbea58ff76f831bc2631f08dfe9
        #endregion

        #region constructor
        public GameState()
        {

        }
        #endregion

        #region methods
        /// <summary>
        /// GameStates should override this to provide all update logic
        /// </summary>
        /// <param name="gameTime">gameTime.ElapsedGameTime contains the amount of time elapsed since last update</param>
        /// <param name="input">Check this object for any relevant inputs. It is updated by Game1 every cycle before passing in</param>
        public abstract void Update(GameTime gameTime, InputManager input);
        /// <summary>
        /// GameStates should override this to provide all drawing logic
        /// </summary>
        /// <param name="sb">pass this to the GameStates Views to draw objects to screen</param>
        public abstract void Draw(SpriteBatch sb);
        #endregion
    }
}
