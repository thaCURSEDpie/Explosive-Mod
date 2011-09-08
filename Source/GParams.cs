
namespace Bombit.Source
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    public static class GParams
    {
        public static int MaxBombs = 400;

        public static int NumBombTypes = 3;

        public static int NumDetTypes = 2;

        public static int NumExTypes = 3;

        public static Keys RocketStartKey = Keys.G;

        public static Keys BombPlaceKey = Keys.F6;

        public static Keys BombDetonateKey = Keys.F7;

        public static Keys BombChangeTypeKey = Keys.F8;

        public static Keys DetTypeChangeKey = Keys.F9;

        public static Keys ExTypeChangeKey = Keys.F10;

        // 10 000 ticks in a ms
        // 10 000 000 ticks in a second
        public static long TimedBombLiveTime = 100000000; // 10 seconds

        public static float ProximityMaxDistance = 4.0f;

        public static float ProximityMaxDistanceVeh = 5.0f;

        public static long ProximityExplodeOffset = 44000000;

        public static float CustomExplosionMaxDist = 20f;

        public static float CustomExplosionStrength = 50f;

        public static float VehSearchRange = 250f;

        public static float RocketVel = 50f;

        public static float RocketMaxDist = 300f;

        // in degrees
        public static float RocketMaxAngleDiff = 10f;
    }
}
