using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Control
{
    class Overworld : GameState
    {
        #region static
        #endregion

        #region fields
        ProgressData _progressData;
        #endregion

        #region properties
        #endregion

        #region constructor
        public Overworld(ProgressData progressData)
        {
            _progressData = progressData;
        }
        #endregion

        #region methods
        public override void Update(GameTime gameTime, InputManager input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Call when the player selects a level in the Overworld
        /// will transfer the game state to the new level
        /// </summary>
        /// <param name="levelNum">index of selected level (level numbers start at 0)</param>
        private void SelectLevel(int levelNum)
        {
            NewState = new Level(levelNum, _progressData);
        }

        public override void Draw(SpriteBatch sb)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
