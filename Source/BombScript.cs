//=======================================================================
//
// <copyright file="BombScript.cs" company="not applicable">
//     Copyright (c) thaCURSEDpie. All rights reserved.
// </copyright>
//
//-----------------------------------------------------------------------
//          File:           Script.cs
//          Version:        Pre-Alpha
//          Part of:        Explosive mod
//          Author:         thaCURSEDpie
//          Date:           September 2011
//          Description:
//              This file contains the BombScript class,
//              which inherits GTA.Script and provides the actual
//              script functionality.
//
//=======================================================================

namespace Bombit.Source
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using GTA;

    /// <summary>
    /// The mod's script.
    /// </summary>
    public class BombScript : Script
    {
        /// <summary>
        /// A list containing all bombes placed by the player.
        /// </summary>
        private List<CBomb> bombs;

        /// <summary>
        /// A counter representing the number of bombs placed.
        /// </summary>
        private int bombCounter;

        /// <summary>
        /// The current bomb type.
        /// </summary>
        private EBombType currentBombType = 0;

        /// <summary>
        /// The current explosion type.
        /// </summary>
        private EExplosionType currentExplosionType = 0;

        /// <summary>
        /// The current detonation method.
        /// </summary>
        private EDetonationMethod detMethod = EDetonationMethod.Parallel;

        /// <summary>
        /// The current projectile type.
        /// </summary>
        private EProjectileType projType = EProjectileType.Rocket;

        /// <summary>
        /// The rocket spawnable and controllable by the player.
        /// </summary>
        private CRocket rocket;
                
        /// <summary>
        /// Whether the rocket is active at the moment.
        /// </summary>
        private bool rocketActive = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="BombScript"/> class.
        /// </summary>
        public BombScript()
        {
            GVars.PlayerPed = Player.Character;
            this.bombs = new List<CBomb>();
            this.bombCounter = 0;

            this.KeyDown += new GTA.KeyEventHandler(this.BombScript_KeyDown);
            this.Tick += new EventHandler(this.BombScript_Tick);

            this.Interval = 500;
        }

        /// <summary>
        /// Handles the Tick event of the BombScript control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BombScript_Tick(object sender, EventArgs e)
        {
            GVars.AllPeds = World.GetAllPeds();

            lock (this.bombs)
            {
                for (int i = 0; i < this.bombs.Count; i++)
                {
                    if (this.bombs[i] != null)
                    {
                        if (this.bombs[i].State == EBombState.Exploded)
                        {
                            this.bombs[i].Delete();
                            Wait(1);
                            this.bombs.RemoveAt(i);
                        }
                    }
                }
            }

            if (this.rocket != null)
            {
                if (this.rocket.State == ERocketState.Done || this.rocket.Bomb.State == EBombState.Exploded)
                {
                    this.rocket.Delete();
                    Wait(1);
                    this.rocket = null;
                    this.rocketActive = false;
                }
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the BombScript control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GTA.KeyEventArgs"/> instance containing the event data.</param>
        private void BombScript_KeyDown(object sender, GTA.KeyEventArgs e)
        {
            if (e.Key == GParams.ProjectileChangeKey)
            {
                switch (this.projType)
                {
                    case EProjectileType.Rocket:
                        this.projType = EProjectileType.Grenade;                        
                        break;

                    case EProjectileType.Grenade:
                        this.projType = EProjectileType.Rocket;

                        break;
                }

                Game.DisplayText("Projectile type: " + this.projType.ToString(), 1800);
            }
            else if (e.Key == GParams.RocketStartKey)
            {
                if (Game.isGameKeyPressed(GameKey.Aim) && !Player.Character.isInVehicle())
                {
                    switch (this.projType)
                    {
                        case EProjectileType.Rocket:
                            if (!this.rocketActive)
                            {
                                this.rocket = new CRocket(Game.CurrentCamera.Position + Game.CurrentCamera.Direction * 3.0f, Game.CurrentCamera.Direction, this.currentBombType, this.currentExplosionType, 5, 0);
                                this.rocketActive = true;
                            }

                            break;

                        case EProjectileType.Grenade:
                            CGrenade tempGrenade = new CGrenade(Game.CurrentCamera.Position + 1.0f * Game.CurrentCamera.Direction, Game.CurrentCamera.Direction, this.currentBombType, this.currentExplosionType, 5, 0);

                            break;
                    }
                }
            }
            else if (e.Key == GParams.BombPlaceKey)
            {
                Vector3 pos = Player.Character.Position;
                
                lock (this.bombs)
                {
                    // Too many bombs: start over
                    if (this.bombCounter > GParams.MaxBombs)
                    {
                        this.bombCounter = 0;

                        if (this.bombs[0] != null)
                        {
                            this.bombs[0].Delete();
                            this.bombs.RemoveAt(0);
                        }
                    }

                    this.bombs.Add(new CBomb(
                                             Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f)).ToGround(),
                                             "EC_BOMB_NE",
                                             this.currentBombType,
                                             this.currentExplosionType,
                                             5));
                    this.bombCounter++;
                }
            }
            else if (e.Key == GParams.BombDetonateKey)
            {
                if (this.detMethod == EDetonationMethod.Parallel)
                {
                    GVars.DetonateTime = System.DateTime.Now.Ticks;
                }
                else if (this.detMethod == EDetonationMethod.Serial)
                {
                    for (int i = 0; i < this.bombs.Count; i++)
                    {
                        if (this.bombs[i].Type == EBombType.manual)
                        {
                            this.bombs[i].Explode();
                            Wait(20);
                            this.bombs[i].Delete();
                            Wait(10);
                        }                        
                    }
                }
            }
            else if (e.Key == GParams.BombChangeTypeKey)
            {
                if ((int)this.currentBombType + 1 < GParams.NumBombTypes)
                {
                    this.currentBombType++;
                }
                else
                {
                    this.currentBombType = 0;
                }

                Game.DisplayText("Bomb type: " + this.currentBombType.ToString(), 1800);
            }
            else if (e.Key == GParams.DetTypeChangeKey)
            {
                if ((int)this.detMethod + 1 < GParams.NumDetTypes)
                {
                    this.detMethod++;
                }
                else
                {
                    this.detMethod = 0;
                }

                Game.DisplayText("Det method: " + this.detMethod.ToString(), 1800);
            }
            else if (e.Key == GParams.ExTypeChangeKey)
            {
                if ((int)this.currentExplosionType + 1 < GParams.NumExTypes)
                {
                    this.currentExplosionType++;
                }
                else
                {
                    this.currentExplosionType = 0;
                }

                Game.DisplayText("Explosion type: " + this.currentExplosionType.ToString(), 1800);
            }
        }
    }
}
