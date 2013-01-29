using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using xTile;
using xTile.Display;
using xTile.Layers;
using xTile.Tiles;

namespace Platformer
{
    class InLevel : GameState
    {
        public static XnaDisplayDevice MapDisplayDevice;

        Map _tileMap;
        xTile.Dimensions.Rectangle _viewport;
        Sprite _sprite;
        Rectangle _hitRect;
        Layer _collisionLayer;
        Tile _hitDetectTile;
        xTile.Dimensions.Location _tileLocation;

        KeyboardState _previousKeyboardState, _currentKeyboardState;

        public InLevel(int levelNumber, ContentManager content)
        {
            //general purpose hit detection rectangle
            _hitRect = new Rectangle(0, 0, 0, 0);

            _sprite = new Sprite(content.Load<Texture2D>("character"), 48, 48, 2, 2, TimeSpan.FromSeconds(0.2), 4);
            _sprite.SetLocation(60, Game1.SCREEN_HEIGHT - 2 * Tile.TILE_WIDTH);

            _tileMap = content.Load<Map>("Maps/level1");
            _tileMap.LoadTileSheets(MapDisplayDevice);
            _viewport = new xTile.Dimensions.Rectangle(
                new xTile.Dimensions.Size(
                    Game1.SCREEN_WIDTH, Game1.SCREEN_HEIGHT));

            _collisionLayer = _tileMap.Layers[0];


        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime, InputManager input)
        {

            _tileMap.Update(gameTime.ElapsedGameTime.Milliseconds);

            // Allows the game to exit
            if (input.KeyDown(Keys.Escape))
                RequestExit = true;

            if (input.KeyDown(Keys.D))
                _viewport.X += 1;
            else if (input.KeyDown(Keys.A))
                _viewport.X -= 1;

            if (input.KeyDown(Keys.S))
                _viewport.Y += 1;
            else if (input.KeyDown(Keys.W))
                _viewport.Y -= 1;

            if (input.KeyDown(Keys.Right))
                _sprite.Velocity.X = 3;
            else if (input.KeyDown(Keys.Left))
                _sprite.Velocity.X = -3;
            else
                _sprite.Velocity.X = 0;

            //jump
            if (input.KeyDown(Keys.Up))
                _sprite.Velocity.Y = -6;

            //apply gravity
            _sprite.Velocity.Y += 9 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            moveSprite(_sprite, _sprite.Velocity);
            _sprite.Update(gameTime);
        }

