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
                    __result = default;
                    __result.eulerAngles = new Vector3(0f, 0f, 1000 - (int)cutDirection);
                    break;
                case >= 2000 and <= 2360:
                    __result = default;
                    __result.eulerAngles = new Vector3(0f, 0f, 2000 - (int)cutDirection);
                    break;
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
                    Vector3 dir = quaternion * Vector3.down;
                    __result = new Vector2(dir.x, dir.y);
                    break;
                }
                case >= 2000 and <= 2360:
                {
                    var quaternion = default(Quaternion);
                    quaternion.eulerAngles = new Vector3(0f, 0f, 2000 - (int)cutDirection);
                    Vector3 dir = quaternion * Vector3.down;
                    __result = new Vector2(dir.x, dir.y);
                    break;
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
                    __result = 1000 - (int)cutDirection;
                    break;
                case >= 2000 and <= 2360:
                    __result = 2000 - (int)cutDirection;
                    break;
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
            switch ((int)__state)
            {
                case >= 1000 and <= 1360:
                {
                    var cutDir = (int)__state;
                    int newDir = 2360 - cutDir;
                    __result = (NoteCutDirection)newDir;
                    break;
                }
                case >= 2000 and <= 2360:
                {
                    var cutDir = (int)__state;
                    int newDir = 4360 - cutDir;
                    __result = (NoteCutDirection)newDir;
                    break;
                }
            }
        }
    }
}
