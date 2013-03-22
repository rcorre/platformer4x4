using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer.Model
{
    class Enemy : Unit
    {
        #region static
        static string[] ENEMY_KEYS;//for multiple enemy types
        static string ENEMY_KEY = "testenemy";//to test walking function
        #endregion

        #region fields
        #endregion

        #region properties
        #endregion

        #region constructor
        public Enemy(string key, Vector2 position, bool facingRight)
            :base(key, position, facingRight)
        {
        }
        #endregion

        #region methods
        public void turnAround()
        {
            

        }
        #endregion
    }
}
