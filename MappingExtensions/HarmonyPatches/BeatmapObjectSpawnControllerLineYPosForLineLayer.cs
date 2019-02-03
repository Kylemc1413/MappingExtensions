using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(BeatmapObjectSpawnController),
new Type[] {
            typeof(NoteLineLayer)})]
    [HarmonyPatch("LineYPosForLineLayer", MethodType.Normal)]
    class BeatmapObjectSpawnControllerLineYPosForLineLayer
    {
        static void Postfix(NoteLineLayer lineLayer, ref float __result, ref float ____topLinesYPos, ref float ____upperLinesYPos)
        {
            float delta = (____topLinesYPos - ____upperLinesYPos);

            if ((int)lineLayer >= 1000 || (int)lineLayer <= -1000)
            {
                __result = ____upperLinesYPos - delta + (((int)lineLayer) * (delta / 1000f));

                return;
            }

            if ((int)lineLayer > 2)
            {

                __result = ____upperLinesYPos - delta + ((int)lineLayer * delta);
                return;
            }

            if ((int)lineLayer < 0)
            {
                __result = ____upperLinesYPos - delta  + ((int)lineLayer * delta);
                return;
            }

        }

    }
}
