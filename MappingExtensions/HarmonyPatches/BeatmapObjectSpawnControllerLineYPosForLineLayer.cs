using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(BeatmapObjectSpawnMovementData),
new Type[] {
            typeof(NoteLineLayer)})]
    [HarmonyPatch("LineYPosForLineLayer", MethodType.Normal)]
    class BeatmapObjectSpawnControllerLineYPosForLineLayer
    {
        static void Postfix(NoteLineLayer lineLayer, ref float __result, ref float ____topLinesYPos, ref float ____upperLinesYPos)
        {
            if (!Plugin.active) return;
            float delta = (____topLinesYPos - ____upperLinesYPos);

            if ((int)lineLayer >= 1000 || (int)lineLayer <= -1000)
            {
                __result = ____upperLinesYPos - delta - delta + (((int)lineLayer) * (delta / 1000f));
            }
            else if ((int)lineLayer > 2)
            {
                __result = ____upperLinesYPos - delta + ((int)lineLayer * delta);
            }
            else if ((int)lineLayer < 0)
            {
                __result = ____upperLinesYPos - delta + ((int)lineLayer * delta);
            }
       //     Plugin.log.Info($"LineYPos Layer {(int)lineLayer} Final Result {__result}");

        }

    }
}
