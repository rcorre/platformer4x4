using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Platformer.View;

namespace Platformer.Control
{
    class InstructionScreen : GameState
    {
       // SpriteFont Font1;
        GraphicsDevice graphics;
        GameState _current_state;
        static Texture2D instructionTexture;
        public InstructionScreen(GraphicsDevice g)
        {
            SoundPlayer.StopSound();
            SoundPlayer.StartSound("pathetique");
            graphics = g;
           
        }

        public override void Update(GameTime gameTime, InputManager input)
        {

           _current_state = this;
            if (input.ConfirmSelection)
            {
                //_current_state.RequestExit=true;
                _current_state.NewState = new Overworld(
                            new ProgressData()
                           {
                                NumCoins = 0,
                               CurrentLevel = 0,
                                LevelCompleted = new bool[5]
                            }
                        );
            }

        }
        public static void LoadTextures(Texture2D background)
        {

            instructionTexture = background;
            
        }
        public override void Draw(SpriteBatch sb)
        {
           // Don't know how to load the background
         //   graphics.Clear(Color.Gray);
  
            sb.Draw(instructionTexture, Vector2.Zero, Color.White);
          
        }
    }
}
