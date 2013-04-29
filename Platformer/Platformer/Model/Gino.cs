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
            Revolver,
            MachinePistol,
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
        }
        #endregion

        #region methods
        public void addHealth(int health)
        {
            base.addHealth(health);
        }

        public void addAmmo(int howMuch)
        {
            EquippedWeapon.addAmmo(howMuch);
        }

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
                    Sprite = _sprites[(int)WeaponType.MachinePistol];
                    break;
            }
        }
        #endregion
    }
}
