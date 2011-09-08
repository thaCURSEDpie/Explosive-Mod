//=======================================================================
//
// <copyright file="CBomb.cs" company="not applicable">
//     Copyright (c) thaCURSEDpie. All rights reserved.
// </copyright>
//
//-----------------------------------------------------------------------
//          File:           CBomb.cs
//          Version:        Pre-Alpha
//          Part of:        Explosive mod
//          Author:         thaCURSEDpie
//          Date:           September 2011
//          Description:
//              This file contains the CBomb class,
//              which describes a bomb.
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
    /// A class which describes bombs.
    /// </summary>
    public class CBomb
    {
        /// <summary>
        /// Timer used to provide tick functionality.
        /// </summary>
        private GTA.Timer timer;

        /// <summary>
        /// Which model to use for the bomb.
        /// </summary>
        private Model model;

        /// <summary>
        /// The in-game representation of the bomb.
        /// </summary>
        private GTA.Object obj;

        /// <summary>
        /// The type of bomb.
        /// </summary>
        private EBombType bombType;

        /// <summary>
        /// The state the bomb is in.
        /// </summary>
        private EBombState state;

        /// <summary>
        /// Tick number at which the bomb was created.
        /// </summary>
        private long creationTime;

        /// <summary>
        /// At which tick number to explode the bomb.
        /// </summary>
        private long explosionTime;

        /// <summary>
        /// The bomb's explosion type.
        /// </summary>
        private EExplosionType exType;       

        /// <summary>
        /// Initializes a new instance of the <see cref="CBomb"/> class.
        /// </summary>
        /// <param name="position">The spawn position.</param>
        /// <param name="model">The model.</param>
        /// <param name="bombType">The bomb type.</param>
        /// <param name="explosionType">The explosion type.</param>
        /// <param name="tickInterval">The tick interval.</param>
        public CBomb(Vector3 position, Model model, EBombType bombType, EExplosionType explosionType, int tickInterval)
        {
            this.model = model;
            this.bombType = bombType;

            this.obj = World.CreateObject(model, position);
            this.timer = new GTA.Timer(tickInterval, true);
            this.timer.Tick += new EventHandler(this.timer_Tick);

            this.creationTime = System.DateTime.Now.Ticks;
            this.state = EBombState.Armed;
            this.exType = explosionType;

            // We wait until the object has been created
            while (this.obj == null)
            {
            }

            GTA.Native.Function.Call("SET_OBJECT_AS_STEALABLE", this.obj, 1);
            //START_PTFX_ON_OBJ("ambient_cig_smoke", c.a, 0.125, -0.02, 0.01, 0.0, 0.0, 0.0, 1.1) --+z-y+x
            //ptfxHandle = GTA.Native.Function.Call<int>("START_PTFX_ON_OBJ", "shot_directed_flame", this.obj, 0, 0, 0.3, 45.0, -90.0, 45.0, 0.0);
            //
            // START_PTFX_ON_OBJ(string ptfxName, int objId, float xOffset, float yOffset, float zOffset, float yaw, float pitch, float roll, float scale)
        }

        /// <summary>
        /// Gets the associated GTA.Object.
        /// </summary>
        public GTA.Object Obj
        {
            get
            {
                return this.obj;
            }
        }

        /// <summary>
        /// Gets the bomb type.
        /// </summary>
        public EBombType Type
        {
            get
            {
                return this.bombType;
            }
        }

        /// <summary>
        /// Gets the current state the bomb is in.
        /// </summary>
        public EBombState State
        {
            get
            {
                return this.state;
            }
        }

        /// <summary>
        /// Explodes the bomb.
        /// </summary>
        public void Explode()
        {
            if (this.obj == null || !this.obj.Exists())
            {
                this.state = EBombState.Exploded;
                return;
            }

            switch (this.exType)
            {
                case EExplosionType.Explosion:
                    World.AddExplosion(this.obj.Position, ExplosionType.Default, 50f, true, false, 0f);                   

                    break;

                case EExplosionType.Fire:
                    World.AddExplosion(this.obj.Position, ExplosionType.Molotov, 50f, true, false, 0f);

                    break;

                case EExplosionType.Custom:
                    GTA.Native.Function.Call("ADD_EXPLOSION", this.obj.Position.X, this.obj.Position.Y, this.obj.Position.Z, 3, 7f, 1, 0, 0f);

                    break;
            }

            this.state = EBombState.Exploded;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete()
        {
            this.obj.Delete();
        }

        /// <summary>
        /// Handles the Tick event of the timer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.obj == null
                || !this.obj.Exists())
            {
                this.state = EBombState.Exploded;
                return;
            }

            if (this.state == EBombState.Exploded)
            {
                return;
            }

            lock (this.obj)
            {
                switch (this.bombType)
                {
                    //--- Manual triggering
                    //---------------------
                    case EBombType.manual:
                        if (this.state == EBombState.Armed)
                        {
                            // If the player wants us to explode
                            if (GVars.DetonateTime > this.creationTime &&
                                System.DateTime.Now.Ticks > GVars.DetonateTime)
                            {
                                this.Explode();
                            }
                        }

                        break;

                    //--- Timed triggering
                    //---------------------
                    case EBombType.timed:
                        if (this.state == EBombState.Armed)
                        {
                            if (this.creationTime + GParams.TimedBombLiveTime <= System.DateTime.Now.Ticks)
                            {
                                Game.Console.Print("current time: " + System.DateTime.Now.Ticks.ToString());
                                Game.Console.Print("creation time: " + this.creationTime.ToString());
                                Game.Console.Print("cr time + live time: " + (this.creationTime + GParams.TimedBombLiveTime).ToString());
                                this.Explode();
                            }
                        }

                        break;

                    //--- Proximity triggering
                    //---------------------
                    case EBombType.proximity:
                        if (this.state == EBombState.Armed)
                        {
                            Game.Console.Print("armed!");
                            for (int i = 0; i < GVars.AllPeds.Length; i++)
                            {
                                if (i >= GVars.AllPeds.Length)
                                {
                                    break;
                                }

                                if (GVars.AllPeds[i] != null &&
                                    GVars.AllPeds[i].Exists() &&
                                    GVars.AllPeds[i].isAlive)
                                {
                                    if (GVars.AllPeds[i].isInVehicle() &&
                                        GVars.AllPeds[i].CurrentVehicle.Position.DistanceTo(this.obj.Position) <= GParams.ProximityMaxDistanceVeh)
                                    {
                                        this.state = EBombState.Triggered;
                                        this.explosionTime = this.creationTime + GParams.ProximityExplodeOffset;

                                        return;
                                    }
                                    else if (GVars.AllPeds[i] != GVars.PlayerPed &&
                                            this.obj.Position.DistanceTo(GVars.AllPeds[i].Position) <= GParams.ProximityMaxDistance)
                                    {
                                        this.state = EBombState.Triggered;
                                        this.explosionTime = this.creationTime + GParams.ProximityExplodeOffset;

                                        return;
                                    }
                                }
                            }
                        }
                        else if (this.state == EBombState.Triggered)
                        {
                            this.Explode();
                        }

                        break;
                }
            }
        }
    }
}
