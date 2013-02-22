using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Platformer.View;
using Platformer.Model;

using xTile;
using xTile.Layers;
using xTile.Tiles;

namespace Platformer.Control
{
    class Level : GameState
    {
        #region static
        /// <summary>
        /// reference to game display device from Game1 - needed to draw xTile maps
        /// </summary>
        public static xTile.Display.XnaDisplayDevice MapDisplayDevice;
        /// <summary>
        /// reference to ContentManager from Game1 - needed to load maps
        /// </summary>
        public static ContentManager Content;
        #endregion

        #region fields
        Map _tileMap;   //xTile map
        xTile.Dimensions.Rectangle _viewport;   //camera
        Layer _collisionLayer;   //layer of xTile map on which to detect collisions 
        Tile _hitDetectTile;
        xTile.Dimensions.Location _tileLocation;
        Vector2 centerPos = Vector2.Zero;
        Unit _gino = new Gino(Vector2.Zero, true);
        #endregion

        #region properties
        #endregion

        #region constructor
        public Level(int levelNumber)
        {
            //set camera size based on screen size
            _viewport = new xTile.Dimensions.Rectangle(
                new xTile.Dimensions.Size(
                    Game1.SCREEN_WIDTH, Game1.SCREEN_HEIGHT));

            //load the map for the specified level
            //_tileMap = Content.Load<Map>("Maps\\" + levelNumber);
            _tileMap = Content.Load<Map>("Maps\\TestLevel2");

            //load tile sheet
            _tileMap.LoadTileSheets(MapDisplayDevice);

            _collisionLayer = _tileMap.Layers[0];

        }
        #endregion

        #region methods
        public override void Update(GameTime gameTime, InputManager input)
        {
            handleInput(input);
            centerCamera(_gino.Center);
            _gino.Update(gameTime);
            moveUnit(_gino, gameTime);
        }

        private void handleInput(InputManager input)
        {
            if (input.MoveLeft)
                _gino.WalkLeft();
            else if (input.MoveRight)
                _gino.WalkRight();
            if (input.Jump)
                _gino.Jump();
        }

        /// <summary>
        /// Move a unit based on its velocity and the surrounding tiles
        /// </summary>
        /// <param name="unit">the unit to move</param>
        private void moveUnit(Unit unit, GameTime gameTime)
        {
            int pxRight = (int)(unit.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
            int pxDown = (int)(unit.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
            unit.Left += pxRight;
            unit.Top += pxDown;
            //enter unit moving logic here
            //I suggest breaking down unit.Velocity into individual x and y components, cast into integers
        }

        /// <summary>
        /// Center viewport on specified condition.
        /// Should be called with player unit center every update
        /// </summary>
        /// <param name="centerPosition">Location to center view on</param>
        private void centerCamera(Vector2 centerPosition)
        {
            _viewport.X = (int)MathHelper.Clamp(
                (centerPosition.X - Game1.SCREEN_WIDTH / 2.0f),
                0, _tileMap.DisplayWidth - _viewport.Width);
            _viewport.Y = (int)MathHelper.Clamp(
                (centerPosition.Y - Game1.SCREEN_HEIGHT / 2.0f),
                0, _tileMap.DisplayHeight - _viewport.Height);
        }

        public override void Draw(SpriteBatch sb)
        {
            _tileMap.Draw(MapDisplayDevice, _viewport);
            SpriteView.DrawSprite(sb, _gino, _viewport.X, _viewport.Y);
        }
        #endregion
    }
}
