using HarmonyLib;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(NoteCutDirectionExtensions), nameof(NoteCutDirectionExtensions.Rotation))]
    internal class NoteCutDirectionExtensionsRotation
    {
        private static void Postfix(NoteCutDirection cutDirection, ref Quaternion __result)
        {
            if (!Plugin.active) return;
            switch ((int)cutDirection)
            {
                case >= 1000 and <= 1360:
                {
                    __result = default;
                    __result.eulerAngles = new Vector3(0f, 0f, 1000 - (int)cutDirection);
                    return;
                }
                case >= 2000 and <= 2360:
                {
                    __result = default;
                    __result.eulerAngles = new Vector3(0f, 0f, 2000 - (int)cutDirection);
                    return;
                }
            }
        }
    }

    [HarmonyPatch(typeof(NoteCutDirectionExtensions), nameof(NoteCutDirectionExtensions.Direction))]
    internal class NoteCutDirectionExtensionsDirection
    {
        private static void Postfix(NoteCutDirection cutDirection, ref Vector2 __result)
        {
            if (!Plugin.active) return;
            switch ((int)cutDirection)
            {
                case >= 1000 and <= 1360:
                {
                    var quaternion = default(Quaternion);
                    quaternion.eulerAngles = new Vector3(0f, 0f, 1000 - (int)cutDirection);
                    Vector3 dir = quaternion * Vector3.forward;
                    __result = new Vector2(dir.x, dir.y);
                    Debug.Log(__result);
                    return;
                }
                case >= 2000 and <= 2360:
                {
                    var quaternion = default(Quaternion);
                    quaternion.eulerAngles = new Vector3(0f, 0f, 2000 - (int)cutDirection);
                    Vector3 dir = quaternion * Vector3.forward;
                    __result = new Vector2(dir.x, dir.y);
                    Debug.Log(__result);
                    return;
                }
            }
        }
    }

    [HarmonyPatch(typeof(NoteCutDirectionExtensions), nameof(NoteCutDirectionExtensions.RotationAngle))]
    internal class NoteCutDirectionExtensionsRotationAngle
    {
        private static void Postfix(NoteCutDirection cutDirection, ref float __result)
        {
            if (!Plugin.active) return;
            switch ((int)cutDirection)
            {
                case >= 1000 and <= 1360:
                {
                    int angle = 1000 - (int)cutDirection;
                    __result = angle;
                    return;
                }
                case >= 2000 and <= 2360:
                {
                    int angle = 2000 - (int)cutDirection;
                    __result = angle;
                    return;
                }
            }
        }
    }

    [HarmonyPatch(typeof(NoteCutDirectionExtensions), nameof(NoteCutDirectionExtensions.Mirrored))]
    internal class NoteCutDirectionExtensionsMirrored
    {
        private static void Prefix(NoteCutDirection cutDirection, out NoteCutDirection __state)
        {
            __state = cutDirection;
        }

        private static void Postfix(ref NoteCutDirection __result, NoteCutDirection __state)
        {
            if (!Plugin.active) return;
            if ((int)__state >= 1000)
            {
                var cutDir = (int)__state;
                int newDir = 2360 - cutDir;
                __result = (NoteCutDirection)newDir;
            }
        }
    }
}
