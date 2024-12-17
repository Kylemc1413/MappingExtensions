using HarmonyLib;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(SliderMeshController), nameof(SliderMeshController.CutDirectionToControlPointPosition))]
    internal class SliderMeshControllerCutDirectionToControlPointPositionPatch
    {
        private static void Postfix(ref Vector3 __result, NoteCutDirection noteCutDirection)
        {
            if (!Plugin.active)
            {
                return;
            }

            var direction = (int)noteCutDirection;
            if (direction is >= 1000 and <= 1360)
            {
                var quaternion = default(Quaternion);
                quaternion.eulerAngles = new Vector3(0f, 0f, 1000 - direction);
                __result = quaternion * Vector3.down;
            }
            else if (direction is >= 2000 and <= 2360)
            {
                var quaternion = default(Quaternion);
                quaternion.eulerAngles = new Vector3(0f, 0f, 2000 - direction);
                __result = quaternion * Vector3.down;
            }
        }
    }
}
