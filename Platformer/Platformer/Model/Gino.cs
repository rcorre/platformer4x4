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
        enum Weapon
        {
            Rifle,
            Shotgun,
            Pistol,
            Revolver
        }
        #endregion

        #region fields
        Sprite[] _sprites;
        #endregion

        #region properties
        #endregion

        #region constructor
        public Gino(Vector2 position, bool facingRight)
            : base(GINO_KEY, position, facingRight)
        {
            _sprites = new Sprite[] 
            {
                new Sprite("Gino-Rifle", true),
                new Sprite("Gino-Shotgun", true),
                new Sprite("Gino-Revolver", true),
                new Sprite("Gino-MachinePistol", true)
            };
            Sprite = _sprites[(int)Weapon.Rifle];
        }
        #endregion

        #region methods
        #endregion
    }
}
