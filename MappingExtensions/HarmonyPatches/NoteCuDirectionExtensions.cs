using HarmonyLib;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(NoteCutDirectionExtensions), nameof(NoteCutDirectionExtensions.Rotation))]
    internal class NoteCutDirectionExtensionsRotationPatch
    {
        private static void Postfix(ref Quaternion __result, NoteCutDirection cutDirection)
        {
            if (!Plugin.active)
            {
                return;
            }

            var direction = (int)cutDirection;
            if (direction is >= 1000 and <= 1360)
            {
                __result = default;
                __result.eulerAngles = new Vector3(0f, 0f, 1000 - direction);
            }
            else if (direction is >= 2000 and <= 2360)
            {
                __result = default;
                __result.eulerAngles = new Vector3(0f, 0f, 2000 - direction);
            }
        }
    }

    [HarmonyPatch(typeof(NoteCutDirectionExtensions), nameof(NoteCutDirectionExtensions.Direction))]
    internal class NoteCutDirectionExtensionsDirectionPatch
    {
        private static void Postfix(ref Vector2 __result, NoteCutDirection cutDirection)
        {
            if (!Plugin.active)
            {
                return;
            }

            var direction = (int)cutDirection;
            if (direction is >= 1000 and <= 1360)
            {
                var quaternion = default(Quaternion);
                quaternion.eulerAngles = new Vector3(0f, 0f, 1000 - direction);
                var newDirection = quaternion * Vector3.down;
                __result = new Vector2(newDirection.x, newDirection.y);
            }
            else if (direction is >= 2000 and <= 2360)
            {
                var quaternion = default(Quaternion);
                quaternion.eulerAngles = new Vector3(0f, 0f, 2000 - direction);
                var newDirection = quaternion * Vector3.down;
                __result = new Vector2(newDirection.x, newDirection.y);
            }
        }
    }

    [HarmonyPatch(typeof(NoteCutDirectionExtensions), nameof(NoteCutDirectionExtensions.RotationAngle))]
    internal class NoteCutDirectionExtensionsRotationAnglePatch
    {
        private static void Postfix(ref float __result, NoteCutDirection cutDirection)
        {
            if (!Plugin.active)
            {
                return;
            }

            var direction = (int)cutDirection;
            if (direction is >= 1000 and <= 1360)
            {
                __result = 1000 - direction;
            }
            else if (direction is >= 2000 and <= 2360)
            {
                __result = 2000 - direction;
            }
        }
    }

    [HarmonyPatch(typeof(NoteCutDirectionExtensions), nameof(NoteCutDirectionExtensions.Mirrored))]
    internal class NoteCutDirectionExtensionsMirroredPatch
    {
        private static void Prefix(out NoteCutDirection __state, NoteCutDirection cutDirection)
        {
            __state = cutDirection;
        }

        private static void Postfix(ref NoteCutDirection __result, NoteCutDirection __state)
        {
            var direction = (int)__state;
            if (direction is >= 1000 and <= 1360)
            {
                var newDirection = 2360 - direction;
                __result = (NoteCutDirection)newDirection;
            }
            else if (direction is >= 2000 and <= 2360)
            {
                var newDirection = 4360 - direction;
                __result = (NoteCutDirection)newDirection;
            }
        }
    }
}
