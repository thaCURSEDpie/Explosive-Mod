//=======================================================================
//
// <copyright file="GVars.cs" company="not applicable">
//     Copyright (c) thaCURSEDpie. All rights reserved.
// </copyright>
//
//-----------------------------------------------------------------------
//          File:           GVars.cs
//          Version:        Pre-Alpha
//          Part of:        Explosive mod
//          Author:         thaCURSEDpie
//          Date:           September 2011
//          Description:
//              This file contains the GVars class,
//              which holds the global variables for the mod.
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
    /// This class holds the global variables for the mod.
    /// </summary>
    public static class GVars
    {
        /// <summary>
        /// Variable used to signal to triggered explosives that they should explode.
        /// </summary>
        public static long DetonateTime = -1;

        /// <summary>
        /// An array holding all peds in the game.
        /// </summary>
        public static GTA.Ped[] AllPeds;

        /// <summary>
        /// Variable representing the player ped.
        /// </summary>
        public static GTA.Ped PlayerPed;
    }
}
