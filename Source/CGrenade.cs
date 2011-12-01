//=======================================================================
//
// <copyright file="CGrenade.cs" company="not applicable">
//     Copyright (c) thaCURSEDpie. All rights reserved.
// </copyright>
//
//-----------------------------------------------------------------------
//          File:           CGrenade.cs
//          Version:        Pre-Alpha
//          Part of:        Explosive mod
//          Author:         thaCURSEDpie
//          Date:           September 2011
//          Description:
//              This file contains the CGrenade class,
//              which describes a grenade: a projectile fired from the
//              gun which explodes upon contact.
//
//=======================================================================

namespace Bombit.Source
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GTA;

    public class CGrenade
    {
        private CBomb bomb;

        private GTA.Timer timer;

        private int ptfxHandle;

        private Vector3 startDir;

        private bool firstBoost = true;

        public CGrenade(Vector3 startPos, Vector3 startDirection, EBombType bombType, EExplosionType exType, int bombTickInterval, int grenadeTickInterval)
        {
            this.startDir = startDirection;

            this.bomb = new CBomb(
                                  startPos,
                                  "CJ_PROP_GRENADE",
                                  EBombType.payload,
                                  exType,
                                  bombTickInterval);

            this.timer = new GTA.Timer();
            this.timer.Interval = grenadeTickInterval;
            this.timer.Start();
            this.timer.Tick += new EventHandler(timer_Tick);

            this.bomb.Obj.Collision = true;
            this.bomb.Obj.SetRecordCollisions(true);

            this.ptfxHandle = GTA.Native.Function.Call<int>("START_PTFX_ON_OBJ", "weap_molotov_smoke", this.bomb.Obj, 0.125, -0.02, 0.01, 0.0, 0.0, 0.0, 1.1);

            this.bomb.Obj.FreezePosition = false;
            this.bomb.Obj.Rotation = GTA.Helper.DirectionToRotation(startDirection, 0f);            
        }

        /// <summary>
        /// Gets the rocket's payload.
        /// </summary>
        public CBomb Bomb
        {
            get
            {
                return this.bomb;
            }
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete()
        {
            GTA.Native.Function.Call("REMOVE_PTFX", this.ptfxHandle);

            this.timer.Stop();
            this.bomb.Delete();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.firstBoost)
            {
                if (this.bomb.Obj.Velocity.Length() > 2f)
                {
                    this.firstBoost = false;
                }
                else
                {
                    this.bomb.Obj.Velocity = this.startDir * 60f;
                }
            }

            if (this.bomb.Obj.HasCollidedWithAnything())
            {
                this.bomb.Explode();
                this.Delete();
            }
        }
    }
}
