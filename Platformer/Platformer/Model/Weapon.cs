using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Platformer.View;

namespace Platformer.Model
{
    class Projectile
    {
        public bool Active;
        public bool Hostile;    //if true, collides with player. else collides with enemies
        public Sprite ProjectileSprite;
        public Vector2 Position;
        public Vector2 Velocity;
        public int Damage;
        public int DistanceLeft;

        public void CollideWithObstacle(Direction direction)
        {
            Active = false;
        }
        public void CollideWithUnit(Unit unit)
        {
            if (unit.State == Unit.UnitState.Dead)
                return;

            Active = false;
            unit.Damage(Damage, (Velocity.X > 0) ? Direction.West : Direction.East);
        }
    }

    class WeaponData
    {
        public string Key;
        public string ProjectileSpriteKey;
        public float ProjectileSpeed;
        public float FireRate;
        public int Ammo;
        public int Damage;
        public int Range;
    }

    class Weapon
    {
        #region constant
        const int MAX_PROJECTILE_COUNT = 20;
        #endregion

        #region static
        public static Dictionary<string, WeaponData> Data;
        public static Projectile[] Projectiles;
        public static void Initialize()
        {
            Projectiles = new Projectile[MAX_PROJECTILE_COUNT];
            for (int i = 0; i < Projectiles.Length; i++)
            {
                Projectiles[i] = new Projectile();
            }
        }

        public static void UpdateProjectiles(GameTime gameTime)
        {
            for (int i = 0; i < Projectiles.Length; i++)
            {
                if (Projectiles[i].Active)
                {
                    Projectiles[i].Active = Projectiles[i].DistanceLeft > 0;
                    Projectiles[i].ProjectileSprite.Animate(0, gameTime, 1.0f, true);
                }
            }
        }
        #endregion

        #region properties
        public int Ammo;
        public string Name { get; private set; }
        public float Range { get { return _range; } }
        #endregion

        #region fields
        Unit _owner;
        TimeSpan _fireTime, _tillNextFire;
        int _ammo;
        int _damage;
        float _projectileSpeed;
        bool _firing;
        bool _hostile;
        Vector2 _fireLocation, _fireDirection;
        string _projectileSpriteKey;
        int _range;
        #endregion

        #region constructor
        public Weapon(string key, Unit owner)
            :this(Data[key], owner)
        { }

        protected Weapon(WeaponData data, Unit owner)
        {
            _owner = owner;
            _hostile = (owner.GetType() != typeof(Gino));
            _fireTime = TimeSpan.FromSeconds(1.0f / data.FireRate);
            _tillNextFire = TimeSpan.Zero;
            _projectileSpeed = data.ProjectileSpeed;
            _projectileSpriteKey = data.ProjectileSpriteKey;
            _ammo = data.Ammo;
            _damage = data.Damage;
            _range = data.Range;
            Name = data.Key;
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
            {
                return;
            }

            SoundPlayer.playSoundEffects("snare");

            for (int i = 0; i < Projectiles.Length; i++)
            {
                if (!Projectiles[i].Active)
                {
                    Projectiles[i].Active = true;
                    Projectiles[i].Hostile = _hostile;
                    Projectiles[i].Damage = _damage;
                    Projectiles[i].ProjectileSprite = new Sprite(_projectileSpriteKey, fireDirection.X > 0);
                    Vector2.Multiply(ref fireDirection, _projectileSpeed, out Projectiles[i].Velocity);
                    Projectiles[i].Position = fireLocation;
                    Projectiles[i].DistanceLeft = _range;
                    break;
                }
            }
            _ammo -= 1;
            _tillNextFire = _fireTime;
        }
        #endregion

    }
}
