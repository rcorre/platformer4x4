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
    class MainMenu : GameState
    {
        #region static
        const int numButtons = 3, buttonHeight = 58, buttonWidth = 150, buttonPadding = (Game1.SCREEN_HEIGHT / 10) * 2;
        enum buttonState { up, down };
        string[] buttonTxt = { "Start Game", "Instructions", "Quit" };
        const int numBullets = 21, bulletHeight = 5, bulletWidth = 10;
        Rectangle crop = new Rectangle(0, 0, 64, 96);
        #endregion

        #region fields
        SpriteFont Font1;
        MouseState previousMouseState;
        Vector2 image = new Vector2(0, 0);
        Vector2 title = new Vector2(0, 0);
        Vector2[] FontPos = new Vector2[numButtons];

        Vector2[] ButtonPos = new Vector2[numButtons];
        Color[] buttonColor = new Color[numButtons];
        Rectangle[] buttonRect = new Rectangle[numButtons];
        buttonState[] buttonSt = new buttonState[numButtons];
        Texture2D[] buttonTexture = new Texture2D[numButtons];
        int buttonXpos = (Game1.SCREEN_WIDTH / 2) - (buttonWidth / 2);
        // buttonYpos might need reworking, its accounting for only 2 buttons
        int buttonYpos = (Game1.SCREEN_HEIGHT / 2) - (Game1.SCREEN_HEIGHT / 4);

        Vector2 stringLength;
        GraphicsDevice graphics;
        Texture2D sprite;

        bool[] bulletFired = new bool[numBullets];
        Color[] bulletColor = new Color[numBullets];
        Rectangle[] bulletRect = new Rectangle[numBullets];
        Texture2D[] bulletTexture = new Texture2D[numBullets];

        #endregion

        #region properties
        #endregion

        #region constructor
        public MainMenu(GraphicsDevice g, SpriteFont f, Texture2D s)
        {
            SoundPlayer.Initialize();
            SoundPlayer.StartSound("rosesdepicardie");
            graphics = g;
            Font1 = f;
            sprite = s;
            for (int i = 0; i < numButtons; i++)
            {
                buttonSt[i] = buttonState.up;
                buttonColor[i] = Color.Black;
                buttonRect[i] = new Rectangle(buttonXpos, buttonYpos, buttonWidth, buttonHeight);
                stringLength = XnaHelper.Font.MeasureString(buttonTxt[i]);
                FontPos[i].X = buttonXpos + (buttonWidth / 2 - stringLength.X / 2);
                FontPos[i].Y = buttonYpos + (buttonHeight / 2 - stringLength.Y / 2);
                ButtonPos[i].X = buttonXpos;
                ButtonPos[i].Y = buttonYpos;
                buttonYpos += buttonPadding;
                buttonTexture[i] = XnaHelper.PixelTexture;
            }
            for (int i = 0; i < numBullets; i++)
            {
                bulletFired[i] = false;
                bulletRect[i].X = 0;
                bulletRect[i].Y = 0;
            }
        }
        #endregion

        #region methods
        private bool inButton(int i)
        {
            for (int j = 0; j < numBullets; j++)
            {
                if ((bulletRect[j].X >= ButtonPos[i].X) && (bulletRect[j].X <= (ButtonPos[i].X + buttonWidth))
                    && (bulletRect[j].Y >= ButtonPos[i].Y) && (bulletRect[j].Y <= (ButtonPos[i].Y + buttonHeight)))
                {
                    bulletFired[j] = false;
                    return true;
                }
            }
            return false;
        }

        private void fireBullet(Vector2 image)
        {
            SoundPlayer.playSoundEffects("jumpsnare");
            bool found = false;
            int i = 0;
            while (!found)
            {
                if (i >= numBullets)
                    return;
                else if (!bulletFired[i])
                    found = true;
                else
                    i++;
            }
            for (int j = 0; j < 3; j++)
            {
                bulletColor[i + j] = Color.Gold;
                bulletRect[i + j] = new Rectangle(((int)image.X + j * (bulletWidth + 20)), (int)image.Y + 40, bulletWidth, bulletHeight);
                bulletTexture[i + j] = XnaHelper.PixelTexture;
                bulletFired[i + j] = true;
            }
        }

        public override void Update(GameTime gameTime, InputManager input)
        {

            MouseState mouseState = Mouse.GetState();
            image.X = (Game1.SCREEN_WIDTH / 8);
            image.Y = mouseState.Y - 20;
            if (previousMouseState.LeftButton == ButtonState.Released
                && mouseState.LeftButton == ButtonState.Pressed)
            {
                fireBullet(image);
                previousMouseState = mouseState;
            }
            if (previousMouseState.LeftButton == ButtonState.Pressed
                && mouseState.LeftButton == ButtonState.Released)
                previousMouseState = mouseState;

            for (int i = 0; i < numButtons; i++)
            {
                if (buttonSt[i] == buttonState.down) //decide where to go
                {
                    GameState _current_state = this;


                    if (buttonSt[0] == buttonState.down)
                    {//******************************start game
                        NewState = new Overworld(
                            new ProgressData()
                            {
                                NumCoins = 0,
                                CurrentLevel = 0,
                                LevelCompleted = new bool[Overworld.Nodes.Length]
                            }
                        );

                    }
                    else if (buttonSt[1] == buttonState.down)
                        NewState = new InstructionScreen(graphics);

                    if (buttonSt[2] == buttonState.down) //quit
                        _current_state.RequestExit = true;
                }
                if (inButton(i))
                    buttonSt[i] = buttonState.down;
            }

            for (int i = 0; i < numBullets; i++)
            {
                if (bulletFired[i])
                {
                    bulletRect[i].X += 10;
                    if (bulletRect[i].X > Game1.SCREEN_WIDTH)
                        bulletFired[i] = false;
                }
            }

        }
        public override void Draw(SpriteBatch sb)
        {
            graphics.Clear(Color.Gray);
            for (int i = 0; i < numButtons; i++)
            {
                if (buttonSt[i] == buttonState.down)
                    buttonColor[i] = Color.Red;
                else
                    buttonColor[i] = Color.Black;
                sb.Draw(buttonTexture[i], buttonRect[i], buttonColor[i]);
                sb.DrawString(XnaHelper.Font, buttonTxt[i], FontPos[i], Color.White);
            }
            for (int i = 0; i < numBullets; i++)
            {
                if (bulletFired[i])
                    sb.Draw(bulletTexture[i], bulletRect[i], bulletColor[i]);
            }
            string t = "Project Mafia";
            title.X = Game1.SCREEN_WIDTH / 2 - (Font1.MeasureString(t).X / 2);
            title.Y = 20;
            sb.DrawString(Font1, t, title, Color.Red);
            sb.Draw(sprite, image, crop, Color.White);
        }
        #endregion
    }
}
