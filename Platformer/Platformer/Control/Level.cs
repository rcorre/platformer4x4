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
            centerCamera(centerPos);
            _gino.Right = (int)centerPos.X;
            _gino.Bottom = (int)centerPos.Y;
            _gino.Update(gameTime);
        }

        private void handleInput(InputManager input)
        {
            if (input.MoveLeft)
                centerPos.X -= 10;
            else if (input.MoveRight)
            {
                centerPos.X += 10;
            }
            if (input.MoveUp)
                centerPos.Y -= 10;
            if (input.MoveDown)
                centerPos.Y += 10;
        }

        /// <summary>
        /// Move a unit based on its velocity and the surrounding tiles
        /// </summary>
        /// <param name="unit">the unit to move</param>
        private void moveUnit(Unit unit, Vector2 _velocity)
        {
            //enter unit moving logic here
            //I suggest breaking down unit.Velocity into individual x and y components, cast into integers
            int pixelRight = (int)_velocity.X;
            int pixelDown  = (int)_velocity.Y;

            #region horizontal collision detection
            if (pixelRight != 0)
            {
                //Get the coordinate of the forward-facing edge
                //If walking left, forwardEdge = left of bounding box
                //If walking right, forwardEdge = right of bounding box
                int forwardEdge = (pixelRight > 0) ?
                    unit.HitRect.Right : unit.HitRect.Left;
                int closestObstacleX = forwardEdge + pixelRight;

                int xDirection = pixelRight / Math.Abs(pixelRight);

                int startCol = forwardEdge / xTile.Dimensions.Size.Zero.Width;
                int colsToScan = (pixelRight + 1) / xTile.Dimensions.Size.Zero.Width;

                for (int row = unit.HitRect.Top / xTile.Dimensions.Size.Zero.Height;
                        row <= (unit.HitRect.Bottom - 1) / xTile.Dimensions.Size.Zero.Height;
                        row++)
                {
                    for (int col = startCol;
                            Math.Abs(col - startCol) <= colsToScan;
                            col = col + xDirection)
                    {
                        if (_collisionLayer.Tiles[col, row] != null)
                        {
                            int obstacleBound = (pixelRight > 0) ? col * xTile.Dimensions.Size.Zero.Width : (col + 1) * xTile.Dimensions.Size.Zero.Width;

                            closestObstacleX = (pixelRight > 0) ?
                                Math.Min(closestObstacleX, obstacleBound) :
                                Math.Max(closestObstacleX, obstacleBound);
                        }
                    }
                }
                pixelRight = closestObstacleX - forwardEdge;
            }
            #endregion

            #region vertical collision detection
            if (pixelDown != 0)
            {
                int forwardEdge = (pixelDown > 0) ?
                    unit.HitRect.Bottom : unit.HitRect.Top;

                //At most, if no obstacles are hit, the player will come to a stop
                //at the maximum movement distance
                int closestObstacleY = forwardEdge + pixelDown;

                //get which direction to scan(Up or Down)
                int yDirection = pixelDown / Math.Abs(pixelDown);

                int startRow = forwardEdge / unit.HitRect.Height;
                int rowsToScan = (pixelDown + 1) / unit.HitRect.Height;

                for (int col = unit.HitRect.Left / xTile.Dimensions.Size.Zero.Width;
                        col <= (unit.HitRect.Right - 1) / xTile.Dimensions.Size.Zero.Width;
                        col++)
                {
                    for (int row = startRow;
                            Math.Abs(col - startRow) <= rowsToScan;
                            col = col + yDirection)
                    {
                        if (_collisionLayer.Tiles[col, row] != null)
                        {
                            int obstacleBound = (pixelDown > 0) ? col * xTile.Dimensions.Size.Zero.Height : (col + 1) * xTile.Dimensions.Size.Zero.Height;

                            closestObstacleY = (pixelDown > 0) ?
                                Math.Min(closestObstacleY, obstacleBound) :
                                Math.Max(closestObstacleY, obstacleBound);
                        }
                        if (pixelDown > 0)
                        {
                            if (closestObstacleY == unit.HitRect.Bottom)
                                _velocity.Y = 0;
                        }
                    }
                }
                pixelDown = closestObstacleY - forwardEdge;
            }
            #endregion

            //move the unit the amount specified above
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
