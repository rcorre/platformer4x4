using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Diagnostics;

using Platformer.Control;
using Platformer.View;
using Platformer.Model;
using Platformer.Data;

namespace Platformer
{
    public enum Direction
    {
        North,      //up
        East,       //right
        South,      //down
        West        //left
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;
        public const int NUM_LEVELS = 1;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //current game state. call Update() and Draw() on it every frame
        GameState _currentState;
        //update and pass to current State every Update()
        InputManager _input;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _input = new InputManager();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //initialize helper pixel texture for debugging
            XnaHelper.PixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            XnaHelper.PixelTexture.SetData<Color>(new Color[] {Color.White});
            XnaHelper.Font = Content.Load<SpriteFont>("Fonts/standard_font");
            SpriteFont Font1 = Content.Load<SpriteFont>("Fonts/TitleFont");
            Texture2D sprite = Content.Load<Texture2D>("spritesheets/Gino");

            //needed for level to load and draw maps
            Level.MapDisplayDevice = new xTile.Display.XnaDisplayDevice(this.Content, this.GraphicsDevice);
            //give Level ref to Content so they can load data
            Level.Content = this.Content;

            //initialize unit data
            Unit.UnitDataDict = DataLoader.LoadUnitData();
            //initialize sprite data
            Sprite.SpriteDataDict = DataLoader.LoadSpriteData();
            //initialize weapon data
            Weapon.Data = DataLoader.LoadWeaponData();
            Overworld.Nodes = DataLoader.LoadOverworldData();
            OverworldView.LoadTextures(Content.Load<Texture2D>("Icons/Node"), Content.Load<Texture2D>("Backgrounds/skyline"));
            SpriteView.LoadTextures(Sprite.SpriteDataDict.Keys.ToArray<string>(), Content);

            //Later: change this in main menu
            //initialize new data if select new game
            //load data if select continue game

            _currentState = new MainMenu(GraphicsDevice, Font1, sprite);
//            _currentState = new Overworld(
//                new ProgressData()
//                {
//                    NumCoins = 0,
//                    CurrentLevel = 0,
//                    LevelCompleted = new bool[Overworld.Nodes.Length]
//                }
 //               );                            ***************************moved to MainMenu
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //make sure to update inputmanager, otherwise player input will not be detected
            _input.Update();

            // Allows the game to exit
            if (_currentState.RequestExit)
                this.Exit();

            if (_currentState.NewState != null)     //new state requested
                _currentState = _currentState.NewState;

            _currentState.Update(gameTime, _input);

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            _currentState.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
