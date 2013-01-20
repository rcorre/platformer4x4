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

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int SCREEN_WIDTH = 1280;
        const int SCREEN_HEIGHT = 720;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TileMap _tileMap;

        Sprite _sprite;
        Rectangle _hitRect;

        KeyboardState _previousKeyboardState, _currentKeyboardState;

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
            //general purpose hit detection rectangle
            _hitRect = new Rectangle(0, 0, 0, 0);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Tile.LoadTileTextures(Content.Load<Texture2D>("tiles"), 2, 2);

            _tileMap = new TileMap(Content, 
                new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT));

            _sprite = new Sprite(Content.Load<Texture2D>("character"), 48, 48, 2, 2, TimeSpan.FromSeconds(0.2), 4);
            _sprite.SetLocation(20, SCREEN_HEIGHT - 2 * Tile.TILE_WIDTH);

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
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (_currentKeyboardState.IsKeyDown(Keys.D))
                _tileMap.OffsetX += 1;
            else if (_currentKeyboardState.IsKeyDown(Keys.A))
                _tileMap.OffsetX -= 1;

            if (_currentKeyboardState.IsKeyDown(Keys.S))
                _tileMap.OffsetY += 1;
            else if (_currentKeyboardState.IsKeyDown(Keys.W))
                _tileMap.OffsetY -= 1;

            if (_currentKeyboardState.IsKeyDown(Keys.Right))
                moveSprite(_sprite, _sprite.MoveSpeed, 0);
            else if (_currentKeyboardState.IsKeyDown(Keys.Left))
                moveSprite(_sprite, -_sprite.MoveSpeed, 0);

            _sprite.Update(gameTime);

            base.Update(gameTime);
        }

        //Decompose movement into X and Y axes, step one at a time.
        private void moveSprite(Sprite sprite, int pixelsRight, int pixelsDown)
        {
            /*Get the coordinate of the forward-facing edge, 
              e.g.  If walking left, the x coordinate of left of bounding box. 
              If walking right, x coordinate of right side. If up, y coordinate of top, etc.*/

            int forwardEdge = (pixelsRight > 0) ?
                sprite.HitRect.Right : sprite.HitRect.Left;
            /*Figure which lines of tiles the bounding box intersects with – 
             * this will give you a minimum and maximum tile value on the OPPOSITE axis. 
             * For example, if we’re walking left, 
             * perhaps the player intersects with horizontal rows 32, 33 and 34*/

            /*Scan along those lines of tiles and towards the direction of movement until 
             * you find the closest static obstacle. Then loop through every moving obstacle, 
             * and determine which is the closest obstacle that is actually on your path.*/

            //At most, the player will come to a stop at the maximum movement distance
            //(assuming no obstacles hit)
            int closestObstacleX = forwardEdge + pixelsRight;
            
            for (int row = sprite.HitRect.Top / Tile.TILE_HEIGHT;
                     row <= (sprite.HitRect.Bottom  - 1)/ Tile.TILE_HEIGHT;
                     row++)
            {
                for (int col = forwardEdge / Tile.TILE_WIDTH;
                      col <= (forwardEdge + pixelsRight) / Tile.TILE_WIDTH;
                      col++)
                {
                    if (!_tileMap.TilePassable(row, col))
                        closestObstacleX = Math.Min(closestObstacleX, col * Tile.TILE_WIDTH);
                }
            }

            pixelsRight -= (forwardEdge + pixelsRight) - closestObstacleX;


            sprite.Move(pixelsRight, pixelsDown);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            _tileMap.Draw(spriteBatch);
            _sprite.Draw(spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
