using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

using System.Xml.Linq;

using Microsoft.Xna.Framework;

namespace Platformer
{
    class TileMap
    {
        Tile[][] _tileMap;
        int _numRows;
        int _numCols;

        Rectangle _drawRect;
        Rectangle _tileDrawRect;

        int _offsetX, _offsetY;
        public int OffsetX
        {
            get { return _offsetX; }
            set
            {
                if (value < 0)
                    _offsetX = 0;
                else if (value + _drawRect.Width > _numCols * Tile.TILE_WIDTH)
                    _offsetX = _numCols * Tile.TILE_WIDTH - _drawRect.Width;
                else
                    _offsetX = value;

            }
        }

        public int OffsetY
        {
            get { return _offsetY; }
            set
            {
                if (value < 0)
                    _offsetY = 0;
                else if (value + _drawRect.Height > _numRows * Tile.TILE_HEIGHT)
                    _offsetY = _numRows * Tile.TILE_HEIGHT - _drawRect.Height;
                else
                    _offsetY = value;

            }
        }

        public TileMap(ContentManager content, Rectangle drawRect)
        {
            _drawRect = drawRect;
            _tileDrawRect = new Rectangle(0, 0, Tile.TILE_WIDTH, Tile.TILE_HEIGHT);

            int[][] tileIDs =
                    (from row in XElement.Load("MapData.xml").Descendants("TileRow")
                     select (from col in row.Attribute("Tiles").Value.Split(',')
                             select int.Parse(col)).ToArray<int>()).ToArray<int[]>();
            _numRows = tileIDs.Length;
            _numCols = tileIDs[0].Length;

            _tileMap = new Tile[tileIDs.Length][];
            for (int i = 0; i < tileIDs.Length; i++)
            {
                _tileMap[i] = new Tile[tileIDs[i].Length];
                for (int j = 0; j < tileIDs[i].Length; j++)
                    _tileMap[i][j] = new Tile(tileIDs[i][j]);
            }

        }

        public bool TilePassable(int row, int col)
        {
            //if tile out of range, it is not passable
            if (row < 0 || row >= _numRows || col < 0 || col >= _numCols)
                return false;
            //for this simple demo, I define tileID 0 as passable, all others as impassable
            return (_tileMap[row][col].TileID == 0);
        }

        public void Draw(SpriteBatch sb)
        {
            int startCol = _offsetX / Tile.TILE_WIDTH;
            int startRow = _offsetY / Tile.TILE_HEIGHT;
            _tileDrawRect.X = -(_offsetX % Tile.TILE_WIDTH);
            _tileDrawRect.Y = -(_offsetY % Tile.TILE_HEIGHT);
            int col = startCol;
            int row = startRow;
            while (_tileDrawRect.Top <= _drawRect.Bottom)
            {
                while (_tileDrawRect.Left <= _drawRect.Right)
                {
                    _tileMap[row][col].Draw(sb, _tileDrawRect);
                    _tileDrawRect.X += Tile.TILE_WIDTH;
                    col++;
                }
                _tileDrawRect.Y += Tile.TILE_HEIGHT;
                row++;
                _tileDrawRect.X = -(_offsetX % Tile.TILE_WIDTH);
                col = startCol;
            }
            
        }
    }
}
