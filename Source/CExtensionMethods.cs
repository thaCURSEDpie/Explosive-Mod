//=======================================================================
//
// <copyright file="CExtensionMethods.cs" company="not applicable">
//     Copyright (c) thaCURSEDpie. All rights reserved.
// </copyright>
//
//-----------------------------------------------------------------------
//          File:           CExtensionMethods.cs
//          Version:        Pre-Alpha
//          Part of:        Explosive mod
//          Author:         thaCURSEDpie
//          Date:           September 2011
//          Description:
//              This file contains the CExtensionMethods class,
//              which supplies handy extension methods.
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
    /// A class which supplies handy extension methods
    /// </summary>
    public static class CExtensionMethods
    {
        public static void SetRecordCollisions(this GTA.Object obj, bool records)
        {
            GTA.Native.Function.Call("SET_OBJECT_RECORDS_COLLISIONS", obj, records);
        }

        public static bool HasCollidedWithAnything(this GTA.Object obj)
        {
            return GTA.Native.Function.Call<bool>("HAS_OBJECT_COLLIDED_WITH_ANYTHING", obj);
        }

        public static int GetHealth(this GTA.Object obj)
        {
            return GTA.Native.Function.Call<int>("GET_OBJECT_HEALTH", obj);
        }

        /// <summary>
        /// Gets the angle between two 3D vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>A double representing the angle between the two vectors.</returns>
        public static double GetVectorAngle(this Vector3 v1, Vector3 v2)
        {
            return Math.Acos(Vector3.Dot(v1, v2) / (v1.Length() * v2.Length()));
        }
    }
}
