using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(BeatmapObjectSpawnController),
new Type[] {
            typeof(int),
            typeof(NoteLineLayer)
})]
    [HarmonyPatch("GetNoteOffset", MethodType.Normal)]
    class BeatmapObjectSpawnControllerGetNoteOffset
    {
        static void Postfix(BeatmapObjectSpawnController __instance, int noteLineIndex, NoteLineLayer noteLineLayer, ref Vector3 __result, ref float ____noteLinesCount, ref float ____noteLinesDistance)
        {
            if (!Plugin.active) return;
            if (noteLineIndex >= 1000 || noteLineIndex <= -1000)
                {
                    if (noteLineIndex <= -1000)
                        noteLineIndex += 2000;
                    float num = -(____noteLinesCount - 1f) * 0.5f;
                    num = (num + (((float)noteLineIndex) * (____noteLinesDistance / 1000)));
                    __result = __instance.transform.right * num + new Vector3(0f, __instance.LineYPosForLineLayer(noteLineLayer), 0f);
                    return;
                }
        }





    }
}
