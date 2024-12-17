using HarmonyLib;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(StaticBeatmapObjectSpawnMovementData), nameof(StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer))]
    internal class StaticBeatmapObjectSpawnMovementDataLineYPosForLineLayerPatch
    {
        private static void Postfix(ref float __result, NoteLineLayer lineLayer)
        {
            if (!Plugin.active)
            {
                return;
            }

            const float delta = StaticBeatmapObjectSpawnMovementData.kTopLinesYPos - StaticBeatmapObjectSpawnMovementData.kUpperLinesYPos;
            var layer = (int)lineLayer;
            if (layer is >= 1000 or <= -1000)
            {
                __result = StaticBeatmapObjectSpawnMovementData.kUpperLinesYPos - delta - delta + layer * delta / 1000;
            }
            else if (layer is > 2 or < 0)
            {
                __result = StaticBeatmapObjectSpawnMovementData.kUpperLinesYPos - delta + layer * delta;
            }
        }
    }

    [HarmonyPatch(typeof(StaticBeatmapObjectSpawnMovementData), nameof(StaticBeatmapObjectSpawnMovementData.Get2DNoteOffset))]
    internal class StaticBeatmapObjectSpawnMovementDataGet2DNoteOffsetPatch
    {
        private static void Postfix(ref Vector2 __result, int noteLineIndex, int noteLinesCount, NoteLineLayer noteLineLayer)
        {
            if (!Plugin.active)
            {
                return;
            }

            if (noteLineIndex is >= 1000 or <= -1000)
            {
                if (noteLineIndex <= -1000)
                {
                    noteLineIndex += 2000;
                }

                // TODO: Find a better name for this variable.
                var num = -(noteLinesCount - 1f) * 0.5f;
                var x = num + noteLineIndex * StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance / 1000;
                var y = StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer(noteLineLayer);
                __result = new Vector2(x, y);
            }
        }
    }
}
