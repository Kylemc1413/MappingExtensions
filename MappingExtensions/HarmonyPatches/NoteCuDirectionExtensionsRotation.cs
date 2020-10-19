using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(NoteCutDirectionExtensions))]
    [HarmonyPatch("Rotation", MethodType.Normal)]
    class NoteCuDirectionExtensionsRotation
    {
        static void Postfix(NoteCutDirection cutDirection, ref Quaternion __result)
        {
            if (!Plugin.active) return;
            if ( (int)cutDirection >= 1000 && (int)cutDirection <= 1360)
            {
                int angle = 1000 - (int)cutDirection;
                __result = default(Quaternion);
                __result.eulerAngles = new Vector3(0f, 0f, 1000 - (int)cutDirection);
                return;
            }

            if ((int)cutDirection >= 2000 && (int)cutDirection <= 2360)
            {
                int angle = 2000 - (int)cutDirection;
                __result = default(Quaternion);
                __result.eulerAngles = new Vector3(0f, 0f, 2000 - (int)cutDirection);
                return;
            }
        }
    }

    [HarmonyPatch(typeof(NoteCutDirectionExtensions))]
    [HarmonyPatch("Direction", MethodType.Normal)]
    class NoteCuDirectionExtensionsRotationDirection
    {
        static void Postfix(NoteCutDirection cutDirection, ref Vector2 __result)
        {
            if (!Plugin.active) return;
            if ((int)cutDirection >= 1000 && (int)cutDirection <= 1360)
            {
                int angle = 1000 - (int)cutDirection;
                var quaternion = default(Quaternion);
                quaternion.eulerAngles = new Vector3(0f, 0f, 1000 - (int)cutDirection);
                Vector3 dir = quaternion * Vector3.forward;
                __result = new Vector2(dir.x, dir.y);
                Debug.Log(__result);
                return;
            }

            if ((int)cutDirection >= 2000 && (int)cutDirection <= 2360)
            {
                int angle = 2000 - (int)cutDirection;
                var quaternion = default(Quaternion);
                quaternion.eulerAngles = new Vector3(0f, 0f, 2000 - (int)cutDirection);
                Vector3 dir = quaternion * Vector3.forward;
                __result = new Vector2(dir.x, dir.y);
                Debug.Log(__result);
                return;
            }
        }
    }

    [HarmonyPatch(typeof(NoteCutDirectionExtensions))]
    [HarmonyPatch("RotationAngle", MethodType.Normal)]
    class NoteCuDirectionExtensionsRotationRotationAngle
    {
        static void Postfix(NoteCutDirection cutDirection, ref float __result)
        {
            if (!Plugin.active) return;
            if ((int)cutDirection >= 1000 && (int)cutDirection <= 1360)
            {
                int angle = 1000 - (int)cutDirection;
                __result = angle;
                return;
            }

            if ((int)cutDirection >= 2000 && (int)cutDirection <= 2360)
            {
                int angle = 2000 - (int)cutDirection;
                __result = angle;
                return;
            }
        }
    }
}
