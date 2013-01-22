using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    abstract class GameState
    {
        public bool RequestExit;    //request to close game
        public bool RequestPop;     //request to pop state off stack and resume previous state
        public GameState RequestPushState;      //request to push a new state on to the stack
        public abstract void Update(GameTime gameTime, InputManager input);
        public abstract void Draw(SpriteBatch sb);
    }
}
