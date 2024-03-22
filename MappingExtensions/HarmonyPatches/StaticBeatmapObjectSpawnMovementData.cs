using HarmonyLib;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(StaticBeatmapObjectSpawnMovementData), nameof(StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer))]
    internal class BeatmapObjectSpawnMovementDataLineYPosForLineLayer
    {
        private static void Postfix(NoteLineLayer lineLayer, ref float __result)
        {
            if (!Plugin.active) return;
            const float delta = StaticBeatmapObjectSpawnMovementData.kTopLinesYPos - StaticBeatmapObjectSpawnMovementData.kUpperLinesYPos;
            switch ((int)lineLayer)
            {
                case >= 1000 or <= -1000:
                    __result = StaticBeatmapObjectSpawnMovementData.kUpperLinesYPos - delta - delta + (int)lineLayer * (delta / 1000);
                    break;
                case > 2 or < 0:
                    __result = StaticBeatmapObjectSpawnMovementData.kUpperLinesYPos - delta + (int)lineLayer * delta;
                    break;
            }
        }
    }

    [HarmonyPatch(typeof(StaticBeatmapObjectSpawnMovementData), nameof(StaticBeatmapObjectSpawnMovementData.Get2DNoteOffset))]
    internal class BeatmapObjectSpawnMovementDataGet2DNoteOffset
    {
        private static void Postfix(int noteLineIndex, NoteLineLayer noteLineLayer, ref Vector2 __result, int noteLinesCount)
        {
            if (!Plugin.active) return;
            if (noteLineIndex is >= 1000 or <= -1000)
            {
                if (noteLineIndex <= -1000)
                    noteLineIndex += 2000;
                float num = -(noteLinesCount - 1f) * 0.5f;
                float x = num + noteLineIndex * (StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance / 1000);
                float y = StaticBeatmapObjectSpawnMovementData.LineYPosForLineLayer(noteLineLayer);
                __result = new Vector2(x, y);
            }
        }
    }
}
