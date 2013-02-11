using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer.Model
{
    class Unit
    {
        #region classes
        public class UnitData
        {
            //name of unit. Must match name of associated sprite
            public string Key;
            public float WalkAcceleration;
            public float MaxSpeed;
            public float JumpSpeed;
            public int HitRectWidth;
            public int HitRectHeight;
            public float HorizontalDeceleration;
            public float VerticalDeceleration;
        }
        #endregion

        #region static
        public Dictionary<string, UnitData> UnitDataDict;
        #endregion

        #region fields
        Vector2 _position;  //center of hitrect and draw point of sprite
        Vector2 _velocity;  //speed and direction
        float _walkAcceleration;    //how much unit can accelerate when moving
        float _maxSpeed;    //max speed a unit can reach while moving
        float _jumpSpeed;    //initial vertical velocity while jumping
        Rectangle _hitRect; //for hit detection. Likely smaller than sprite
        Sprite _sprite;     //contains drawing information to be passed to SpriteView in Level.cs
        bool _facingRight;  //true if facing right, else facing left
        #endregion

        #region properties
        public int Left 
        { 
            get { return _hitRect.Left; }
            set
            {
                _position.X = value + _hitRect.Width / 2.0f;
                _hitRect.X = value;
            }
        }
        public int Right 
        { 
            get { return _hitRect.Right; }
            set { Left = value - _hitRect.Width; }
        }
        public int Top 
        { 
            get { return _hitRect.Top; }
            set
            {
                _position.Y = value + _hitRect.Height / 2.0f;
                _hitRect.Y = value;
            }
        }
        public int Right 
        { 
            get { return _hitRect.Bottom; }
            set { Top = value - _hitRect.Height; }
        }
        public Vector2 Center { get { return _position; } }
        #endregion

        #region constructor
        public Unit(string key, Vector2 position, bool facingRight)
        {
            UnitData data = UnitDataDict[key];
            _position = position;
            _velocity = Vector2.Zero;
            _walkAcceleration = data.WalkAcceleration;
            _maxSpeed = data.MaxSpeed;
            _jumpSpeed = data.JumpSpeed;
            _hitRect = new Rectangle(
                (int)(position.X - data.HitRectWidth / 2.0f),
                (int)(position.Y - data.HitRectHeight / 2.0f),
                data.HitRectWidth, data.HitRectHeight);
            _sprite = new Sprite(key);  //sprite key should match unit key
            _facingRight = true;
        }
        #endregion

        #region methods
        public void WalkRight()
        {
            _facingRight = true;
            _velocity.X += _walkAcceleration;
        }
        public void WalkLeft()
        {
            _facingRight = false;
            _velocity.X -= _walkAcceleration;
        }
        public void Jump()
        {
            _velocity.Y = _jumpSpeed;
        }
        #endregion
    }
}
