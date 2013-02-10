using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Model
{
    class Sprite
    {
        #region classes
        public class SpriteSheetData
        {
            string TextureName;
            int SpriteWidth, SpriteHeight;
            int NumFrames, NumStates;
        }
        #endregion
        #region static
        public static Dictionary<string, SpriteSheetData> SpriteDataDict;
        #endregion

        #region fields
        //length of each animation, in # of frames
        int _framesPerAnimation;
        //number of different animations, e.g. for facing different directions
        int _numStates;
        //time between each animation frame, and countdown to next frame
        TimeSpan _animationInterval, _timeTillNext;
        int _currentFrame;
        int _currentState;
        float _scale;
        //organized by state, frame
        Rectangle[,] _spriteSelectRects;
        Color _shade;
        Vector2 _origin;
        #endregion

        #region properties
        #endregion

        #region constructor
        #endregion

        #region methods
        #endregion
    }
}
