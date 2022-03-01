using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(BeatmapObjectSpawnMovementData),
new Type[] {
            typeof(int),
            typeof(NoteLineLayer)
})]
    [HarmonyPatch("GetNoteOffset", MethodType.Normal)]
    class BeatmapObjectSpawnControllerGetNoteOffset
    {
        static void Postfix(BeatmapObjectSpawnMovementData __instance, int noteLineIndex, NoteLineLayer noteLineLayer, ref Vector3 __result, ref int ____noteLinesCount, ref float ____noteLinesDistance, Vector3 ____rightVec)
        {
            if (!Plugin.active) return;
            if (noteLineIndex >= 1000 || noteLineIndex <= -1000)
                {
                    if (noteLineIndex <= -1000)
                        noteLineIndex += 2000;
                    float num = -(____noteLinesCount - 1f) * 0.5f;
                    num = (num + (((float)noteLineIndex) * (____noteLinesDistance / 1000)));
                    __result = ____rightVec * num + new Vector3(0f, __instance.LineYPosForLineLayer(noteLineLayer), 0f);
                }
          //  Plugin.log.Info($"NoteOffset Index {noteLineIndex} Layer {(int)noteLineLayer} Final Result {__result}");

        }





    }
}
