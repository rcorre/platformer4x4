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
        #region static
        static string ENEMY_KEY = "Enemy1";//to test walking function
        #endregion

        #region fields
        Vector2 position;
        #endregion

        #region properties
        #endregion

        #region constructor
        public Enemy(string key, Vector2 position, bool facingRight)
            : base(key, position, facingRight)
        { }
        #endregion

        #region methods
        public override void CollideWithObstacle(Direction direction)
        {
            base.CollideWithObstacle(direction);
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
            Walk(Sprite.FacingRight ? Direction.East : Direction.West);
            base.Update(gameTime, onGround);
        }
        #endregion
    }
}
