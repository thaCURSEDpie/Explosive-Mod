//=======================================================================
//
// <copyright file="GParams.cs" company="not applicable">
//     Copyright (c) thaCURSEDpie. All rights reserved.
// </copyright>
//
//-----------------------------------------------------------------------
//          File:           GParams.cs
//          Version:        Pre-Alpha
//          Part of:        Explosive mod
//          Author:         thaCURSEDpie
//          Date:           September 2011
//          Description:
//              This file contains the GParams class,
//              which holds the global parameters for the mod.
//
//=======================================================================

namespace Bombit.Source
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// This class holds the global parameters for the mod.
    /// </summary>
    public static class GParams
    {
        /// <summary>
        /// How many bombs can be deployed at a time.
        /// </summary>
        public static int MaxBombs = 400;

        /// <summary>
        /// The number of bomb types available. To be used in conjunction with EBombType.
        /// </summary>
        public static int NumBombTypes = 3;

        /// <summary>
        /// The number of detonation types available. To be used with EDetonationmethod.
        /// </summary>
        public static int NumDetTypes = 2;

        /// <summary>
        /// The number of explosion types available. To be used with EExplosionType.
        /// </summary>
        public static int NumExTypes = 3;

        /// <summary>
        /// The key used to deploy a rocket.
        /// </summary>
        public static Keys RocketStartKey = Keys.G;

        /// <summary>
        /// The key used to place a bomb.
        /// </summary>
        public static Keys BombPlaceKey = Keys.F6;

        /// <summary>
        /// The key used to manually detonate bombs.
        /// </summary>
        public static Keys BombDetonateKey = Keys.F7;

        /// <summary>
        /// The key used to change the type of bombs.
        /// </summary>
        public static Keys BombChangeTypeKey = Keys.F8;

        /// <summary>
        /// The key used to change the type of detonations.
        /// </summary>
        public static Keys DetTypeChangeKey = Keys.F9;

        /// <summary>
        /// The key used to change the type of explosions.
        /// </summary>
        public static Keys ExTypeChangeKey = Keys.F10;

        /// <summary>
        /// The key used to change between different projectiles.
        /// </summary>
        public static Keys ProjectileChangeKey = Keys.F11;

        // 10 000 ticks in a ms
        // 10 000 000 ticks in a second
        /// <summary>
        /// For how many ticks a timed bomb lives.
        /// </summary>
        public static long TimedBombLiveTime = 100000000; // 10 seconds

        /// <summary>
        /// If a ped gets closer than this distance from a proximity mine, the mine explodes.
        /// </summary>
        public static float ProximityMaxDistance = 4.0f;

        /// <summary>
        /// If a vehicle gets closer than this distance from a proximity mine, the mine explodes.
        /// </summary>
        public static float ProximityMaxDistanceVeh = 5.0f;

        /// <summary>
        /// How many ticks to wait between triggering the mine and exploding it.
        /// </summary>
        public static long ProximityExplodeOffset = 44000000;

        /// <summary>
        /// The rocket´s cruising speed.
        /// </summary>
        public static float RocketVel = 50f;

        /// <summary>
        /// If the rocket gets further away from the player than this distance, it explodes.
        /// </summary>
        public static float RocketMaxDist = 300f;

        /// <summary>
        /// The rocket´s direction can only differ this much between ticks. In degrees. TODO: implement this.
        /// </summary>
        public static float RocketMaxAngleDiff = 10f;
    }
}
