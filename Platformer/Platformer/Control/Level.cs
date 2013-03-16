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
        Layer _pickupLayer;
        Vector2 centerPos = Vector2.Zero;
        Unit _gino = new Gino(new Vector2(200,100), true);
        ProgressData _progressData;
        List<Pickup> _pickups;
        Weapon _currentWeapon;
        #endregion

        #region properties
        #endregion

        #region constructor
        public Level(int levelNumber, ProgressData progressData)
        {
            //set camera size based on screen size
            _viewport = new xTile.Dimensions.Rectangle(
                new xTile.Dimensions.Size(
                    Game1.SCREEN_WIDTH, Game1.SCREEN_HEIGHT));

            //load the map for the specified level
            _tileMap = Content.Load<Map>("Maps\\" + levelNumber.ToString());

            //load tile sheet
            _tileMap.LoadTileSheets(MapDisplayDevice);

            scanMapLayers();

            _progressData = progressData;

            Weapon.Initialize();

            _currentWeapon = new Weapon("Rifle", _gino);

        }
        #endregion

        #region methods
        private void scanMapLayers()
        {
            _collisionLayer = _tileMap.GetLayer("Collision");
            _pickupLayer = _tileMap.GetLayer("Pickups");
            Tile tile;

            _pickups = new List<Pickup>();
            for (int row = 0; row < _pickupLayer.LayerHeight; row++)
            {
                for (int col = 0; col < _pickupLayer.LayerWidth; col++)
                {
                    tile = _pickupLayer.Tiles[col, row];
                    if (tile != null)
                    {
                        _pickups.Add(new Pickup(row, col,
                        new Vector2(col * _pickupLayer.TileWidth, row * _pickupLayer.TileHeight),
                        tile.TileIndexProperties["PickupType"]));
                    }
                }
            }
            //make marker layers invisible
            _pickupLayer.Visible = false;
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            handleInput(input);
            foreach (Pickup p in _pickups)
                p.Update(gameTime);
            _currentWeapon.Update(gameTime);
            Weapon.UpdateProjectiles(gameTime);
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
                _gino.Jump();
            if (input.Fire)
                _currentWeapon.Fire(_gino.Center, _gino.Sprite.FacingRight ? Vector2.UnitX : -Vector2.UnitX);
            if (input.Debug1)
                Platformer.Data.DataLoader.SaveProgress(_progressData);
            if (input.Debug2)
                _progressData = Platformer.Data.DataLoader.LoadProgress();
        }

        /// <summary>
        /// Move a unit based on its velocity and the surrounding tiles
        /// </summary>
        /// <param name="unit">the unit to move</param>
        private void moveUnit(Unit unit, GameTime gameTime)
        {
            int pxRight = (int)(unit.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
            int pxDown = (int)(unit.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (pxRight != 0)
                checkHorizontalCollision(unit, pxRight);
            if (pxDown != 0)
                checkVerticalCollision(unit, pxDown);
        }

        private void getPickup(string name, int row, int col)
        {
            _pickups.RemoveAll(t => t.Row == row && t.Col == col);
            switch (name)
            {
                case "Coin":
                    _progressData.NumCoins += 1;
                    break;
            }
        }


        private void checkHorizontalCollision(Unit unit, int pxRight)
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

            for (int col = startCol; col != (endCol + xDirection); col = col + xDirection)
            {
                for (int row = unit.Top / _collisionLayer.TileHeight;
                    row <= (unit.Bottom - 1) / _collisionLayer.TileHeight;
                    row++)
                {
                    //pickup checks
                    if (_pickupLayer.Tiles[col, row] != null)
                    {
                        getPickup(_pickupLayer.Tiles[col, row].TileIndexProperties["PickupType"], row, col);
                        _pickupLayer.Tiles[col, row] = null;
                    }

                    //boundary checks----------------------------------------------------------------
                    if (col < 0)
                    {
                        unit.CollideWithObstacle(Direction.West);
                        unit.Left = 0;
                    }
                    else if (col > _collisionLayer.LayerWidth)
                    {
                        unit.CollideWithObstacle(Direction.East);
                        unit.Right = _collisionLayer.LayerWidth * _collisionLayer.TileWidth;
                    }
                    //--------------------------------------------------------------------------------
                    //within bounds -- check tile collision
                    else if (_collisionLayer.Tiles[col, row] != null && _collisionLayer.Tiles[col, row].TileIndex != 0)
                    {
                        if (pxRight > 0)
                        {
                            unit.CollideWithObstacle(Direction.East);
                            unit.Right = col * _collisionLayer.TileWidth;
                        }
                        else
                        {
                            unit.CollideWithObstacle(Direction.West);
                            unit.Left = (col + 1) * _collisionLayer.TileWidth;
                        }
                        break;  //no need to check more
                    }
                }
            }
        }

        private void checkVerticalCollision(Unit unit, int pxDown)
        {
            int forwardEdge = (pxDown > 0) ? unit.Bottom : unit.Top;

            //1 for down, -1 for up
            int yDirection = pxDown / Math.Abs(pxDown);

            //start at initial forward edge
            int startRow = forwardEdge / _collisionLayer.TileHeight;
            //assume unit moves full distance
            unit.Y += pxDown;
            //check up to new assumed unit hitrect edge
            int endRow = (forwardEdge + pxDown) / _collisionLayer.TileHeight;

            for (int row = startRow; row != (endRow + yDirection); row = row + yDirection)
            {
                for (int col = (unit.Left + 1) / _collisionLayer.TileWidth;
                    col <= (unit.Right - 1) / _collisionLayer.TileWidth;
                    col++)
                {
                    //pickup checks
                    if (_pickupLayer.Tiles[col, row] != null)
                    {
                        getPickup(_pickupLayer.Tiles[col, row].TileIndexProperties["PickupType"], row, col);
                        _pickupLayer.Tiles[col, row] = null;
                    }
                    //boundary checks----------------------------------------------------------------
                    if (col < 0)
                    {
                        unit.CollideWithObstacle(Direction.West);
                        unit.Left = 0;
                    }
                    else if (col > _collisionLayer.LayerWidth)
                    {
                        unit.CollideWithObstacle(Direction.East);
                        unit.Right = _collisionLayer.LayerWidth * _collisionLayer.TileWidth;
                    }
                    if (row < 0)
                    {
                        unit.CollideWithObstacle(Direction.North);
                        unit.Top = 0;
                    }
                    else if (row > _collisionLayer.LayerHeight)
                    {
                        unit.CollideWithObstacle(Direction.South);
                        unit.Bottom = _collisionLayer.LayerHeight * _collisionLayer.TileHeight;
                    }
                    //--------------------------------------------------------------------------------

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
                            unit.Top = (row + 1) * _collisionLayer.TileHeight;
                        }
                        break;  //no need to check more
                    }
                }
            }
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
            foreach (Pickup p in _pickups)
            {
                SpriteView.DrawPickup(sb, p, _viewport.X, _viewport.Y);
            }

            SpriteView.DrawUnit(sb, _gino, _viewport.X, _viewport.Y);

            foreach (Projectile p in Weapon.Projectiles)
                SpriteView.DrawProjectile(sb, p, _viewport.X, _viewport.Y);

            //debug
            XnaHelper.DisplayValue(sb, "Velocity", _gino.Velocity.X.ToString(),
                new Rectangle(500, 100, 100, 20), Color.Black);
            XnaHelper.DisplayValue(sb, "Coins", _progressData.NumCoins.ToString(),
                new Rectangle(300, 100, 100, 20), Color.Black);
        }
        #endregion
    }
}