        //Decompose movement into X and Y axes, step one at a time.
        private void moveSprite(Sprite sprite, Vector2 velocity)
        {
            int pixelsRight = (int)velocity.X;
            int pixelsDown = (int)velocity.Y;

            #region horizontal collision detection
            if (pixelsRight != 0)   //perform horizontal collision detection
            {
                /*Get the coordinate of the forward-facing edge, 
                  e.g.  If walking left, the x coordinate of left of bounding box. 
                  If walking right, x coordinate of right side. If up, y coordinate of top, etc.*/
                int forwardEdge = (pixelsRight > 0) ?
                    sprite.HitRect.Right : sprite.HitRect.Left;

                //At most, the player will come to a stop at the maximum movement distance
                //(assuming no obstacles hit)
                int closestObstacleX = forwardEdge + pixelsRight;

                //get which direction to scan (Left or Right)
                int xDirection = pixelsRight / Math.Abs(pixelsRight);

                int startCol = forwardEdge / Tile.TILE_WIDTH;
                int colsToScan = (pixelsRight + 1)/ Tile.TILE_WIDTH;

                /*Figure which lines of tiles the bounding box intersects with – 
                 * this will give you a minimum and maximum tile value on the OPPOSITE axis. 
                 * For example, if we’re walking left, 
                 * perhaps the player intersects with horizontal rows 32, 33 and 34*/
                for (int row = sprite.HitRect.Top / Tile.TILE_HEIGHT;
                         row <= (sprite.HitRect.Bottom - 1) / Tile.TILE_HEIGHT;
                         row++)
                {
                    for (int col = startCol;
                          Math.Abs(col - startCol) <= colsToScan;
                          col = col + xDirection)
                    {
                        /*Scan along those lines of tiles and towards the direction of movement until 
                         * you find the closest static obstacle. Then loop through every moving obstacle, 
                         * and determine which is the closest obstacle that is actually on your path.*/

                        if (_collisionLayer.Tiles[col,row] != null)
                        {
                            //if moving right, use the left edge of the obstacle. Else, use the right edge
                            int obstacleBound = (pixelsRight > 0) ? col * Tile.TILE_WIDTH : (col + 1) * Tile.TILE_WIDTH;
                            //if this obstacle is closer than any obstacles found so far, set its bound as the new movement limit
                            //use min if moving left, max if moving right
                            closestObstacleX = (pixelsRight > 0) ?
                                Math.Min(closestObstacleX, obstacleBound) :
                                Math.Max(closestObstacleX, obstacleBound);
                        }
                    }
                }


                /*adjust the total movement amount based on an detected obstacles in path
                  if no obstacles were detected, pixelsRight is not changed
                  otherwise, this will adjust the sprite's movement so it comes to a stop at the obstacle*/
                pixelsRight = closestObstacleX - forwardEdge;
            }
            #endregion

            #region vertical collision detection
            if (pixelsDown != 0)   //perform vertical collision detection
            {
                /*Get the coordinate of the forward-facing edge, 
                  e.g.  If falling down, the y coordinate of bottom of bounding box. 
                  If rising up, y coordinate of top side.*/
                int forwardEdge = (pixelsDown > 0) ?
                    sprite.HitRect.Bottom : sprite.HitRect.Top;

                //At most, the player will come to a stop at the maximum movement distance
                //(assuming no obstacles hit)
                int closestObstacleY = forwardEdge + pixelsDown;

                //get which direction to scan (Down or Up)
                int yDirection = pixelsDown / Math.Abs(pixelsDown);

                int startRow = forwardEdge / Tile.TILE_HEIGHT;
                int rowsToScan = (pixelsDown + 1)/ Tile.TILE_HEIGHT;

                /*Figure which lines of tiles the bounding box intersects with – 
                 * this will give you a minimum and maximum tile value on the OPPOSITE axis. 
                 * For example, if we’re walking left, 
                 * perhaps the player intersects with horizontal rows 32, 33 and 34*/
                for (int col = sprite.HitRect.Left / Tile.TILE_WIDTH;
                         col <= (sprite.HitRect.Right - 1) / Tile.TILE_WIDTH;
                         col++)
                {
                    for (int row = startRow;
                          Math.Abs(row - startRow) <= rowsToScan;
                          row = row + yDirection)
                    {
                        /*Scan along those lines of tiles and towards the direction of movement until 
                         * you find the closest static obstacle. Then loop through every moving obstacle, 
                         * and determine which is the closest obstacle that is actually on your path.*/

                        if (_collisionLayer.Tiles[col,row] != null)
                        {
                            //if moving down, use the top edge of the obstacle. Else, use the bottom edge
                            int obstacleBound = (pixelsDown > 0) ? row * Tile.TILE_HEIGHT : (col + 1) * Tile.TILE_HEIGHT;
                            //if this obstacle is closer than any obstacles found so far, set its bound as the new movement limit
                            //use min if moving down, max if moving up
                            closestObstacleY = (pixelsDown > 0) ?
                                Math.Min(closestObstacleY, obstacleBound) :
                                Math.Max(closestObstacleY, obstacleBound);

                            //stop falling if standing on a block
                            if (pixelsDown > 0)
                            {
                                if (closestObstacleY == _sprite.HitRect.Bottom)
                                    _sprite.Velocity.Y = 0;
                            }
                        }
                    }
                }


                /*adjust the total movement amount based on any detected obstacles in path
                  if no obstacles were detected, pixelsDown is not changed
                  otherwise, this will adjust the sprite's movement so it comes to a stop at the obstacle*/
                pixelsDown = closestObstacleY - forwardEdge;
            }
            #endregion


            sprite.Move(pixelsRight, pixelsDown);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(SpriteBatch sb)
        {
            _tileMap.Draw(MapDisplayDevice, _viewport);
            _sprite.Draw(sb, _viewport.X, _viewport.Y);
        }
    }
}
