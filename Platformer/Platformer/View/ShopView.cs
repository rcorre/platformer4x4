using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.View
{
    static class ShopView
    {
        #region constant
        #endregion

        #region static
        static int[] pricesVal = { 1, 2, 3, 4, 1 };
        static String[] prices = {"$100","$200","$300","$400","$100"};
        static Vector2[] priceSpot = { new Vector2(55, 365), new Vector2(215, 365),
                    new Vector2(445, 365), new Vector2(685, 365), new Vector2(305, 685)};
        static Vector2 moneys = new Vector2(925, 50);
        static Texture2D backgroundTexture;
        static Texture2D[] revolver = new Texture2D[2];
        static Texture2D[] rifle = new Texture2D[2];
        static Texture2D[] shotgun = new Texture2D[2];
        static Texture2D[] uzi = new Texture2D[2];
        static Texture2D[] fedora = new Texture2D[2];
        static SpriteFont Font1;
        
        public static void LoadTextures(Texture2D background)
        {
            backgroundTexture = background;
        }
        public static void LoadFont(SpriteFont f)
        {
            Font1 = f;
        }
        public static void LoadGuns(Texture2D gun1, Texture2D gun2, Texture2D gun3, Texture2D gun4)
        {
            revolver[1] = gun1;
            rifle[1] = gun2;
            shotgun[1] = gun3;
            uzi[1] = gun4;
        }
        public static void LoadGray(Texture2D gun1, Texture2D gun2, Texture2D gun3, Texture2D gun4, Texture2D arm1)
        {
            revolver[0] = gun1;
            rifle[0] = gun2;
            shotgun[0] = gun3;
            uzi[0] = gun4;
            fedora[0] = arm1;
        } 
        public static void LoadArmor(Texture2D arm1)
        {
            fedora[1] = arm1;
        }
        static Rectangle revolverRect = new Rectangle(50, 260, 130, 100);
        static Rectangle shotgunRect = new Rectangle(210, 260, 200, 100); 
        static Rectangle rifleRect = new Rectangle(440, 260, 200, 100);
        static Rectangle uziRect = new Rectangle(680, 260, 130, 100);
        static Rectangle fedoraRect = new Rectangle(300, 540, 250, 150);
        static Rectangle mainPicRect = new Rectangle(985, 280, 200, 130);
        static Rectangle buyRect = new Rectangle(885, 535, 370, 80);
        static Rectangle contRect = new Rectangle(885, 630, 370, 80);
        #endregion

        #region fields
        
        #endregion

        #region properties
        #endregion

        #region constructor
        #endregion

        #region methods
        public static int CheckLocation(int x, int y)
        {
            if ((x > revolverRect.X) && (x < (revolverRect.X + revolverRect.Width))
                && (y > revolverRect.Y) && (y < (revolverRect.Y + revolverRect.Height)))
                return 1;
            else if ((x > shotgunRect.X) && (x < (shotgunRect.X + shotgunRect.Width))
                && (y > shotgunRect.Y) && (y < (shotgunRect.Y + shotgunRect.Height)))
                return 2;
            else if ((x > rifleRect.X) && (x < (rifleRect.X + rifleRect.Width))
               && (y > rifleRect.Y) && (y < (rifleRect.Y + rifleRect.Height)))
                return 3;
            else if ((x > uziRect.X) && (x < (uziRect.X + uziRect.Width))
               && (y > uziRect.Y) && (y < (uziRect.Y + uziRect.Height)))
                return 4;
            else if ((x > fedoraRect.X) && (x < (fedoraRect.X + fedoraRect.Width))
               && (y > fedoraRect.Y) && (y < (fedoraRect.Y + fedoraRect.Height)))
                return 5;
            else if ((x > buyRect.X) && (x < (buyRect.X + buyRect.Width))
               && (y > buyRect.Y) && (y < (buyRect.Y + buyRect.Height)))
                return 6;
            else if ((x > contRect.X) && (x < (contRect.X + contRect.Width))
               && (y > contRect.Y) && (y < (contRect.Y + contRect.Height)))
                return 7;
            else
                return 0;//this guy will check if mouse is hovering over an item||box and return #
        }

        public static int getPrice(int x)
        {
            if (x < 5 && x >= 0)
                return pricesVal[x];
            else
                return 0;
        }

        public static void DrawShop(SpriteBatch sb, int[] indices, int mainPic)
        {
            sb.Draw(backgroundTexture, Vector2.Zero, Color.White);
            sb.Draw(revolver[indices[0]], revolverRect, Color.White);
            sb.Draw(shotgun[indices[1]], shotgunRect, Color.White);
            sb.Draw(rifle[indices[2]], rifleRect, Color.White);
            sb.Draw(uzi[indices[3]], uziRect, Color.White);
            sb.Draw(fedora[indices[4]], fedoraRect, Color.White);
            if (mainPic == 1)
                sb.Draw(revolver[indices[0]], mainPicRect, Color.White);
            else if (mainPic == 2)
                sb.Draw(shotgun[indices[1]], mainPicRect, Color.White);
            else if (mainPic == 3)
                sb.Draw(rifle[indices[2]], mainPicRect, Color.White);
            else if (mainPic == 4)
                sb.Draw(uzi[indices[3]], mainPicRect, Color.White);
            else if (mainPic == 5)
                sb.Draw(fedora[indices[4]], mainPicRect, Color.White);
        }

        public static void DrawPrices(SpriteBatch sb, String cash)
        {
            for(int i=0;i<5;i++)
                sb.DrawString(XnaHelper.Font, prices[i], priceSpot[i], Color.Black);
            sb.DrawString(Font1, "$"+cash, moneys, Color.Black);
        }
        #endregion
    }
}
