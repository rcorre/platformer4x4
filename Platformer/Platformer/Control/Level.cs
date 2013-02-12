using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            _tileMap = Content.Load<Map>("Maps\\" + levelNumber);

            //load tile sheet
            _tileMap.LoadTileSheets(MapDisplayDevice);

            _collisionLayer = _tileMap.Layers[0];

        }
        #endregion

        #region methods
        public override void Update(GameTime gameTime, InputManager input)
        {
            centerCamera(centerPos);
        }

        private void handleInput(InputManager input)
        {
            if (input.MoveLeft)
                centerPos.X -= 1;
            else if (input.MoveRight)
                centerPos.X += 1;
            if (input.MoveUp)
                centerPos.Y -= 1;
            if (input.MoveDown)
                centerPos.Y += 1;
        }

        private void centerCamera(Vector2 centerPosition)
        {
            _viewport.X = (int)MathHelper.Clamp(
                (centerPosition.X - Game1.SCREEN_WIDTH / 2.0f),
                0, 1000);
            _viewport.Y = (int)MathHelper.Clamp(
                (centerPosition.Y - Game1.SCREEN_HEIGHT / 2.0f),
                0, 1000);
        }


        public override void Draw(SpriteBatch sb)
        {
            _tileMap.Draw(MapDisplayDevice, _viewport);
        }
        #endregion
    }
}
