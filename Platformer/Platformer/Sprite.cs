using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Sprite
    {
        Texture2D _spriteSheet;
        Rectangle[,] _textureRects;
        Rectangle _hitRect;
        TimeSpan _animationInterval, _tillNextFrame;
        int _currentFrame, _numFrames;
        int _moveSpeed;

        bool _moving;

        enum SpriteState
        {
            FacingRight,
            FacingLeft
        }

        SpriteState _spriteState;

        public Rectangle HitRect
        {
            get { return _hitRect; }
        }

        public int MoveSpeed
        {
            get { return _moveSpeed; }
        }

        public Sprite(Texture2D spriteSheet, int spriteWidth, int spriteHeight, int numFrames, int numStates, TimeSpan animationTime, int moveSpeed)
        {
            _spriteSheet = spriteSheet;
            _hitRect = new Rectangle(0, 0, spriteWidth, spriteHeight);
            _textureRects = new Rectangle[numStates,numFrames];
            for (int i = 0; i < numStates; i++)
            {
                for (int j = 0; j < numFrames; j++)
                {
                    _textureRects[i,j] = new Rectangle(i * spriteWidth, j * spriteHeight, spriteWidth, spriteHeight);
                }
            }
            _animationInterval = animationTime;
            _tillNextFrame = _animationInterval;
            _currentFrame = 0;
            _numFrames = numFrames;
            _moveSpeed = moveSpeed;

            _spriteState = SpriteState.FacingLeft;
        }

        public void SetLocation(int x, int y)
        {
            _hitRect.X = x;
            _hitRect.Y = y;
        }

        public void Update(GameTime gameTime)
        {
            if (_moving)
            {
                _tillNextFrame -= gameTime.ElapsedGameTime;
                if (_tillNextFrame < TimeSpan.Zero)
                {
                    _currentFrame = (_currentFrame + 1) % _numFrames;
                    _tillNextFrame = _animationInterval;
                }
            }
            _moving = false;
        }

        public void Move(int pixelsRight, int pixelsDown)
        {
            if (pixelsRight > 0)
                _spriteState = SpriteState.FacingRight;
            if (pixelsRight < 0)
                _spriteState = SpriteState.FacingLeft;

            _hitRect.X += pixelsRight;
            _hitRect.Y += pixelsDown;

            _moving = true;
        }

        public void Draw(SpriteBatch sb)
        { 
            sb.Draw(_spriteSheet, _hitRect, _textureRects[(int)_spriteState, _currentFrame], Color.White);
        }
    }
}
