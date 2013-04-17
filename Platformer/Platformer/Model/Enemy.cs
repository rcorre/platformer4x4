using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework;
using Platformer.Control;
using Platformer.Model;

namespace Platformer.Model
{
    class Enemy : Unit
    {
        #region constant
        const int MAX_Y_ATTACK_OFFSET = 50;
        #endregion

        #region static
        Vector2 tempVec;
        #endregion

        #region fields
        Vector2 position;
        Weapon _weapon;
        bool _attacking;
        #endregion

        #region properties
        #endregion

        #region constructor
        public Enemy(string key, Vector2 position, bool facingRight)
            : base(key, position, facingRight)
        {
            _weapon = (key == "Thug") ? new Weapon("Knuckles", this) : new Weapon("EnemyRifle", this);
        }
        #endregion

        #region methods
        public override void CollideWithObstacle(Direction direction)
        {
            base.CollideWithObstacle(direction);
            _velocity.X = 0;
            switch (direction)
            {
                case Direction.East:
                    Sprite.FacingRight = false;
                    break;
                case Direction.West:
                    Sprite.FacingRight = true;
                    break;
            }
        }

        public override void Update(GameTime gameTime, bool onGround)
        {
            if (_attacking)
            {
                Sprite.Animate((int)UnitSpriteState.Shoot, gameTime, 1.0f, true);
            }

            else if (onGround)
            {
                Walk(Sprite.FacingRight ? Direction.East : Direction.West);
            }

            _weapon.Update(gameTime);
            Sprite.AnimationLock = _attacking;
            base.Update(gameTime, onGround);
            Sprite.AnimationLock = false;
        }

        public void CheckAgainstPlayer(Unit player)
        {
            float xDisp = player.Center.X - Center.X;
            float yDisp = player.Center.Y - Center.Y;
            //dont fire if not facing
            if ((xDisp >= 0 && !Sprite.FacingRight) || (xDisp <= 0 && Sprite.FacingRight))
            {
                return;
            }

            //fire if close enough to player
            if (Math.Abs(xDisp) < _weapon.Range && Math.Abs(yDisp) < MAX_Y_ATTACK_OFFSET)
            {
                _attacking = true;
                _weapon.Fire(Center, Vector2.UnitX * ((player.Center.X > Center.X) ? 1 : -1));
            }
        }
        #endregion
    }
}
