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
        #region constant
        //how long to wait after losing level
        const float TIME_AFTER_LOSS = 1.0f;
        #endregion

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
        Layer _pickupLayer;      // mark pickups
        Layer _markerLayer;     //mark start, end, and shop locations
        Layer _enemyLayer;     //mark enemy spawns and boundaries
        Vector2 centerPos = Vector2.Zero;
        Gino _gino = new Gino(Vector2.Zero, true);
        ProgressData _progressData;
        List<Pickup> _pickups;
        List<Enemy> _enemies;
        Point _endPoint;
        TimeSpan _endTimer;
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
            _tileMap = Content.Load<Map>("Maps\\Level" + levelNumber.ToString());
            //load tile sheet
            _tileMap.LoadTileSheets(MapDisplayDevice);

            scanMapLayers();
            //_instructionScreen = Content.Load
            _progressData = progressData;
            _progressData.CurrentLevel = levelNumber;

            Weapon.Initialize();
            _gino.addHealth(progressData.addHealth);
            _gino.SetWeapon(new Weapon(progressData.shopWeapon, _gino));
            _gino.addAmmo(progressData.addAmmo);

            //_gino.SetWeapon(new Weapon("Revolver", _gino));

            _endTimer = TimeSpan.FromSeconds(TIME_AFTER_LOSS);

        }
        #endregion

        #region methods
        private void scanMapLayers()
        {
            _collisionLayer = _tileMap.GetLayer("Collision");
            _pickupLayer = _tileMap.GetLayer("Pickups");
            _markerLayer = _tileMap.GetLayer("LevelMarkers");
            _enemyLayer = _tileMap.GetLayer("Enemies");
            Tile tile;

            _pickups = new List<Pickup>();
            for (int row = 0; row < _pickupLayer.LayerHeight; row++)
            {
                for (int col = 0; col < _pickupLayer.LayerWidth; col++)
                {
                    tile = _pickupLayer.Tiles[col, row];
                    if (tile != null && tile.TileIndexProperties.ContainsKey("PickupType"))
                    {
                        _pickups.Add(new Pickup(row, col,
                        new Vector2(col * _pickupLayer.TileWidth, row * _pickupLayer.TileHeight),
                        tile.TileIndexProperties["PickupType"]));
                    }
                }
            }

            for (int row = 0; row < _markerLayer.LayerHeight; row++)
            {
                for (int col = 0; col < _markerLayer.LayerWidth; col++)
                {
                    tile = _markerLayer.Tiles[col, row];
                    if (tile != null)
                    {
                        if (tile.TileIndexProperties.ContainsKey("MarkerType"))
                        {
                            switch (tile.TileIndexProperties["MarkerType"].ToString())
                            {
                                case "Start":
                                    _gino.Bottom = (row + 1) * _markerLayer.TileHeight;
                                    _gino.Left = col * _markerLayer.TileWidth;
                                    break;
                                case "End":
                                    _endPoint.X = (int)((col + 0.5f) * _markerLayer.TileWidth);
                                    _endPoint.Y = (int)((row + 0.5f) * _markerLayer.TileHeight);
                                    break;
                            }
                        }
                    }
                }
            }

            _enemies = new List<Enemy>();
            for (int row = 0; row < _enemyLayer.LayerHeight; row++)
            {
                for (int col = 0; col < _enemyLayer.LayerWidth; col++)
                {
                    tile = _enemyLayer.Tiles[col, row];
                    if (tile != null && tile.TileIndexProperties.ContainsKey("Name") &&
                        tile.TileIndexProperties["Name"].ToString() != "Bound")
                    {
                        
                        string enemyName = tile.TileIndexProperties["Name"].ToString();
                        Vector2 enemyLocation = new Vector2(col * _enemyLayer.TileWidth, row * _enemyLayer.TileHeight);
                        _enemies.Add(new Enemy(enemyName, enemyLocation, false));
                    }
                }
            }
            //make marker layers invisible
            _pickupLayer.Visible = false;
            _markerLayer.Visible = false;
            _enemyLayer.Visible = false;
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            if (_progressData.CurrentLevel == 0)
            {
                SoundPlayer.Update("howhighthemoon");
            }
            else if (_progressData.CurrentLevel == 1)
            {
                SoundPlayer.Update("djangostiger");// these are 24-bit .wav PCMs
            }
            else if (_progressData.CurrentLevel == 2)
            {
                SoundPlayer.Update("sweetsue");
            }
            else if (_progressData.CurrentLevel == 3)
            {
                SoundPlayer.Update("shine");
            }
            else if (_progressData.CurrentLevel == 4)
            {
                SoundPlayer.Update("chinaboy");
            }
            else if (_progressData.CurrentLevel ==5)
            {
                SoundPlayer.Update("younger");
            }
            handleInput(input);
            foreach (Pickup p in _pickups)
                p.Update(gameTime);
            foreach (Enemy e in _enemies)
            {
                e.Update(gameTime, onGround(e.Bottom, e.Left, e.Right));
                e.CheckAgainstPlayer(_gino);
                moveUnit(e, gameTime);
            }
            _gino.EquippedWeapon.Update(gameTime);
            Weapon.UpdateProjectiles(gameTime);
            moveProjectiles(gameTime);
            centerCamera(_gino.Center);
            _gino.Update(gameTime, onGround(_gino.Bottom, _gino.Left, _gino.Right));
            if (_gino.HitRect.Contains(_endPoint))
            {
                endLevel(true);
            }
            moveUnit(_gino, gameTime);

            if (_gino.State == Unit.UnitState.Dead)
            {
                _endTimer -= gameTime.ElapsedGameTime;
                if (_endTimer < TimeSpan.Zero)
                {
                    endLevel(false);
                }
            }
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
                _gino.EquippedWeapon.Fire(_gino.Center, _gino.Sprite.FacingRight ? Vector2.UnitX : -Vector2.UnitX);
            if (input.Debug1)
                _gino.Damage(50, Direction.East);
        }

        private void getPickup(string name, int row, int col)
        {
            if (_pickups.Count(t => t.Row == row && t.Col == col) == 0)
                return;

            _pickups.RemoveAll(t => t.Row == row && t.Col == col);
            if (name == "Coin")
            {
                _progressData.NumCoins += 1;
                SoundPlayer.playSoundEffects("hihatloop");
            }
            else
            {   //must be a weapon
                _gino.SetWeapon(new Weapon(name, _gino));
            }
        }

        private void moveProjectiles(GameTime gameTime)
        {
            Point point;
            foreach (Projectile p in Weapon.Projectiles)
            {
                if (p.Active)
                {
                    int pxRight = (int)(p.Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    int pxDown = (int)(p.Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (pxRight != 0)
                        checkHorizontalCollision(p, pxRight);
                    if (pxDown != 0)
                        checkVerticalCollision(p, pxDown);

                    p.Damage -= pxRight * p.DamageDrop;
                    point.X = (int)p.Position.X;
                    point.Y = (int)p.Position.Y;

                    if (p.Hostile)  //check for collision with player
                    {
                        if (_gino.HitRect.Contains(point))
                        {
                            p.CollideWithUnit(_gino);
                        }
                    }
                    else           //check for collision with enemies
                    {
                        foreach (Unit u in _enemies)
                        {
                            if (u.HitRect.Contains(point))
                            {
                                p.CollideWithUnit(u);
                            }
                        }
                    }
                }
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

            if (pxRight != 0)
                checkHorizontalCollision(unit, pxRight);
            if (pxDown != 0)
                checkVerticalCollision(unit, pxDown);

            if (unit.Top > _collisionLayer.LayerHeight * _collisionLayer.TileHeight)
            {
                unit.Damage(100, Direction.South);
            }
        }

        private void checkHorizontalCollision(Unit unit, int pxRight)
        {
            if (unit.Left < 0 || unit.Right >= _collisionLayer.LayerWidth * _collisionLayer.TileWidth)
            {
                unit.X += pxRight;
                return;
            }
            //Get the coordinate of the forward-facing edge
            //If walking left, forwardEdge = left of bounding box
            //If walking right, forwardEdge = right of bounding box
            int forwardEdge = (pxRight > 0) ? unit.Right : unit.Left;

            //-1 for left, 1 for right
            int xDirection = pxRight / Math.Abs(pxRight);

            int startCol = forwardEdge / _collisionLayer.TileWidth;
            int endCol = (forwardEdge + pxRight) / _collisionLayer.TileWidth;
            startCol = (int)MathHelper.Clamp(startCol, 0, _collisionLayer.LayerWidth - 1);
            endCol = (int)MathHelper.Clamp(endCol, 0, _collisionLayer.LayerWidth - 1);

            unit.X += pxRight;

            for (int col = startCol; col != (endCol + xDirection); col = col + xDirection)
            {
                for (int row = unit.Top / _collisionLayer.TileHeight;
                    row <= (unit.Bottom - 1) / _collisionLayer.TileHeight;
                    row++)
                {
                    if (!_collisionLayer.IsValidTileLocation(col, row))
                    {
                        continue;
                    }
                    //pickup checks
                    if (unit == _gino && _pickupLayer.Tiles[col, row] != null)
                    {
                        if (_pickupLayer.Tiles[col, row].TileIndexProperties.ContainsKey("PickupType"))
                        {
                            getPickup(_pickupLayer.Tiles[col, row].TileIndexProperties["PickupType"], row, col);
                        }
                    }
                    //check boundary collision for enemies
                    if (_enemyLayer.IsValidTileLocation(col, row))
                    {
                        if (_enemyLayer.Tiles[col, row] != null && 
                            _enemyLayer.Tiles[col, row].TileIndexProperties.ContainsKey("Name") &&
                            _enemyLayer.Tiles[col, row].TileIndexProperties["Name"] == "Bound")
                        {
                            unit.CollideWithObstacle(pxRight > 0 ? Direction.East : Direction.West);
                        }
                    }

                    //within bounds -- check tile collision
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
                            unit.Left = (col + 1) * _collisionLayer.TileWidth;
                        }
                        break;  //no need to check more
                    }
                }
            }
        }

        private void checkVerticalCollision(Unit unit, int pxDown)
        {
            if (unit.Bottom >= _collisionLayer.LayerHeight * _collisionLayer.TileHeight)
            {
                unit.Y += pxDown;
                return; //let unit fall
            }
            int forwardEdge = (pxDown > 0) ? unit.Bottom : unit.Top;

            //1 for down, -1 for up
            int yDirection = pxDown / Math.Abs(pxDown);

            //start at initial forward edge
            int startRow = forwardEdge / _collisionLayer.TileHeight;
            //assume unit moves full distance
            unit.Y += pxDown;
            //check up to new assumed unit hitrect edge
            int endRow = (forwardEdge + pxDown) / _collisionLayer.TileHeight;

            startRow = (int)MathHelper.Clamp(startRow, 0, _collisionLayer.LayerHeight - 1);
            endRow = (int)MathHelper.Clamp(endRow, 0, _collisionLayer.LayerHeight - 1);

            for (int row = startRow; row != (endRow + yDirection); row = row + yDirection)
            {
                for (int col = (unit.Left + 1) / _collisionLayer.TileWidth;
                    col <= (unit.Right - 1) / _collisionLayer.TileWidth;
                    col++)
                {
                    if (!_collisionLayer.IsValidTileLocation(col, row))
                    {
                        continue;
                    }

                    //pickup checks
                    if (unit == _gino && _pickupLayer.IsValidTileLocation(col, row) && _pickupLayer.Tiles[col, row] != null)
                    {
                        if (_pickupLayer.Tiles[col, row].TileIndexProperties.ContainsKey("PickupType"))
                        {
                            getPickup(_pickupLayer.Tiles[col, row].TileIndexProperties["PickupType"], row, col);
                        }
                    }

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

        private void checkHorizontalCollision(Projectile p, int pxRight)
        {
            //Get the coordinate of the forward-facing edge
            //If walking left, forwardEdge = left of bounding box
            //If walking right, forwardEdge = right of bounding box
            int forwardEdge = (int)p.Position.X;

            //-1 for left, 1 for right
            int xDirection = pxRight / Math.Abs(pxRight);

            int startCol = forwardEdge / _collisionLayer.TileWidth;
            int endCol = (forwardEdge + pxRight) / _collisionLayer.TileWidth;
            startCol = (int)MathHelper.Clamp(startCol, 0, _collisionLayer.LayerWidth - 1);
            endCol = (int)MathHelper.Clamp(endCol, 0, _collisionLayer.LayerWidth - 1);

            p.Position.X += pxRight;

            for (int col = startCol; col != (endCol + xDirection); col = col + xDirection)
            {
                for (int row = (int)p.Position.Y / _collisionLayer.TileHeight;
                    row <= p.Position.Y / _collisionLayer.TileHeight;
                    row++)
                {
                    if (!_collisionLayer.IsValidTileLocation(col, row))
                    {
                        continue;
                    }

                    //within bounds -- check tile collision
                    else if (_collisionLayer.Tiles[col, row] != null && _collisionLayer.Tiles[col, row].TileIndex != 0)
                    {
                        if (pxRight > 0)
                        {
                            p.CollideWithObstacle(Direction.East);
                            p.Position.X = col * _collisionLayer.TileWidth;
                        }
                        else
                        {
                            p.CollideWithObstacle(Direction.West);
                            p.Position.X = (col + 1) * _collisionLayer.TileWidth;
                        }
                        break;  //no need to check more
                    }
                }
            }
        }

        private void checkVerticalCollision(Projectile p, int pxDown)
        {
            int forwardEdge = (int)p.Position.Y;

            //1 for down, -1 for up
            int yDirection = pxDown / Math.Abs(pxDown);

            //start at initial forward edge
            int startRow = forwardEdge / _collisionLayer.TileHeight;
            //assume unit moves full distance
            p.Position.Y += pxDown;
            //check up to new assumed unit hitrect edge
            int endRow = (forwardEdge + pxDown) / _collisionLayer.TileHeight;
            startRow = (int)MathHelper.Clamp(startRow, 0, _collisionLayer.LayerHeight - 1);
            endRow = (int)MathHelper.Clamp(endRow, 0, _collisionLayer.LayerHeight - 1);

            for (int row = startRow; row != (endRow + yDirection); row = row + yDirection)
            {
                for (int col = ((int)p.Position.X + 1) / _collisionLayer.TileWidth;
                    col <= (p.Position.X - 1) / _collisionLayer.TileWidth;
                    col++)
                {
                    //boundary checks----------------------------------------------------------------
                    if (col < 0)
                    {
                        p.CollideWithObstacle(Direction.West);
                        p.Position.X = 0;
                        break;
                    }
                    else if (col > _collisionLayer.LayerWidth)
                    {
                        p.CollideWithObstacle(Direction.East);
                        p.Position.Y = _collisionLayer.LayerWidth * _collisionLayer.TileWidth;
                        break;
                    }
                    if (row < 0)
                    {
                        p.CollideWithObstacle(Direction.North);
                        p.Position.Y = 0;
                        break;
                    }
                    else if (row > _collisionLayer.LayerHeight)
                    {
                        p.CollideWithObstacle(Direction.South);
                        p.Position.Y = _collisionLayer.LayerHeight * _collisionLayer.TileHeight;
                        break;
                    }
                    //--------------------------------------------------------------------------------

                    if (_collisionLayer.Tiles[col, row] != null && _collisionLayer.Tiles[col, row].TileIndex != 0)
                    {
                        if (pxDown > 0)
                        {
                            p.CollideWithObstacle(Direction.South);
                            p.Position.Y = row * _collisionLayer.TileHeight;
                        }
                        else
                        {
                            p.CollideWithObstacle(Direction.North);
                            p.Position.Y = (row + 1) * _collisionLayer.TileHeight;
                        }
                        break;  //no need to check more
                    }
                }
            }
        }

        private bool onGround(int bottom, int left, int right)
        {
            int rowBelow = (bottom + 1) / _collisionLayer.TileHeight;
            if (rowBelow >= _collisionLayer.LayerHeight)
            {
                return false;   //below level
            }


            for (int col = (int)MathHelper.Clamp(left / _collisionLayer.TileWidth, 0, _collisionLayer.LayerWidth - 1);
                    col <= (right - 5) / _collisionLayer.TileWidth;
                    col++)
            {
                if (_collisionLayer.IsValidTileLocation(col, rowBelow) &&
                    _collisionLayer.Tiles[col, rowBelow] != null && _collisionLayer.Tiles[col, rowBelow].TileIndex != 0)
                {
                    return true;    //standing on a solid tile
                }
            }
            return false;       //no solid tiles right beneath unit
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

        private void endLevel(bool success)
        {
            //record level as completed if successful
            _progressData.LevelCompleted[_progressData.CurrentLevel - 1] = 
                _progressData.LevelCompleted[_progressData.CurrentLevel - 1] || success;
            Data.DataLoader.SaveProgress(_progressData);
            SoundPlayer.StopSound();
            //trigger the end-level sound

            SoundPlayer.playSoundEffects("endgamesound");
            SoundPlayer.StartSound("rosesdepicardie");

            _progressData.shopWeapon = "Revolver";
            _progressData.addAmmo = 0;
            NewState = new Overworld(_progressData);
        }

        public override void Draw(SpriteBatch sb)
        {
            _tileMap.Draw(MapDisplayDevice, _viewport);
            foreach (Pickup p in _pickups)
            {
                if (p.Active)
                {
                    SpriteView.DrawPickup(sb, p, _viewport.X, _viewport.Y);
                }
            }

            foreach (Unit u in _enemies)
            {
                SpriteView.DrawUnit(sb, u, _viewport.X, _viewport.Y);
            }

            SpriteView.DrawUnit(sb, _gino, _viewport.X, _viewport.Y);

            foreach (Projectile p in Weapon.Projectiles)
                SpriteView.DrawProjectile(sb, p, _viewport.X, _viewport.Y);

            XnaHelper.DisplayValue(sb, "Coins", _progressData.NumCoins.ToString(),
                new Rectangle(300, 100, 100, 20), Color.Black);
            // SpriteView.DrawUnit(sb, _enemies[0], _viewport.X, _viewport.Y);
        }
        #endregion
    }
}
