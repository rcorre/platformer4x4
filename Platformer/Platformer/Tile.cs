using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Tile
    {
        public const int TILE_WIDTH = 48;
        public const int TILE_HEIGHT = 48;
        static Texture2D tileTextureSheet;
        static Rectangle[] textureSelectRects;

        public static void LoadTileTextures(Texture2D textureSheet, int numTiles, int rowLength)
        {
            tileTextureSheet = textureSheet;
            textureSelectRects = new Rectangle[numTiles];
            int x = 0, y = 0;
            for (int i = 0; i < numTiles; i++)
            {
                if (i % rowLength == 0 && i != 0)
                {
                    x = 0;
                    y += TILE_HEIGHT;
                }

                textureSelectRects[i] = new Rectangle(x, y, TILE_WIDTH, TILE_HEIGHT);
                x += TILE_WIDTH;
            }
        }


        public int TileID;

        public Tile(int tileID)
        {
            TileID = tileID;
        }

        public void Draw(SpriteBatch sb, Rectangle drawRect)
        {
            sb.Draw(tileTextureSheet, drawRect, textureSelectRects[TileID], Color.White);
        }
    }
}
