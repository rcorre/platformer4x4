using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Platformer.Model;
using Platformer.View;

namespace Platformer.Control
{
    class Shop : GameState
    {
        #region static
        static Texture2D mouseTexture;
        public static void LoadMouse(Texture2D mouseT2D)
        {
            mouseTexture = mouseT2D;
        }
        #endregion

        #region fields
        ProgressData progressData; 
        int[] indices = new int[5]; //revolver,rifle,shotgun,uzi,fedora
        int[] countBought = new int[4];
        int mainPic = 0;
        int lmainPic;
        int locked, gotPrice;
        MouseState pmouseState;
        Rectangle mouse = new Rectangle(0, 0, 15, 15); 
        #endregion

        #region properties
        #endregion

        #region constructor
        public Shop(ProgressData pd)
        {
            SoundPlayer.StopSound();
            SoundPlayer.StartSound("younger");
            for(int i = 0; i<5; i++)
                indices[i] = 0;
            for (int j = 0; j < 4; j++)
                countBought[j] = 0;
            locked = 0;
            lmainPic = 0;
            progressData = pd;
            progressData.shopWeapon = "Revolver";
            progressData.addAmmo = 0;
        }
        #endregion

        #region methods
        public override void Update(GameTime gameTime, InputManager input)
        {
            //TODO -- probably need stuff here
            //where is the mouse, change the picture based on the mouse click
            MouseState mouseState = Mouse.GetState();
            mouse.X = mouseState.X - 5;
            mouse.Y = mouseState.Y - 5;
            mainPic = ShopView.CheckLocation(mouse.X, mouse.Y); //returns != 0 if mouse is hovering over item
            if (pmouseState.LeftButton == ButtonState.Released
                && mouseState.LeftButton == ButtonState.Pressed)
            {
                pmouseState = mouseState;
            }
            if (pmouseState.LeftButton == ButtonState.Pressed
                && mouseState.LeftButton == ButtonState.Released)
            {
                pmouseState = mouseState;
                if (mainPic == 7)
                {
                    //alter progress data to reflect items bought
                    if (countBought[3] > 0) //bought a machine pistol
                    {
                        progressData.shopWeapon = "MachinePistol";
                        countBought[3]--;
                        while (countBought[3] != 0)
                        {
                            progressData.addAmmo++;
                            countBought[3]--;
                        }
                    }
                    else if (countBought[2] > 0) //bought a rifle
                    {
                        progressData.shopWeapon = "Rifle";
                        countBought[2]--;
                        while (countBought[2] != 0)
                        {
                            progressData.addAmmo++;
                            countBought[2]--;
                        }
                    }
                    else if (countBought[1] > 0) //bought a shotgun
                    {
                        progressData.shopWeapon = "Shotgun";
                        countBought[1]--;
                        while (countBought[1] != 0)
                        {
                            progressData.addAmmo++;
                            countBought[1]--;
                        }
                    }
                    else if (countBought[0] > 0) //bought a revolver
                    {
                        progressData.shopWeapon = "Revolver";
                        while (countBought[0] != 0)
                        {
                            progressData.addAmmo++;
                            countBought[0]--;
                        }
                    }
                    SoundPlayer.StopSound();
                    SoundPlayer.StartSound("rosesdepicardie");
                    NewState = new Overworld(progressData);//return to overworld
                }
                else if (mainPic == 6) //attempt to buy
                {
                    locked = 0;
                    gotPrice = ShopView.getPrice(lmainPic - 1);
                    if (progressData.NumCoins >= gotPrice)
                    {
                        progressData.NumCoins -= gotPrice;
                        if (lmainPic > 0)
                            indices[lmainPic - 1] = 1;
                        if (lmainPic == 5)
                            progressData.addHealth++;
                        if ((lmainPic < 5) && (lmainPic > 0))
                            countBought[lmainPic - 1]++;
                    }
                    lmainPic = 0;
                }
                else if ((mainPic > 0) && (mainPic < 6))
                {
                    locked = 1;
                    lmainPic = mainPic;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if(locked == 1)
                ShopView.DrawShop(sb,indices,lmainPic); //draw background of shop
            else
                ShopView.DrawShop(sb,indices,mainPic); //draw background of shop
            sb.Draw(mouseTexture, mouse, Color.Yellow);
            ShopView.DrawPrices(sb,((progressData.NumCoins*100).ToString()));//draw prices, money from progressData
        }
        #endregion
    }
}
