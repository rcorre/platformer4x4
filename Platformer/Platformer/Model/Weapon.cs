using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Model
{
    struct Projectile
    {
        public bool Active;
        public Sprite ProjectileSprite;
        public Vector2 Position;
        public Vector2 Velocity;
        int Damage;
    }

    class WeaponData
    {
        public string Key;
        public string ProjectileSpriteKey;
        public float ProjectileSpeed;
        public float FireRate;
        public int Ammo;
        public int Damage;
    }

    class Weapon
    {
        #region constant
        const int MAX_PROJECTILE_COUNT = 10;
        #endregion

        #region static
        public static Dictionary<string, WeaponData> Data;
        public static Projectile[] Projectiles;
        public static void Initialize()
        {
            Projectiles = new Projectile[MAX_PROJECTILE_COUNT];
        }

        public static void UpdateProjectiles(GameTime gameTime)
        {
            for (int i = 0; i < Projectiles.Length; i++)
            {
                if (Projectiles[i].Active)
                {
                    Vector2.Add(ref Projectiles[i].Position,
                        ref Projectiles[i].Velocity,
                        out Projectiles[i].Position);
                    Projectiles[i].ProjectileSprite.Animate(0, gameTime, 1.0f);
                }
            }
        }
        #endregion

        #region properties
        #endregion

        #region fields
        Unit _owner;
        TimeSpan _fireTime, _tillNextFire;
        int _ammo;
        int _damage;
        float _projectileSpeed;
        bool _firing;
        Vector2 _fireLocation, _fireDirection;
        #endregion

        #region constructor
        public Weapon(string key, Unit owner)
            :this(Data[key], owner)
        { }

        protected Weapon(WeaponData data, Unit owner)
        {
            _owner = owner;
            _fireTime = TimeSpan.FromSeconds(1.0f / data.FireRate);
            _tillNextFire = _fireTime;
            _projectileSpeed = data.ProjectileSpeed;
            _ammo = data.Ammo;
            _damage = data.Damage;
        }
        #endregion

        #region methods
        public void Update(GameTime gameTime)
        {
            if (_tillNextFire > TimeSpan.Zero)
                _tillNextFire -= gameTime.ElapsedGameTime;
        }

        public void Fire(Vector2 fireLocation, Vector2 fireDirection)
        {
            if (_tillNextFire > TimeSpan.Zero || _ammo <= 0)
                return;

            for (int i = 0; i < Projectiles.Length; i++)
            {
                if (!Projectiles[i].Active)
                {
                    Projectiles[i].Active = true;
                    Vector2.Multiply(ref fireDirection, _projectileSpeed, out Projectiles[i].Velocity);
                    Projectiles[i].ProjectileSprite.Reset();
                    Projectiles[i].Position = fireLocation;
                    _ammo -= 1;
                    _tillNextFire = _fireTime;
                    break;
                }
            }
        }
        #endregion

    }
}
