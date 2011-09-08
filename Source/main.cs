/**********************************************/
/*      file:           master.cs
 *      part of:        Terror Mod
 *      author:         thaCURSEDpie
 *      creation date:  2010-05-14
 *      
 *      description:
 *          drop & explode bombs.
 *          
 *          
 *          
/*
/**********************************************/

// TODO:
/*
 *   
 *  - No 2 missions at the same time
 *  - GUI
 * 
*/

#define DEBUG

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Media;
using GTA;

namespace Bombit
{
    class bombScript : Script
    {
        int numOfBombs = 0,
            numOfMines = 0,
            maxBombs = 400;
        GTA.Object[] objArray,
                     mineArray;
        Ped[] pedArray;
        Vehicle[] vehArray;

        GTA.Object thrownBomb;
        int playerIsHoldingBombIndex = -1,
            mouseX1 = 0,
            mouseX2 = 1,
            xDifference,
            mouseY1 = 0,
            mouseY2 = 1,
            yDifference,
            pedIsHoldingObject = -1,
            objectBeingThrown = -1,
            bombMode = 0,
            numberOfModes = 2;

        float mouseX,
              mouseY,
              mouseXdif,
              mouseYdif,
              camHeadingY,
              camHeadingX,
              camHeadingZ,
              camX,
              camY,
              camZ,
              lastSpeed = 0;
        bool bombThrown = false,
            once = false;

        Keys bombKey = Keys.F6,
             triggerKey = Keys.F7;

        GTA.Light testLight;

        GTA.Light[] bombLightArray,
                    mineLightArray;

        SoundPlayer tingPlayer;

        public bombScript()
        {
            testLight = new GTA.Light(Color.Red, 3.0f, 50f);
            testLight.Enabled = false;
            Game.Console.Print("BombScript " + Application.ProductVersion.ToString());
            Interval = 0;
            tingPlayer = new SoundPlayer(Properties.Resources.ting1_old1);
            objArray = new GTA.Object[maxBombs];
            mineArray = new GTA.Object[maxBombs];
            bombLightArray = new GTA.Light[maxBombs];
            mineLightArray = new GTA.Light[maxBombs];

            this.KeyDown += new GTA.KeyEventHandler(bombScript_KeyDown);
            this.Tick += new EventHandler(bombScript_Tick);
        }

