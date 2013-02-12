using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Platformer.Model
{
    class Gino : Unit
    {
        #region const
        const string GINO_KEY = "Gino";
        #endregion

        #region static
        #endregion

        #region fields
        #endregion

        #region properties
        #endregion

        #region constructor
        public Gino(Vector2 position, bool facingRight)
            : base(GINO_KEY, position, facingRight)
        { }
        #endregion

        #region methods
        #endregion
    }
}
