using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapObjectSpawnMovementData))]
    [HarmonyPatch("Get2DNoteOffset", MethodType.Normal)]
    class BeatmapObjectSpawnMovementDataGet2DNoteOffset
    {
        static void Postfix(int noteLineIndex, NoteLineLayer noteLineLayer, ref Vector2 __result, ref BeatmapObjectSpawnMovementData __instance, float ____noteLinesCount, float ____noteLinesDistance)
        {
            if (!Plugin.active) return;
            float x, y = 0;
            if (noteLineIndex >= 1000 || noteLineIndex <= -1000)
            {
                if (noteLineIndex <= -1000)
                    noteLineIndex += 2000;
                float num = -(____noteLinesCount - 1f) * 0.5f;
                x = (num + (((float)noteLineIndex) * (____noteLinesDistance / 1000)));
                y = __instance.LineYPosForLineLayer(noteLineLayer);
                __result = new Vector2(x, y);
            }
        }
    }
}
