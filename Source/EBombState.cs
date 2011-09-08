//=======================================================================
//
// <copyright file="EBombState.cs" company="not applicable">
//     Copyright (c) thaCURSEDpie. All rights reserved.
// </copyright>
//
//-----------------------------------------------------------------------
//          File:           EBombState.cs
//          Version:        Pre-Alpha
//          Part of:        Explosive mod
//          Author:         thaCURSEDpie
//          Date:           September 2011
//          Description:
//              This file contains the EBombState enum,
//              which enumerates the states a bomb can be in.
//
//=======================================================================

namespace Bombit.Source
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An enum which describes the states a bomb can be in.
    /// </summary>
    public enum EBombState
    {
        /// <summary>
        /// The bomb is armed.
        /// </summary>
        Armed = 0,

        /// <summary>
        /// The bomb has already exploded.
        /// </summary>
        Exploded,

        /// <summary>
        /// The bomb has been triggered.
        /// </summary>
        Triggered,
    }
}
