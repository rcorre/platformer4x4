using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Platformer.Model;
using Platformer.View;
using Platformer.Control;

namespace Platformer.Data
{
    static class DataLoader
    {
        //datafile locations
        const string SPRITEDATA_PATH = "Data/SpriteData.xml";
        const string UNITDATA_PATH = "Data/UnitData.xml";
        const string WEAPONDATA_PATH = "Data/WeaponData.xml";
        const string OVERWORLDDATA_PATH = "Data/OverworldData.xml";
        //ref to Game1.Content for loading resources
        public static ContentManager Content;
        //for saving data:
        const string PROGRESSDATA_CONTAINER = "ProgressData";
        const string PROGRESSDATA_FILE = "progress.sav";

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
                        SecondsPerFrame = (float)sd.Attribute("SecondsPerFrame"),
                        Scale = (float)sd.Attribute("Scale")
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
                        Health = (int)ud.Attribute("Health"),
                        HorizontalDeceleration = (float)ud.Attribute("HorizontalDeceleration"),
                        Gravity = (float)ud.Attribute("Gravity")
                    }).ToDictionary(t => t.Key);
        }

        public static Dictionary<string, WeaponData> LoadWeaponData()
        {
            return (from el in XElement.Load(WEAPONDATA_PATH).Descendants("WeaponData")
                    select buildWeaponData(el)).ToDictionary(t => t.Key);
                    
        }

        private static WeaponData buildWeaponData(XElement el)
        {
            WeaponData data = new WeaponData();
            Type dataType = data.GetType();

            foreach (XAttribute at in el.Attributes())
            {
                    string fieldName = at.Name.LocalName;
                    System.Reflection.FieldInfo p = dataType.GetField(fieldName);
                    data.GetType().GetField(fieldName).SetValue(data, Convert.ChangeType(at.Value, p.FieldType));
            }
            return data;
        }

        public static Overworld.OverworldNode[] LoadOverworldData()
        {
            int numNodes = 0;
            foreach (XElement el in XElement.Load(OVERWORLDDATA_PATH).Descendants("OverworldNode"))
                numNodes++;     //count nodes

            return (from el in XElement.Load(OVERWORLDDATA_PATH).Descendants("OverworldNode")
                    select buildOverworldNode(el, numNodes)).ToArray();
        }

        private static Overworld.OverworldNode buildOverworldNode(XElement el, int numNodes)
        {
            Overworld.OverworldNode node = new Overworld.OverworldNode();
            Type dataType = node.GetType();

            foreach (XAttribute at in el.Attributes())
            {
                    string fieldName = at.Name.LocalName;
                    if (fieldName == "ConnectedTo")
                    {
                        string[] nums = at.Value.Split(',');
                        node.ConnectedTo = new bool[numNodes];
                        for (int i = 0; i < nums.Length; i++)
                        {
                            node.ConnectedTo[Int32.Parse(nums[i])] = true;
                        }
                    }
                    else
                    {
                        System.Reflection.FieldInfo p = dataType.GetField(fieldName);
                        node.GetType().GetField(fieldName).SetValue(node, Convert.ChangeType(at.Value, p.FieldType));
                    }
            }
            return node;
        }

        public static void SaveProgress(ProgressData data)
        {
            FileStream fs = new FileStream(PROGRESSDATA_FILE, FileMode.Create);
            XmlSerializer xs = new XmlSerializer(data.GetType());
            xs.Serialize(fs, data);
            fs.Close();
        }

        public static ProgressData LoadProgress()
        {
            ProgressData data = new ProgressData();
            FileStream fs = new FileStream(PROGRESSDATA_FILE, FileMode.Open, FileAccess.Read);
            XmlSerializer xs = new XmlSerializer(data.GetType());
            data = (ProgressData)xs.Deserialize(fs);
            fs.Close();
            return data;
        }


    }
}
