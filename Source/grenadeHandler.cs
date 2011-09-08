using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA;

namespace terrorNiko
{
    class grenadeHandler
    {
        public grenadeHandler(int maxNumGrenades, float shootForce, float maxHeightAboveGround, float maxSpeedChange, float maxAngleChange, int minLifeTime)
        {
            this._shootForce = shootForce;
            this._maxGrenades = maxNumGrenades;
            this._minLifeTime = minLifeTime;
            this._maxHeightAboveGround = maxHeightAboveGround;
            this._maxSpeedChange = maxSpeedChange;
            this._maxAngleChange = maxAngleChange;
            this._grenadeArray = new Bomb[maxNumGrenades];
        }

        //-- Properties
        private int _numGrenades = 0;
        private int _maxGrenades;
        private int _minLifeTime;
        private float _maxHeightAboveGround;
        private float _maxSpeedChange;
        private float _maxAngleChange;
        private float _shootForce;

        public Bomb[] _grenadeArray;

        public void shootGrenade(GTA.Player attacker)
        {

            //-- We make a temp grenade
            Bomb tempGrenade = new Bomb(attacker.Character.GetOffsetPosition(new Vector3(0f, 0.7f, 0.4f)), bombType.grenade_impact, "CJ_PROP_GRENADE", false);

            //-- We wait a while, avoiding errors
            //Wait(0);

            //-- If the grenade has been made successfully
            if (tempGrenade.init() == 0)
            {
                //-- If we have too many grenades, we start afresh
                if (this._numGrenades >= this._maxGrenades)
                {
                    this._numGrenades = 0;
                }

                //-- We get the player direction in 2D (Z ALWAYS EQUALS 0!!)
                Vector3 playerDirection = attacker.Character.Direction;

                //-- We use the current camera dir to fill the Z dimension
                // This is pretty crude, but no other way around it
                playerDirection.Z = Game.CurrentCamera.Direction.Z;

                //-- We add the grenade to the array
                this._grenadeArray[_numGrenades] = tempGrenade;

                //-- We apply force to the grenade
                this._grenadeArray[_numGrenades].bombObject.ApplyForce(this._shootForce * playerDirection);
                this._numGrenades++;

#if DEBUG
                Game.Console.Print("Spawned grenade # " + _numGrenades.ToString() + " player direction: " + playerDirection.ToString());
#endif
            }
        }
        public void handleTick()
        {
            //-- We loop through them
            for (int i = 0; i < this._maxGrenades; i++)
            {
                //-- Try, because grenades sometimes disappear...
                try
                {
                    //-- Check if the grenade exists
                    if ((this._grenadeArray[i] != null) && (this._grenadeArray[i].bombObject.Exists()))
                    {
                        //-- Now we grab the info we need to check for impact
                        //- First we check if the grenade is on the ground
                        float zCoord = this._grenadeArray[i].bombObject.Position.Z;
                        float groundZCoord = this._grenadeArray[i].bombObject.Position.ToGround().Z;
                        float dZ = zCoord - groundZCoord;

                        //- Next we check the speed and angle difference.
                        //-  You can detect a bounce this way because:
                        //-  1. Direction changes
                        //-  2. Speed changes
                        float dSpeed = 0f;
                        float dAngle = 0f;
                        float currentSpeed = this._grenadeArray[i].bombObject.Velocity.Length();
                        Vector3 currentVelocity = this._grenadeArray[i].bombObject.Velocity;

                        //-- We check if the grenade has been initialized.
                        // if not, we ignore him for this tick.
                        if (!this._grenadeArray[i].hasBeenInitialized)
                        {
                            this._grenadeArray[i].oldVelocity = currentVelocity;
                        }
                        else
                        {
                            //-- We now get the angle between the current velocity vector and the old one
                            dAngle = handyFunctions.radToDeg((float)handyFunctions.getAngleBetweenVectors(currentVelocity, this._grenadeArray[i].oldVelocity));

                            //-- We get the speed difference between this tick and the last one
                            dSpeed = currentSpeed - this._grenadeArray[i].oldVelocity.Length();

                            //-- We set the old velocity to the new one
                            this._grenadeArray[i].oldVelocity = currentVelocity;
#if DEBUG
                            Game.Console.Print("dSpeed: " + dSpeed.ToString() + " dAngle: " + dAngle.ToString());
#endif
                        }

                        //-- If the grenade has been alive for long enough, he is allowed to explode
                        if (this._grenadeArray[i].armCount > this._minLifeTime)
                        {
                            //-- If the grenade either:
                            //   1. Is too close to the ground
                            //   2. Has changed its speed too much
                            //   3. Has changed its direction too much
                            //    then he explodes
                            if ((dZ <= this._maxHeightAboveGround) || (dSpeed <= this._maxSpeedChange) || (dAngle >= this._maxAngleChange))
                            {
                                this._grenadeArray[i].explode();
                            }
                        }
                        else
                        {
                            //-- If the grenade is too young, we mature him
                            this._grenadeArray[i].armCount++;
                        }
                    }
                }
                catch (Exception er)
                {
                    //-- We catch possible exceptions here
                    Game.Console.Print("ERROR: data: " + er.Data.ToString() + "\nstacktrace: " + er.StackTrace + "\nsource: " + er.Source);
                }
            }
        }
    }
}
