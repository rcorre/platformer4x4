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
        //reusable vector for calculating unit draw locations
        static Vector2 locationVector;
        //reusable sprite for assigning sprites during draw calls
        static Sprite sprite;
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
        private static void drawSprite(SpriteBatch sb, string key, Vector2 position, 
            Rectangle source, Color color, Vector2 origin, float angle, float scale,
            bool facingRight)
        {
            sb.Draw(TextureDict[key], position, source, color, angle, origin, scale,
                facingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally , 0.0f);
        }

        /// <summary>
        /// Draw a unit's sprite to the screen
        /// </summary>
        /// <param name="sb">SpriteBatch with which to draw</param>
        /// <param name="unit">Unit to draw</param>
        /// <param name="xCameraOffset">x coordinate of left side of viewport</param>
        /// <param name="yCameraOffset">y coordinate of left side of viewport</param>
        public static void DrawUnit(SpriteBatch sb, Unit unit, int xCameraOffset, int yCameraOffset)
        {
            sprite = unit.Sprite;
            locationVector.X = unit.Center.X - xCameraOffset;
            locationVector.Y = unit.Center.Y - yCameraOffset;
            drawSprite(sb, sprite.TextureKey, locationVector, sprite.TextureSelectRect,
                 sprite.Shade, sprite.Origin, sprite.Angle, sprite.Scale, sprite.FacingRight);
        }

        /// <summary>
        /// Draw a pickup to the screen
        /// </summary>
        /// <param name="sb">SpriteBatch with which to draw</param>
        /// <param name="location">Location at which to draw sprite</param>
        /// <param name="theSprite">Sprite to draw</param>
        /// <param name="xCameraOffset">x coordinate of left side of viewport</param>
        /// <param name="yCameraOffset">y coordinate of left side of viewport</param>
        public static void DrawPickup(SpriteBatch sb, Pickup pickup, int xCameraOffset, int yCameraOffset)
        {
            sprite = pickup.PickupSprite;
            locationVector.X = pickup.Position.X - xCameraOffset;
            locationVector.Y = pickup.Position.Y - yCameraOffset;
            drawSprite(sb, sprite.TextureKey, locationVector, sprite.TextureSelectRect,
                 sprite.Shade, sprite.Origin, sprite.Angle, sprite.Scale, sprite.FacingRight);
        }

        /// <summary>
        /// Draw a pickup to the screen
        /// </summary>
        /// <param name="sb">SpriteBatch with which to draw</param>
        /// <param name="location">Location at which to draw sprite</param>
        /// <param name="theSprite">Sprite to draw</param>
        /// <param name="xCameraOffset">x coordinate of left side of viewport</param>
        /// <param name="yCameraOffset">y coordinate of left side of viewport</param>
        public static void DrawProjectile(SpriteBatch sb, Projectile projectile, int xCameraOffset, int yCameraOffset)
        {
            if (!projectile.Active)
                return;

            sprite = projectile.ProjectileSprite;
            locationVector.X = projectile.Position.X - xCameraOffset;
            locationVector.Y = projectile.Position.Y - yCameraOffset;
            drawSprite(sb, sprite.TextureKey, locationVector, sprite.TextureSelectRect,
                 sprite.Shade, sprite.Origin, sprite.Angle, sprite.Scale, sprite.FacingRight);
        }

        #endregion
    }
}
