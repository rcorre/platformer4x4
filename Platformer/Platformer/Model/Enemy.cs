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
        static string ENEMY_KEY = "gino";//to test walking function
        #endregion

        #region fields
        Vector2 position;
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
       
          public void Walk(GameTime gameTime)
          {
              
              this.Walk(Direction.East);
            
              if (this.X > 700 && this.X < 900)
              {
                  this.Jump();
              }
              else if (this.X > 700)
                  this.Walk(Direction.West);
              
             // this.Walk(Direction.West);
                  
          }
        }
        #endregion
    }