        void bombScript_Tick(object sender, EventArgs e)
        {
            if (numOfMines > 0)
            {
                //Ped[] tempPedArray = World.GetAllPeds();
                for (int i = 0; i < mineArray.Length; i++)
                {
                    if (Exists(mineArray[i]))
                    {
                        mineLightArray[i].Position = mineArray[i].Position;
                        Ped[] tempPedArray = World.GetPeds(mineArray[i].Position, 3f);
                        if ((tempPedArray.Length > 0) && (mineArray[i].Position.DistanceTo(Player.Character.Position) > 8f))
                        {
                            World.AddExplosion(mineArray[i].Position, ExplosionType.Default, 90f);
                            mineArray[i].Delete();
                            mineLightArray[i].Enabled = false;
                            mineLightArray[i].Disable();
                        }
                    }
                }
            }
            if (numOfBombs > 0)
            {
                for (int i = 0; i < objArray.Length; i++)
                {
                    if (Exists(objArray[i]))
                    {
                        bombLightArray[i].Position = objArray[i].Position;
                    }
                }
                if (objectBeingThrown != -1)
                {
                    float objVelX = objArray[objectBeingThrown].Velocity.X,
                          objVelY = objArray[objectBeingThrown].Velocity.Y,
                          objVelZ = objArray[objectBeingThrown].Velocity.Z;
                    float totalVel = (float)Math.Pow(objVelX * objVelX + objVelY * objVelY + objVelZ * objVelZ, 0.5f);
                    //Game.Console.Print(totalVel.ToString());
                    if (totalVel < 0.7f * lastSpeed)
                    {
                        GTA.Vehicle closestVehicle = World.GetClosestVehicle(objArray[objectBeingThrown].Position, 8f);
                        bool isVehTouchingObj = false;
                        if (Exists(closestVehicle))
                        {
                            isVehTouchingObj = GTA.Native.Function.Call<bool>("IS_VEHICLE_TOUCHING_OBJECT", closestVehicle, objArray[objectBeingThrown]);
                            if (isVehTouchingObj)
                            {
#if DEBUG
                                Game.Console.Print("touching vehicle!");
#endif
                                // float distanceX = closestVehicle.Position.X - objArray[objectBeingThrown].Position.X;
                                // float distanceY = closestVehicle.Position.Y - objArray[objectBeingThrown].Position.Y;
                                // float distanceZ = closestVehicle.Position.Z - objArray[objectBeingThrown].Position.Z;
                                //  float totalDistance = (float)Math.Pow(distanceX * distanceX + distanceY * distanceY + distanceZ * distanceZ, 0.5f);

                                // objArray[objectBeingThrown].Position = new Vector3(objArray[objectBeingThrown].Position.X + distanceX * totalDistance / 10,
                                //                                                  objArray[objectBeingThrown].Position.Y + distanceY * totalDistance / 10,
                                //                                                 objArray[objectBeingThrown].Position.Z + distanceZ * totalDistance / 10);
                                //float offsPosX = -objArray[objectBeingThrown].Position.X + closestVehicle.Position.X;
                                // float offsPosY = -objArray[objectBeingThrown].Position.Y + closestVehicle.Position.Y;
                                //float offsPosZ = -objArray[objectBeingThrown].Position.Z + closestVehicle.Position.Z;
                                Vector3 tempOffsPos = closestVehicle.GetOffset(objArray[objectBeingThrown].Position);

                                GTA.Native.Function.Call("ATTACH_OBJECT_TO_CAR",
                                                         objArray[objectBeingThrown],
                                                         closestVehicle,
                                                         -1,
                                                         tempOffsPos.X,
                                                         tempOffsPos.Y,
                                                         tempOffsPos.Z,
                                                         objArray[objectBeingThrown].Rotation.X,
                                                         objArray[objectBeingThrown].Rotation.Y,
                                                         objArray[objectBeingThrown].Rotation.Z);
#if DEBUG
                                Game.Console.Print("object stickied to vehicle!");
#endif
                                objectBeingThrown = -1;
                            }
                        }

                        if (!isVehTouchingObj)
                        {
                            Ped[] closestPed = World.GetPeds(objArray[objectBeingThrown].Position, 3.0f, 100);

                            bool isTouchingPed = false;

                            for (int i = 0; i < closestPed.Length; i++)
                            {
                                if (GTA.Native.Function.Call<bool>("IS_CHAR_TOUCHING_OBJECT", closestPed[i], objArray[objectBeingThrown]))
                                {
                                    isTouchingPed = true;
                                    break;
                                }
                            }
                            if (!isTouchingPed)
                            {
                                objArray[objectBeingThrown].FreezePosition = true;
                            }
#if DEBUG
                            Game.Console.Print("object stickied!");
#endif
                            objectBeingThrown = -1;
                        }
                    }
                    lastSpeed = totalVel;
                }
                else
                {
                    if (pedIsHoldingObject == -1)
                    {
                        GTA.Object tempObject = GTA.Native.Function.Call<GTA.Object>("GET_OBJECT_PED_IS_HOLDING", Player.Character);

                        if (Exists(tempObject))
                        {
                            Game.Console.Print("1");

                            for (int i = 0; i < objArray.Length; i++)
                            {
                                if (Exists(objArray[i]))
                                {
                                    if (objArray[i] == tempObject)
                                    {
                                        if (pedIsHoldingObject == -1)
                                        {
                                            Game.Console.Print("Player picked up bomb # " + i.ToString());
                                        }
                                        pedIsHoldingObject = i;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        bool isPedHoldingObject = GTA.Native.Function.Call<bool>("IS_PED_HOLDING_AN_OBJECT", Player.Character);
                        if (!isPedHoldingObject)
                        {
                            objectBeingThrown = pedIsHoldingObject;
                            Game.Console.Print("Player threw bomb # " + objectBeingThrown.ToString());
                            pedIsHoldingObject = -1;
                        }
                    }
                }
            }
        }

        void bombScript_KeyDown(object sender, GTA.KeyEventArgs e)
        {
            if (e.Key == Keys.F8)
            {
                bombMode += 1;
                if (bombMode >= numberOfModes)
                {
                    bombMode = 0;
                }
                switch (bombMode)
                {
                    case 0:
                        Game.DisplayText("Triggered explosives");
                        break;
                    case 1:
                        Game.DisplayText("mines");
                        break;
                }
            }
            if (e.Key == bombKey)
            {
                tingPlayer.Play();
                switch (bombMode)
                {
                    case 0:
                        if (Exists(objArray[numOfBombs]))
                        {
                            objArray[numOfBombs].Delete();
                        }
                        objArray[numOfBombs] = World.CreateObject("EC_BOMB_NE"/*"EC_BOMB"*/, Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f)).ToGround());
                        Wait(300);

                        //GTA.Native.Function.Call<GTA.Object>("CREATE_OBJECT", 0xaf6834a4, Player.Character.Position.X + 0.5f, Player.Character.Position.Y, Player.Character.Position.Z, 1);
                        // Wait(50);
                        if (Exists(objArray[numOfBombs]))
                        {
                            objArray[numOfBombs].Rotation = new Vector3(0f, 180f, 0f);
                            bombLightArray[numOfBombs] = new GTA.Light(Color.Blue, 3.0f, 30f, new Vector3(objArray[numOfBombs].Position.X, objArray[numOfBombs].Position.Y, objArray[numOfBombs].Position.Z + 0.9f));
                            bombLightArray[numOfBombs].Enabled = true;

                            GTA.Native.Function.Call("SET_OBJECT_AS_STEALABLE", objArray[numOfBombs], 1);
                            //lightArray[numOfBombs] = new GTA.Light(Color.Blue, 2.0f, 50f, objArray[numOfBombs].Position);
                            //lightArray[numOfBombs].Enabled = true;
                            numOfBombs += 1;
                            if (numOfBombs > maxBombs)
                            {
                                numOfBombs = 0;
                            }

#if DEBUG
                            Game.Console.Print("Spawned bomb! # of bombs: " + numOfBombs.ToString());
#endif

                        }
                        else
                        {
#if DEBUG
                            Game.Console.Print("Failed to spawn bomb!");
#endif
                        }
                        break;
                    case 1:
                        if (Exists(mineArray[numOfMines]))
                        {
                            mineArray[numOfMines].Delete();
                        }
                        mineArray[numOfMines] = World.CreateObject("EC_BOMB", Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f)).ToGround());
                        // Wait(300);
                        //GTA.Native.Function.Call("SET_OBJECT_AS_STEALABLE", objArray[numOfBombs], 1);
                        //GTA.Native.Function.Call<GTA.Object>("CREATE_OBJECT", 0xaf6834a4, Player.Character.Position.X + 0.5f, Player.Character.Position.Y, Player.Character.Position.Z, 1);
                        // Wait(50);
                        if (Exists(mineArray[numOfMines]))
                        {
                            mineArray[numOfMines].Rotation = new Vector3(0f, 180f, 0f);
                            mineLightArray[numOfMines] = new GTA.Light(Color.Red, 3.0f, 30f, new Vector3(mineArray[numOfMines].Position.X, mineArray[numOfMines].Position.Y, mineArray[numOfMines].Position.Z + 0.9f));
                            mineLightArray[numOfMines].Enabled = true;
                            //lightArray[numOfBombs] = new GTA.Light(Color.Blue, 2.0f, 50f, objArray[numOfBombs].Position);
                            //lightArray[numOfBombs].Enabled = true;
                            numOfMines += 1;
                            if (numOfMines > maxBombs)
                            {
                                numOfBombs = 0;
                            }

#if DEBUG
                            Game.Console.Print("Spawned mine! # of mines: " + numOfMines.ToString());
#endif

                        }
                        else
                        {
#if DEBUG
                            Game.Console.Print("Failed to spawn mine!");
#endif
                        }
                        break;
                }



            }

            if (e.Key == triggerKey)
            {
                if (numOfBombs > 0)
                {
                    for (int i = 0; i < numOfBombs; i++)
                    {
                        bombLightArray[i].Enabled = false;
                        bombLightArray[i].Disable();
                        GTA.Native.Function.Call("ADD_EXPLOSION", objArray[i].Position.X,
                                                                  objArray[i].Position.Y,
                                                                  objArray[i].Position.Z,
                                                                  0,
                                                                  160f,
                                                                  true,
                                                                  false,
                                                                  0.5f);
                        //World.AddExplosion(objArray[i].Position, ExplosionType.Default, 160f, true, false, 0.5f);
                        objArray[i].Delete();
                        Wait(10);
                    }
                    numOfBombs = 0;
                }
            }
        }

    }
}
