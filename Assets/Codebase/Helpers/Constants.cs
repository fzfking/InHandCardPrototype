using UnityEngine;

namespace Codebase.Helpers
{
    public static class Constants
    {
        public static class Durations
        {
            public static float PopupAnimationDuration => 0.2f;
            public static float TextAnimationDuration => 1f;
            public static float RotateDuration => 1f;
            public static float PunchDuration => 0.15f;
            public static float ColorSmoothDuration => 0.5f;
        }

        public static Vector3 PunchScaleDefault => Vector3.one * 0.1f;
        public static Vector3 PunchScaleMedium => Vector3.one * 0.5f;
    }
}