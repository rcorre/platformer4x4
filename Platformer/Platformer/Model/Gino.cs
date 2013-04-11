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
        enum WeaponType
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
        public Weapon EquippedWeapon { get; private set;}
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
            Sprite = _sprites[(int)WeaponType.Rifle];
        }
        #endregion

        #region methods
        public void SetWeapon(Weapon weapon)
        {
            EquippedWeapon = weapon;
            switch (weapon.Name)
            {
                case "Rifle":
                    Sprite = _sprites[(int)WeaponType.Rifle];
                    break;
                case "Shotgun":
                    Sprite = _sprites[(int)WeaponType.Shotgun];
                    break;
                case "Revolver":
                    Sprite = _sprites[(int)WeaponType.Revolver];
                    break;
                case "MachinePistol":
                    Sprite = _sprites[(int)WeaponType.Pistol];
                    break;
            }
        }
        #endregion
    }
}
