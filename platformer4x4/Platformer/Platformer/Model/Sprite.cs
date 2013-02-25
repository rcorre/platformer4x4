﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Model
{
    class Sprite
    {
        #region classes
        public class SpriteData
        {
            public string Key;
            public int SpriteWidth, SpriteHeight;
            public int NumFrames, NumStates;
            public float SecondsPerFrame;
        }
        #endregion
        #region static
        public static Dictionary<string, SpriteData> SpriteDataDict;
        #endregion

        #region fields
        //length of each animation, in # of frames
        int _framesPerAnimation;
        //number of different animations, e.g. for facing different directions
        int _numStates;
        //time between each animation frame, and countdown to next frame
        TimeSpan _animationInterval, _timeTillNext;
        int _currentFrame;  //animation frame (row of spritesheet
        int _currentState;  //animation state (column of spritesheet)
        float _scale;       //magnification, 1.0f is normal sprite size
        float _angle;       //rotation about origin (radians, 0 is straight up)
        Rectangle[,] _spriteSelectRects;    //texutre source selection rects [state, frame]
        Color _shade;        // color with which to draw sprite
        Vector2 _origin;     //center of texture
        bool _facingRight;  //true if facing right, else facing left
        string _textureKey; //key to access drawing texture
        #endregion

        #region properties
        public Rectangle TextureSelectRect
        {
            get { return _spriteSelectRects[_currentState, _currentFrame]; }
        }
        public float Scale { get { return _scale; } }
        public float Angle { get { return _angle; } }
        public Vector2 Origin { get { return _origin; } }
        public Color Shade { get { return _shade; } }
        public bool FacingRight 
        { 
            get { return _facingRight; }
            set { _facingRight = value; }
        }
        public string TextureKey { get { return _textureKey; } }
        #endregion

        #region constructor
        public Sprite(string name, bool facingRight)
        {
            _textureKey = name;
            SpriteData data = SpriteDataDict[name];
            _origin = new Vector2(data.SpriteWidth / 2.0f, data.SpriteHeight / 2.0f);
            _shade = Color.White;
            _currentFrame = 0;
            _currentState = 0;
            _scale = 1.0f;
            _numStates = data.NumStates;
            _framesPerAnimation = data.NumFrames;

            //set texture selection rectangles
            _spriteSelectRects = new Rectangle[_numStates, _framesPerAnimation];
            for (int i = 0; i < _numStates; i++)
            {
                for (int j = 0; j < _framesPerAnimation; j++)
                {
                    _spriteSelectRects[i, j] =
                        new Rectangle(i * data.SpriteWidth, j * data.SpriteHeight, data.SpriteWidth, data.SpriteHeight);
                }
            }

            //set animation timers
            _animationInterval = TimeSpan.FromSeconds(data.SecondsPerFrame);
            _timeTillNext = _animationInterval;
            _facingRight = facingRight;
        }
        #endregion

        #region methods
        /// <summary>
        /// Update sprites animation based on time elapsed. Should be called every Update loop
        /// </summary>
        /// <param name="animationNumber">Animation to play. If different from last call, reset at frame 0 for the new state</param>
        /// <param name="gameTime">Time elapsed since last call</param>
        /// <param name="speedFactor">Multiplier for animation speed. 1.0f is normal, 0.0f means freeze animation</param>
        public void Animate(int animationNumber, GameTime gameTime, float speedFactor)
        {
            if (animationNumber != _currentState)
            {   //change state, reset timer and frame
                _currentState = animationNumber;
                _currentFrame = 0;
                _timeTillNext = _animationInterval;
            }

            //decrement timer till next frame
            _timeTillNext -= TimeSpan.FromSeconds((float)gameTime.ElapsedGameTime.TotalSeconds * speedFactor);

            if (_timeTillNext <= TimeSpan.Zero)     //ready for new frame
            {
                //increment frame, reset to 0 if at last frame
                _currentFrame = (_currentFrame + 1) % _framesPerAnimation;
                _timeTillNext = _animationInterval;     //reset timer
            }
        }

        #endregion
    }
}
