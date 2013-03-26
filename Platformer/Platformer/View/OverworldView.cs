using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.View
{
    static class OverworldView
    {
        #region constant
        #endregion

        #region static
        static Texture2D nodeTexture, backgroundTexture;
        static Color completedColor, uncompletedColor, unavailableColor, selectedColor;
        public static void LoadTextures(Texture2D node, Texture2D background)
        {
            nodeTexture = node;
            backgroundTexture = background;
            completedColor = Color.Green;
            uncompletedColor = Color.Yellow;
            unavailableColor = Color.Gray;
            selectedColor = Color.AliceBlue;
        }
        static Vector2 tempVec;
        #endregion

        #region fields
        #endregion
        
        #region properties
        #endregion

        #region constructor
        #endregion

        #region methods
        public static void DrawNode(SpriteBatch sb, int x, int y, bool completed, bool selected)
        {
            tempVec.X = x;
            tempVec.Y = y;
            Color color = completed ? completedColor : uncompletedColor;
            color = selected ? selectedColor : color;
            sb.Draw(nodeTexture, tempVec, color);
        }
        public static void DrawBackground(SpriteBatch sb)
        {
            sb.Draw(backgroundTexture, Vector2.Zero, Color.White);
        }
        #endregion
    }
}
