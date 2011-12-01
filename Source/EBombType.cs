//=======================================================================
//
// <copyright file="EBombType.cs" company="not applicable">
//     Copyright (c) thaCURSEDpie. All rights reserved.
// </copyright>
//
//-----------------------------------------------------------------------
//          File:           EBombType.cs
//          Version:        Pre-Alpha
//          Part of:        Explosive mod
//          Author:         thaCURSEDpie
//          Date:           September 2011
//          Description:
//              This file contains the EBombType enum,
//              which enumerates the different bomb types.
//
//=======================================================================

namespace Bombit.Source
{
    /// <summary>
    /// An enum which describes the type of bombs.
    /// </summary>
    public enum EBombType
    {
        /// <summary>
        /// Explodes when triggered by the player.
        /// </summary>
        manual = 0,

        /// <summary>
        /// Explodes after a certain amount of time has expired.
        /// </summary>
        timed,

        /// <summary>
        /// Explodes when a target gets too close.
        /// </summary>
        proximity,

        /// <summary>
        /// Payload for a projectile.
        /// </summary>
        payload,
    }
}