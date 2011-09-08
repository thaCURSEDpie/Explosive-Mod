//=======================================================================
//
// <copyright file="ERocketState.cs" company="not applicable">
//     Copyright (c) thaCURSEDpie. All rights reserved.
// </copyright>
//
//-----------------------------------------------------------------------
//          File:           ERocketState.cs
//          Version:        Pre-Alpha
//          Part of:        Explosive mod
//          Author:         thaCURSEDpie
//          Date:           September 2011
//          Description:
//              This file contains the ERocketState enum,
//              which enumerates the states a rocket can be in.
//
//=======================================================================

namespace Bombit.Source
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Describes the states a rocket can be in.
    /// </summary>
    public enum ERocketState
    {
        /// <summary>
        /// The rocket is flying.
        /// </summary>
        Flying = 0,

        /// <summary>
        /// The rocket is ready to be deleted.
        /// </summary>
        Done
    }
}
