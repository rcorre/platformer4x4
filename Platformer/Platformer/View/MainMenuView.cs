using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.View
{
    static class MainMenuView
    {
        #region constant
        #endregion

        #region static
        static Texture2D backgroundTexture;
        public static void LoadTextures(Texture2D background)
        {
            backgroundTexture = background;
        }
        #endregion

        #region fields
        #endregion

        #region properties
        #endregion

        #region constructor
        #endregion

        #region methods
        public static void DrawBackground(SpriteBatch sb)
        {
            sb.Draw(backgroundTexture, Vector2.Zero, Color.White);
        }
        #endregion
    }
}
