using HarmonyLib;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(SliderMeshController), "CutDirectionToControlPointPosition")]
    internal class SliderMeshControllerCutDirectionToControlPointPosition
    {
        private static void Postfix(NoteCutDirection noteCutDirection, ref Vector3 __result)
        {
            if (!Plugin.active) return;
            switch ((int)noteCutDirection)
            {
                case >= 1000 and <= 1360:
                {
                    var quaternion = default(Quaternion);
                    quaternion.eulerAngles = new Vector3(0f, 0f, 1000 - (int)noteCutDirection);
                    __result = quaternion * Vector3.down;
                    break;
                }
                case >= 2000 and <= 2360:
                {
                    var quaternion = default(Quaternion);
                    quaternion.eulerAngles = new Vector3(0f, 0f, 2000 - (int)noteCutDirection);
                    __result = quaternion * Vector3.down;
                    break;
                }
            }
        }
    }
}
