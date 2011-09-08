//=======================================================================
//
// <copyright file="EExplosionType.cs" company="not applicable">
//     Copyright (c) thaCURSEDpie. All rights reserved.
// </copyright>
//
//-----------------------------------------------------------------------
//          File:           EExplosionType.cs
//          Version:        Pre-Alpha
//          Part of:        Explosive mod
//          Author:         thaCURSEDpie
//          Date:           September 2011
//          Description:
//              This file contains the EExplosionType enum,
//              which enumerates the different type of explosions.
//
//=======================================================================

namespace Bombit.Source
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Describes the various explosion types.
    /// </summary>
    public enum EExplosionType
    {
        /// <summary>
        /// "Default" explosion.
        /// </summary>
        Explosion = 0,

        /// <summary>
        /// Fireball explosion (molotov).
        /// </summary>
        Fire,

        /// <summary>
        /// Explosion which can be customized by the player (HI_OCTANE)
        /// </summary>
        Custom
    }
}
