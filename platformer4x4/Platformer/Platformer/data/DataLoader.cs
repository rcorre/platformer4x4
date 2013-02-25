using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Model;
using Platformer.View;

namespace Platformer.Data
{
    static class DataLoader
    {
        //datafile locations
        const string SPRITEDATA_PATH = "Data/SpriteData.xml";
        const string UNITDATA_PATH = "Data/UnitData.xml";
        //ref to Game1.Content for loading resources
        public static ContentManager Content;

        public static Dictionary<string, Sprite.SpriteData> LoadSpriteData()
        {
            return (from sd in XElement.Load(SPRITEDATA_PATH).Descendants("SpriteData")
                    select new Sprite.SpriteData
                    {
                        Key = (string)sd.Attribute("Key"),
                        SpriteWidth = (int)sd.Attribute("SpriteWidth"),
                        SpriteHeight = (int)sd.Attribute("SpriteHeight"),
                        NumFrames = (int)sd.Attribute("NumFrames"),
                        NumStates = (int)sd.Attribute("NumStates"),
                        SecondsPerFrame = (float)sd.Attribute("SecondsPerFrame")
                    }).ToDictionary(t => t.Key);
        }

        public static Dictionary<string, Unit.UnitData> LoadUnitData()
        {
            return (from ud in XElement.Load(UNITDATA_PATH).Descendants("UnitData")
                    select new Unit.UnitData
                    {
                        Key = (string)ud.Attribute("Key"),
                        HitRectHeight = (int)ud.Attribute("HitRectHeight"),
                        HitRectWidth = (int)ud.Attribute("HitRectWidth"),
                        WalkAcceleration = (float)ud.Attribute("WalkAcceleration"),
                        MaxSpeed = (float)ud.Attribute("MaxSpeed"),
                        JumpSpeed = (float)ud.Attribute("JumpSpeed"),
                        HorizontalDeceleration = (float)ud.Attribute("HorizontalDeceleration"),
                        Gravity = (float)ud.Attribute("Gravity")
                    }).ToDictionary(t => t.Key);
        }

    }
}
