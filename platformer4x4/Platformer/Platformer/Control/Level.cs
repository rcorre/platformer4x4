﻿using System;
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
        Vector2 centerPos = Vector2.Zero;
        Unit _gino = new Gino(new Vector2(200,100), true);
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
            _gino.Update(gameTime, onGround(_gino));
            moveUnit(_gino, gameTime);
        }

        private void handleInput(InputManager input)
        {
            if (input.MoveLeft)
                _gino.Walk(Direction.West);
            else if (input.MoveRight)
                _gino.Walk(Direction.East);
            if (input.Jump)
            {
                if (onGround(_gino))//so you can only hear the jump sound when you are on the ground
                {
                    SoundPlayer.playSoundEffect1();
                }
                _gino.Jump();
          
            }
        }

        /// <summary>
        /// Move a unit based on its velocity and the surrounding tiles
        /// </summary>
        /// <param name="unit">the unit to move</param>
        private void moveUnit(Unit unit, GameTime gameTime)
        {
            int pxRight = (int)(unit.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
            int pxDown = (int)(unit.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);

            #region horizontal collision detection
            if (pxRight != 0)
            {
                //Get the coordinate of the forward-facing edge
                //If walking left, forwardEdge = left of bounding box
                //If walking right, forwardEdge = right of bounding box
                int forwardEdge = (pxRight > 0) ? unit.Right : unit.Left;

                //-1 for left, 1 for right
                int xDirection = pxRight / Math.Abs(pxRight);

                int startCol = forwardEdge / _collisionLayer.TileWidth;
                int endCol = (forwardEdge + pxRight) / _collisionLayer.TileWidth;

                unit.X += pxRight;

                for (int row = unit.Top / _collisionLayer.TileHeight;
                        row <= (unit.Bottom - 1) / _collisionLayer.TileHeight;
                        row++)
                {
                    for (int col = startCol; col != endCol; col = col + xDirection)
                    {
                        if (_collisionLayer.Tiles[col, row] != null && _collisionLayer.Tiles[col, row].TileIndex != 0)
                        {
                            if (pxRight > 0)
                            {
                                unit.CollideWithObstacle(Direction.East);
                                unit.Right = col * _collisionLayer.TileWidth;
                            }
                            else
                            {
                                unit.CollideWithObstacle(Direction.West);
                                unit.Right = (col + 1) * _collisionLayer.TileWidth;
                            }
                            break;  //no need to check more
                        }
                    }
                }
            }
            #endregion

            #region vertical collision detection
            if (pxDown != 0)
            {
                
                int forwardEdge = (pxDown > 0) ? unit.Bottom : unit.Top;

                //-1 for down, 1 for up
                int yDirection = pxDown / Math.Abs(pxDown);

                //start at initial forward edge
                int startRow = forwardEdge / _collisionLayer.TileHeight;
                //assume unit moves full distance
                unit.Y += pxDown;
                //check up to new assumed unit hitrect edge
                int endRow = (forwardEdge + pxDown) / _collisionLayer.TileHeight;

                for (int col = unit.Left / _collisionLayer.TileWidth;
                        col <= (unit.Right - 1) / _collisionLayer.TileWidth;
                        col++)
                {
                    for (int row = startRow; row != endRow; row = row + yDirection)
                    {
                        if (_collisionLayer.Tiles[col, row] != null && _collisionLayer.Tiles[col, row].TileIndex != 0)
                        {
                            if (pxDown > 0)
                            {
                                unit.CollideWithObstacle(Direction.South);
                                unit.Bottom = row * _collisionLayer.TileHeight;
                            }
                            else
                            {
                                unit.CollideWithObstacle(Direction.North);
                                unit.Right = (row + 1) * _collisionLayer.TileHeight;
                            }
                            break;  //no need to check more
                        }
                    }
                }
            }
            #endregion
            
        }

        private bool onGround(Unit unit)
        {
            
            int rowBelow = (unit.Bottom + 1) / _collisionLayer.TileHeight;
            for (int col = unit.Left / _collisionLayer.TileWidth;
                    col <= (unit.Right - 1) / _collisionLayer.TileWidth;
                    col++)
            {
                if (_collisionLayer.Tiles[col, rowBelow] != null && _collisionLayer.Tiles[col, rowBelow].TileIndex != 0)
                {
                    return true;    //standing on a solid tile
                }
            }
            return false;       //no solid tiles right beneath unit
        }
        /*
        /// <summary>
        /// Move a unit based on its velocity and the surrounding tiles
        /// </summary>
        /// <param name="unit">the unit to move</param>
        private void moveUnit(Unit unit, GameTime gameTime)
        {
            int pxRight = (int)(unit.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
            int pxDown = (int)(unit.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);

            #region horizontal collision detection
            if (pxRight != 0)
            {
                //Get the coordinate of the forward-facing edge
                //If walking left, forwardEdge = left of bounding box
                //If walking right, forwardEdge = right of bounding box
                int forwardEdge = (pxRight > 0) ? unit.Right : unit.Left;
                    
                //start assuming no obstacle hit (move full distance)
                int closestObstacleX = forwardEdge + pxRight;

                //-1 for left, 1 for right
                int xDirection = pxRight / Math.Abs(pxRight);

                int startCol = forwardEdge / _collisionLayer.TileWidth;
                int colsToScan = (pxRight + 1) / _collisionLayer.TileWidth;

                for (int row = unit.Top / _collisionLayer.TileHeight;
                        row <= (unit.Bottom - 1) / _collisionLayer.TileHeight;
                        row++)
                {
                    for (int col = startCol;
                            Math.Abs(col - startCol) <= colsToScan;
                            col = col + xDirection)
                    {
                        if (_collisionLayer.Tiles[col, row] != null && _collisionLayer.Tiles[col,row].TileIndex != 0)
                        {
                            int obstacleBound = (pxRight > 0) ? col * _collisionLayer.TileWidth : (col + 1) * _collisionLayer.TileWidth;

                            closestObstacleX = (pxRight > 0) ?
                                Math.Min(closestObstacleX, obstacleBound) :
                                Math.Max(closestObstacleX, obstacleBound);
                        }
                    }
                }
                if (pxRight > 0)
                    unit.Right = closestObstacleX;
                else
                    unit.Left = closestObstacleX;
            }
            #endregion

            #region vertical collision detection
            if (pxDown != 0)
            {
                int forwardEdge = (pxDown > 0) ?
                    unit.Bottom : unit.Top;

                //At most, if no obstacles are hit, the player will come to a stop
                //at the maximum movement distance
                int closestObstacleY = forwardEdge + pxDown;

                //get which direction to scan(Up or Down)
                int yDirection = pxDown / Math.Abs(pxDown);

                int startRow = forwardEdge / _collisionLayer.TileHeight;
                int rowsToScan = (pxDown + 1) / _collisionLayer.TileHeight;

                for (int col = unit.Left / _collisionLayer.TileWidth;
                        col <= (unit.Right - 1) / _collisionLayer.TileWidth;
                        col++)
                {
                    for (int row = startRow;
                            Math.Abs(col - startRow) <= rowsToScan;
                            col = col + yDirection)
                    {
                        if (_collisionLayer.Tiles[col, row] != null && _collisionLayer.Tiles[col,row].TileIndex != 0)
                        {
                            unit.CollideWithObstacle(pxDown > 0 ? Direction.South : Direction.North);

                            int obstacleBound = (pxDown > 0) ? col * _collisionLayer.TileHeight : (col + 1) * _collisionLayer.TileHeight;

                            closestObstacleY = (pxDown > 0) ?
                                Math.Min(closestObstacleY, obstacleBound) :
                                Math.Max(closestObstacleY, obstacleBound);
                        }
                    }
                }
                if (pxDown > 0)
                    unit.Bottom = closestObstacleY;
                else
                    unit.Top = closestObstacleY;
            }
            #endregion

            //move the unit the amount specified above

        }
        */

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
            XnaHelper.DisplayValue(sb, "Velocity", _gino.Velocity.X.ToString(), 
                new Rectangle(500, 100, 100, 20), Color.Black);
        }
        #endregion
    }
}
