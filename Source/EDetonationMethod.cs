//=======================================================================
//
// <copyright file="EDetonationMethod.cs" company="not applicable">
//     Copyright (c) thaCURSEDpie. All rights reserved.
// </copyright>
//
//-----------------------------------------------------------------------
//          File:           EDetonationMethod.cs
//          Version:        Pre-Alpha
//          Part of:        Explosive mod
//          Author:         thaCURSEDpie
//          Date:           September 2011
//          Description:
//              This file contains the EDetonationMethod enum,
//              which enumerates the different bomb detonation
//              methods.
//
//=======================================================================

namespace Bombit.Source
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Describes the ways bombs can be detonated.
    /// </summary>
    public enum EDetonationMethod
    {
        /// <summary>
        /// All bombs detonate at once. Note: due to limits in GTA only a certain amount of explosions can be created at a time.
        /// </summary>
        Parallel = 0,

        /// <summary>
        /// The bombs detonate one after the other, with a slight delay in-between.
        /// </summary>
        Serial
    }
}
