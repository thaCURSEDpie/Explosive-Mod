//=======================================================================
//
// <copyright file="CRocket.cs" company="not applicable">
//     Copyright (c) thaCURSEDpie. All rights reserved.
// </copyright>
//
//-----------------------------------------------------------------------
//          File:           CRocket.cs
//          Version:        Pre-Alpha
//          Part of:        Explosive mod
//          Author:         thaCURSEDpie
//          Date:           September 2011
//          Description:
//              This file contains the CRocket class,
//              which represents a controllable rocket.
//
//=======================================================================

namespace Bombit.Source
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GTA;

    /// <summary>
    /// A class representing a rocket.
    /// </summary>
    public class CRocket
    {
        /// <summary>
        /// The rocket's payload.
        /// </summary>
        private CBomb bomb;

        /// <summary>
        /// A timer used to implement tick behaviour.
        /// </summary>
        private GTA.Timer timer;

        /// <summary>
        /// The rocket's cruising speed.
        /// </summary>
        private float vel;

        /// <summary>
        /// The light emitted by the rocket.
        /// </summary>
        private Light light;

        /// <summary>
        /// The current state the rocket is in.
        /// </summary>
        private ERocketState state;

        /// <summary>
        /// Initializes a new instance of the <see cref="CRocket"/> class.
        /// </summary>
        /// <param name="startPos">The start position.</param>
        /// <param name="startDirection">The start direction.</param>
        /// <param name="bombType">Type of payload.</param>
        /// <param name="exType">Type of explosion.</param>
        /// <param name="bombTickInterval">The bomb tick interval.</param>
        /// <param name="rocketTickInterval">The rocket tick interval.</param>
        public CRocket(Vector3 startPos, Vector3 startDirection, EBombType bombType, EExplosionType exType, int bombTickInterval, int rocketTickInterval)
        {
            this.bomb = new CBomb(
                                  startPos,
                                  "cj_rpg_rocket",
                                  bombType,
                                  exType,
                                  bombTickInterval);

            this.timer = new GTA.Timer();
            this.timer.Interval = rocketTickInterval;
            this.timer.Start();
            this.timer.Tick += new EventHandler(this.timer_Tick);

            this.vel = GParams.RocketVel;

            // Example...
            //START_PTFX_ON_OBJ("ambient_cig_smoke", c.a, 0.125, -0.02, 0.01, 0.0, 0.0, 0.0, 1.1) --+z-y+x
            GTA.Native.Function.Call("START_PTFX_ON_OBJ", "weap_rocket_player", this.bomb.Obj, 0.125, -0.02, 0.01, 0.0, 0.0, 0.0, 1.1);

            this.light = new GTA.Light(System.Drawing.Color.Orange, 15.0f, 80f);
            this.light.Position = this.bomb.Obj.GetOffsetPosition(new Vector3(0f, -2f, 0f));
            this.light.Enabled = true;
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
        /// Gets the current state.
        /// </summary>
        public ERocketState State
        {
            get
            {
                return this.state;
            }
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete()
        {
            this.light.Disable();
            this.light = null;
            this.bomb.Delete();
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.bomb == null || this.bomb.Obj == null || !this.bomb.Obj.Exists())
            {
                this.state = ERocketState.Done;
                return;
            }

            // Flying :)
            float distToCam = this.bomb.Obj.Position.DistanceTo(Game.CurrentCamera.Position);

            Vector3 targetPos = Game.CurrentCamera.Position + Game.CurrentCamera.Direction * (distToCam + 9.5f);

            Vector3 targetDir = targetPos - this.bomb.Obj.Position;

            targetDir.Normalize();
           
            this.bomb.Obj.Velocity = targetDir * GParams.RocketVel;
            this.bomb.Obj.Rotation = GTA.Helper.DirectionToRotation(targetDir, 0f);
            
            // this.bomb.Obj.Rotation = new Vector3(this.bomb.Obj.Rotation.X, this.bomb.Obj.Rotation.Y, this.bomb.Obj.Rotation.Z - 90f);

            float speed = this.bomb.Obj.Velocity.Length();
            float distToGround = this.bomb.Obj.Position.Z - this.bomb.Obj.Position.ToGround().Z;

            Game.Console.Print("speed: " + speed.ToString());

            this.light.Position = this.bomb.Obj.GetOffsetPosition(new Vector3(0f, -2f, 0f));
            this.light.Enabled = true;

            if ((speed < 40.0f || distToGround < 0.05f || distToCam > GParams.RocketMaxDist) && (speed != 0f))
            {
                Game.Console.Print("speed: " + speed.ToString() + " dist to ground: " + distToGround.ToString() + " dist to cam: " + distToCam.ToString());
                this.bomb.Explode();
                return;
            }

            // Stopping
            if (!Game.isGameKeyPressed(GameKey.Aim))
            {
                this.state = ERocketState.Done;
            }
        }   
    }
}
