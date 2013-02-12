using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Model;

namespace Platformer.View
{
    static class SpriteView
    {
        #region static
        const string SPRITESHEET_PATH = "spritesheets/";
        static Dictionary<string, Texture2D> TextureDict;
        /// <summary>
        /// Call during game1.LoadContent to load all needed textures
        /// </summary>
        /// <param name="textureNames">string array containing the names of all textures to load</param>
        /// <param name="content">ContentManager to load textures with</param>
        public static void LoadTextures(string[] textureNames, ContentManager content)
        {
            TextureDict = new Dictionary<string, Texture2D>();
            foreach (string s in textureNames)
                TextureDict[s] = content.Load<Texture2D>(SPRITESHEET_PATH + s);
        }
        #endregion

        #region fields
        #endregion

        #region properties
        #endregion

        #region constructor
        #endregion

        #region methods
        /// <summary>
        /// Draw a sprite given the specified parameters
        /// </summary>
        /// <param name="sb">SpriteBatch with which to draw</param>
        /// <param name="key">Name of spritesheet texture</param>
        /// <param name="position">Location to draw center of sprite</param>
        /// <param name="source">Rectangle to select frame from spritesheet</param>
        /// <param name="color">Color with which to draw sprite</param>
        /// <param name="origin">Point about which to center texture</param>
        /// <param name="angle">Rotation of sprite, where 0 is north(radians)</param>
        /// <param name="scale">Sizing. 1.0f is normal size</param>
        /// <param name="facingRight">Whether Sprite is facing to right (if not, will be mirrored horizontally)</param>
        public static void DrawSprite(SpriteBatch sb, string key, Vector2 position, 
            Rectangle source, Color color, Vector2 origin, float angle, float scale,
            bool facingRight)
        {
            sb.Draw(TextureDict[key], position, source, color, angle, origin, scale,
                facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally , 0.0f);
        }

        public static void DrawSprite(SpriteBatch sb, Unit unit)
        {
            Sprite s = unit.Sprite;
            DrawSprite(sb, s.TextureKey, unit.Center, s.TextureSelectRect,
                 s.Shade, s.Origin, s.Angle, s.Scale, s.FacingRight);
        }

        #endregion
    }
}
