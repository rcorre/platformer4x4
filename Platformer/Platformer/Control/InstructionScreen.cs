using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Platformer.View;

namespace Platformer.Control
{
    class InstructionScreen : GameState
    {
        #region static
        static Texture2D backgroundTexture;
        public static void LoadTextures(Texture2D background)
        {
            backgroundTexture = background;
        }
        #endregion

        #region fields
        KeyboardState oldState, newState;
        GraphicsDevice graphics;
        Texture2D sprite;
        SpriteFont Font1;
        #endregion

        #region properties
        #endregion

        #region constructor
        public InstructionScreen(GraphicsDevice g, SpriteFont f, Texture2D s)
        {
            SoundPlayer.StopSound();

            SoundPlayer.StartSound("pathetique");
            oldState = Keyboard.GetState();
            graphics = g;
            sprite = s;
            Font1 = f;
        }
        #endregion

        #region methods
        public override void Update(GameTime gameTime, InputManager input)
        {
            //return to main menu if any key is pressed
            newState = Keyboard.GetState();
            if (newState != oldState)
            {
                NewState = new MainMenu(graphics, Font1, sprite);
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(backgroundTexture, Vector2.Zero, Color.White);
        }
        #endregion
    }
}
