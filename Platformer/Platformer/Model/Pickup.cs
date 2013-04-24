using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Platformer.View;

namespace Platformer.Model
{
    class Pickup
    {
        #region constant
        #endregion

        #region static
        public enum PickupType
        {
            Coin,
            Revolver,
            MachinePistol,
            Rifle,
            Shotgun
        }
        #endregion

        #region properties
        public bool Active { get; set; }
        public Vector2 Position { get; private set; }
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Sprite PickupSprite
        {
            get { return _sprite; }
        }
        public string PickupName { get; private set; }
        #endregion

        #region fields
        Sprite _sprite;
        #endregion

        #region constructor
        public Pickup(int row, int col, Vector2 drawLocation, string pickupName)
        {
            PickupName = pickupName;
            _sprite = new Sprite(pickupName, true);
            Active = true;
            Row = row;
            Col = col;
            Position = drawLocation;
        }
        #endregion

        #region methods
        public void Update(GameTime gameTime)
        {
            _sprite.Animate(0, gameTime, 1.0f, true);
        }
        #endregion
    }
}
