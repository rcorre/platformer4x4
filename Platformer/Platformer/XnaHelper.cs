using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Platformer
{
    static class XnaHelper
    {
        public static Texture2D PixelTexture;

        public static SpriteFont Font;

        static Random rand = new Random();
        /// <summary>
        /// get a unit vector pointing from start to end
        /// </summary>
        /// <param name="start">direction vector origin</param>
        /// <param name="end">direction vector destination</param>
        /// <returns></returns>
        public static Vector2 DirectionBetween(Vector2 start, Vector2 end)
        {
            Vector2 direction = end - start;

            if (direction.Length() > 0)
                direction.Normalize();

            return direction;
        }

        /// <summary>
        /// get the angle (in radians) that a vector is pointing
        /// </summary>
        /// <param name="direction">vector from which to compute angle</param>
        /// <returns></returns>
        public static float RadiansFromVector(Vector2 direction)
        {
            return (float)Math.Atan2(direction.X, -direction.Y);
        }

        /// <summary>
        /// get the angle (in degrees) that a vector is pointing
        /// </summary>
        /// <param name="direction">vector from which to compute angle</param>
        /// <returns></returns>
        public static float DegreesFromVector(Vector2 direction)
        {
            return MathHelper.ToDegrees((float)Math.Atan2(direction.X, -direction.Y));
        }

        /// <summary>
        /// get a unit vector pointing in the direction of the angle
        /// </summary>
        /// <param name="angle">angle in radians</param>
        /// <returns></returns>
        public static Vector2 VectorFromAngle(float angle)
        {
            Matrix rotMatrix = Matrix.CreateRotationZ(angle);
            return Vector2.Transform(-Vector2.UnitY, rotMatrix);
        }

        public static bool RectsCollide(Rectangle rect1, Rectangle rect2)
        {
            return (rect1.Right > rect2.Left && rect1.Left < rect2.Right &&
                    rect1.Bottom > rect2.Top && rect1.Top < rect2.Bottom);
        }

        public static bool PointInRect(Vector2 point, Rectangle rect)
        {
            return (rect.Left <= point.X && point.X <= rect.Right && rect.Top <= point.Y && point.Y <= rect.Bottom);
        }

        /// <summary>
        /// Generate a random angle(degrees) from within a given arc
        /// </summary>
        /// <param name="centerAngle">Angle of the center of the arc (degrees, clockwise from vertical)</param>
        /// <param name="arc">Width of arc (degrees, edge to edge)</param>
        /// <returns></returns>
        public static float RandomAngle(float centerAngle, float arc)
        {
            return centerAngle + arc * (0.5f - (float)rand.NextDouble());
        }

        public static void RandomizeVector(ref Vector2 refVector, float minX, float maxX, float minY, float maxY)
        {
            refVector.X = minX + (float)rand.NextDouble() * (maxX - minX);
            refVector.Y = minY + (float)rand.NextDouble() * (maxY - minY);
        }

        public static int RandomInt(int min, int max)
        {
            return rand.Next(min, max);
        }

        public static void DrawRect(Color color, Rectangle rect, SpriteBatch sb)
        {
            sb.Draw(PixelTexture, rect, color);
        }

        /// <summary>
        /// Draws the given string as large as possible inside the boundaries Rectangle without going
        /// outside of it.  This is accomplished by scaling the string (since the SpriteFont has a specific
        /// size).
        ///
        /// If the string is not a perfect match inside of the boundaries (which it would rarely be), then
        /// the string will be absolutely-centered inside of the boundaries.
        /// </summary>
        /// <param name="font"></param>
        /// <param name="strToDraw"></param>
        /// <param name="drawRect"></param>
        static public void DisplayValue(SpriteBatch spriteBatch, string label, string value, Rectangle drawRect, Color color)
        {
            String strToDraw = label + ": " + value;
            Vector2 size = Font.MeasureString(strToDraw);

            float xScale = (drawRect.Width / size.X);
            float yScale = (drawRect.Height / size.Y);

            // Taking the smaller scaling value will result in the text always fitting in the boundaires.
            float scale = Math.Min(xScale, yScale);

            // Figure out the location to absolutely-center it in the boundaries rectangle.
            int strWidth = (int)Math.Round(size.X * scale);
            int strHeight = (int)Math.Round(size.Y * scale);
            Vector2 position = new Vector2();
            position.X = (((drawRect.Width - strWidth) / 2) + drawRect.X);
            position.Y = (((drawRect.Height - strHeight) / 2) + drawRect.Y);

            // A bunch of settings where we just want to use reasonable defaults.
            float rotation = 0.0f;
            Vector2 spriteOrigin = new Vector2(0, 0);
            float spriteLayer = 0.0f; // all the way in the front
            SpriteEffects spriteEffects = new SpriteEffects();

            // Draw the string to the sprite batch!
            spriteBatch.DrawString(Font, strToDraw, position, color, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        } // end DrawString()
    }
}
