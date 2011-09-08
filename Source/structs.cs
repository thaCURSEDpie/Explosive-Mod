//-------------------------------------------------
//-------------------------------------------------
//
//      Filename:           structs.cs
//      Author:             thaCURSEDpie
//      Date of creation:   2010-01-23
//      Description:
//          
//
//-------------------------------------------------
//-------------------------------------------------

using System;
using System.Drawing;
using System.Windows.Forms;
using GTA;

namespace terrorNiko
{
    /// <summary>
    /// Class which handles bombs
    /// </summary>
    public class Bomb
    {
        /// <summary>
        /// Initializer
        /// </summary>
        /// <param name="spawnPosition">Position to spawn the bomb</param>
        /// <param name="typeOfBomb">Type of bomb</param>
        /// <param name="modelName">Modelname to use for the bomb</param>
        /// <param name="canBePickedUp">Can the bomb be picked up?</param>
        public Bomb(Vector3 spawnPosition, bombType typeOfBomb, String modelName, bool canBePickedUp)
        {
            this._bombObject = World.CreateObject(modelName, spawnPosition);
            this._pickupAble = canBePickedUp;
            this._bombType = typeOfBomb;
        }

        // --- properties
        public int armCount = 0;
        public float oldAngle = 0f;
        public Vector3 oldVelocity;

        private bool _hasBeenInitialized = false;
        private bool _pickupAble;
        private bombType _bombType;
        private GTA.Object _bombObject;

        // --- Methods
        /// <summary>
        /// Set up the bomb for use
        /// </summary>
        /// <returns>0 on success, 1 on failure</returns>
        public int init()
        {
            if ((this._bombObject != null) && (this.bombObject.Exists()))
            {
                if (this._pickupAble)
                {
                    GTA.Native.Function.Call("SET_OBJECT_AS_STEALABLE", this._bombObject, 1);
                }

                this.oldVelocity = this._bombObject.Velocity;

                this._hasBeenInitialized = true;
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Explode the bomb
        /// </summary>
        public void explode()
        {
            World.AddExplosion(this._bombObject.Position);
            this._bombObject.Delete();
        }

        /// <summary>
        /// Explode the bomb
        /// </summary>
        /// <param name="explosionType">Type of explosion to use</param>
        /// <param name="power">Explosive power</param>
        public void explode(ExplosionType explosionType, float power)
        {
            World.AddExplosion(this._bombObject.Position, explosionType, power);
            this._bombObject.Delete();
        }

        /// <summary>
        /// Explode the bomb
        /// </summary>
        /// <param name="explosionType">Type of explosion to use</param>
        /// <param name="power">Explosive power</param>
        /// <param name="playSound">Will a sound be played?</param>
        /// <param name="noVisuals">Will there be visuals?</param>
        /// <param name="camShake">How much the cam shakes</param>
        public void explode(ExplosionType explosionType, float power, bool playSound, bool noVisuals, float camShake)
        {
            World.AddExplosion(this._bombObject.Position, explosionType, power, playSound, noVisuals, camShake);
            this._bombObject.Delete();
        }


        //--- Accessors
        public bool hasBeenInitialized
        {
            get
            {
                return this._hasBeenInitialized;
            }
        }

        public bool canBePickedUp
        {
            get
            {
                return this._pickupAble;
            }

            set
            {
                this._pickupAble = value;
            }
        }

        public bombType typeOfBomb
        {
            get
            {
                return this._bombType;
            }
        }

        public GTA.Object bombObject
        {
            get
            {
                return this._bombObject;
            }
        }
    }
}
