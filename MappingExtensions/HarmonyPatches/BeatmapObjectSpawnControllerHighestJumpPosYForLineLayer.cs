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
    [HarmonyPatch("HighestJumpPosYForLineLayer", MethodType.Normal)]
    class BeatmapObjectSpawnControllerHighestJumpPosYForLineLayer
    {
        static void Postfix(NoteLineLayer lineLayer, ref float __result, ref float ____topLinesHighestJumpPosY, ref float ____jumpOffsetY, ref float ____upperLinesHighestJumpPosY)
        {
            if (!Plugin.active) return;
            float delta = (____topLinesHighestJumpPosY - ____upperLinesHighestJumpPosY);

            if ((int)lineLayer >= 1000 || (int)lineLayer <= -1000)
            {
                __result = ____upperLinesHighestJumpPosY - delta - delta + ____jumpOffsetY + (((int)lineLayer) * (delta / 1000f));

                return;
            }

            if ((int)lineLayer > 2)
            {

                __result = ____upperLinesHighestJumpPosY - delta + ____jumpOffsetY + ((int)lineLayer * delta);
                return;
            }

            if ((int)lineLayer < 0)
            {
                __result = ____upperLinesHighestJumpPosY - delta + ____jumpOffsetY + ((int)lineLayer * delta);
                return;
            }

        }

    }
}
