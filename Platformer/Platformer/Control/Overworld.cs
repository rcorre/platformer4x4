using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Model;
using Platformer.View;

namespace Platformer.Control
{

    class Overworld : GameState
    {
        #region static

        /// <summary>
        /// Represents a level on the overworld map
        /// </summary>
        public class OverworldNode
        {
            public int LevelNumber;
            public string LevelName;
            public int X, Y;
            public bool[] ConnectedTo;    //ConnectedTo[i] is true if node <LevelNumber> is connected to node <i>
        }
        public static OverworldNode[] Nodes;
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
            //TODO -- add move animation between levels, checking for whether levels are connected/completed

            if (input.SelectLeft && _progressData.CurrentLevel > 0)
                _progressData.CurrentLevel--;
            else if (input.SelectRight && _progressData.CurrentLevel < Nodes.Length - 1)
                _progressData.CurrentLevel++;
            else if (input.ConfirmSelection)
                SelectLevel(_progressData.CurrentLevel);
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
            OverworldView.DrawBackground(sb);
            foreach (OverworldNode node in Nodes)
            {
                OverworldView.DrawNode(sb, node.X, node.Y, _progressData.LevelCompleted[node.LevelNumber]);
            }
        }
        #endregion
    }
}
