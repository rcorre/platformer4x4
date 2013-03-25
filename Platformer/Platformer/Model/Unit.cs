using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer.Model
{
    class Unit
    {
        #region const
        #endregion

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
            public float Gravity;
        }
        #endregion

        #region static
        public static Dictionary<string, UnitData> UnitDataDict;

        public enum UnitState
        {
            Standing,
            Running,
            Drifting,   //not trying to run, but still moving
            Jumping,
            FreeFall,
        }

        #endregion

        #region fields
        Vector2 _position;  //center of hitrect and draw point of sprite
        protected Vector2 _velocity;  //speed and direction   (px/s)
        float _walkAcceleration;    //how much unit can accelerate when moving (px/sec^2)
        float _maxSpeed;    //max speed a unit can reach while moving (px/sec)
        float _jumpSpeed;    //initial vertical velocity while jumping (px/sec)
        float _horizontalDeceleration;  //how much to slow down each second while not running (px/sec^2)
        float _gravity;    //how much to increase vertical velocity while inAir (px/sec^2)
        Rectangle _hitRect; //for hit detection. Likely smaller than sprite
        Sprite _sprite;     //contains drawing information to be passed to SpriteView in Level.cs
        UnitState _state;   //current state of unit
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
        public int Bottom 
        { 
            get { return _hitRect.Bottom; }
            set { Top = value - _hitRect.Height; }
        }

        public Vector2 Center 
        { 
            get { return _position; }
        }

        public float X
        {
            get { return _position.X; }
            set
            {
                _position.X = value;
                Left = (int)(_position.X - _hitRect.Width / 2.0f);
            }
        }

        public float Y
        {
            get { return _position.Y; }
            set
            {
                _position.Y = value;
                Top = (int)(_position.Y - _hitRect.Height / 2.0f);
            }
        }

        public Rectangle HitRect { get { return _hitRect; } }

        public Sprite Sprite { get { return _sprite; } }

        public Vector2 Velocity { get { return _velocity; } }
        #endregion

        #region constructor
        public Unit(string key, Vector2 position, bool facingRight)
        {
            UnitData data = UnitDataDict[key];
            _position = position;
            _velocity = Vector2.Zero;
            _walkAcceleration = data.WalkAcceleration;
            _maxSpeed = data.MaxSpeed;
            _horizontalDeceleration = data.HorizontalDeceleration;
            _jumpSpeed = data.JumpSpeed;
            _gravity = data.Gravity;
            _hitRect = new Rectangle(
                (int)(position.X - data.HitRectWidth / 2.0f),
                (int)(position.Y - data.HitRectHeight / 2.0f),
                data.HitRectWidth, data.HitRectHeight);
            _sprite = new Sprite(key, facingRight);  //sprite key should match unit key
            _state = UnitState.Standing;
        }
        #endregion

        #region methods
        public void Walk(Direction direction)
        {
            if (_state != UnitState.FreeFall && _state != UnitState.Jumping)
            {
                _state = UnitState.Running;
                _sprite.FacingRight = (direction == Direction.East);
            }
        }

        public void Jump()
        {
            if (_state != UnitState.FreeFall)
            {
                _velocity.Y = -_jumpSpeed;
                _state = UnitState.FreeFall;
            }
        }

        public virtual void CollideWithObstacle(Direction direction)
        {
            switch (direction)
            {
                case Direction.South:
                    _velocity.Y = 0;
                    _state = UnitState.Drifting;
                    break;
                case Direction.North:
                    _velocity.Y = 0;
                    //stop jumping if hit head and start falling
                    _state = (_state == UnitState.Jumping) ? UnitState.FreeFall : _state;
                    break;
                case Direction.East:
                    //_velocity.X = 0;
                    _state = UnitState.Standing;
                    break;
                case Direction.West:
                    //_velocity.X = 0;
                    _state = UnitState.Standing;
                    break;
            }

        }

        public virtual void Update(GameTime gameTime, bool onGround)
        {
            if (!onGround)
            {
                _velocity.Y += _gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _state = UnitState.FreeFall;
            }

            if (_state == UnitState.Drifting)
            {
                if (Math.Abs(_velocity.X) <= _horizontalDeceleration)
                    _velocity.X = 0;
                else
                    _velocity.X += _horizontalDeceleration * (float)gameTime.ElapsedGameTime.TotalSeconds
                        * ((_velocity.X > 0) ? -1 : 1);     //make sure slowdown is opposite to direction of velocity
            }

            if (_state == UnitState.Running)
            {
                _velocity.X += _walkAcceleration * (float)gameTime.ElapsedGameTime.TotalSeconds
                    * ((_sprite.FacingRight) ? 1 : -1);     //increase velocity in run direction
                _state = UnitState.Drifting;
            }


            float xSpeedFactor = Math.Abs(_velocity.X / _maxSpeed);
            if (xSpeedFactor > 1.0f)
                _velocity.X /= xSpeedFactor;

            _sprite.Animate(1, gameTime, _velocity.X / _maxSpeed);  //running animation
        }
        #endregion
    }
}
