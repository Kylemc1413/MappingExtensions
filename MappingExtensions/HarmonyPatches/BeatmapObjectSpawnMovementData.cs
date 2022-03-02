using HarmonyLib;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapObjectSpawnMovementData), nameof(BeatmapObjectSpawnMovementData.GetNoteOffset))]
    internal class BeatmapObjectSpawnMovementDataGetNoteOffset
    {
        private static void Postfix(BeatmapObjectSpawnMovementData __instance, int noteLineIndex, NoteLineLayer noteLineLayer, ref Vector3 __result, int ____noteLinesCount, float ____noteLinesDistance, Vector3 ____rightVec)
        {
            if (!Plugin.active) return;
            if (noteLineIndex is >= 1000 or <= -1000)
            {
                if (noteLineIndex <= -1000)
                    noteLineIndex += 2000;
                float num = -(____noteLinesCount - 1f) * 0.5f;
                num += noteLineIndex * (____noteLinesDistance / 1000);
                __result = ____rightVec * num + new Vector3(0f, __instance.LineYPosForLineLayer(noteLineLayer), 0f);
            }
        }
    }
    
    [HarmonyPatch(typeof(BeatmapObjectSpawnMovementData), nameof(BeatmapObjectSpawnMovementData.Get2DNoteOffset))]
    internal class BeatmapObjectSpawnMovementDataGet2DNoteOffset
    {
        private static void Postfix(int noteLineIndex, NoteLineLayer noteLineLayer, ref Vector2 __result, BeatmapObjectSpawnMovementData __instance, int ____noteLinesCount, float ____noteLinesDistance)
        {
            if (!Plugin.active) return;
            if (noteLineIndex is >= 1000 or <= -1000)
            {
                if (noteLineIndex <= -1000)
                    noteLineIndex += 2000;
                float num = -(____noteLinesCount - 1f) * 0.5f;
                float x = num + noteLineIndex * (____noteLinesDistance / 1000);
                float y = __instance.LineYPosForLineLayer(noteLineLayer);
                __result = new Vector2(x, y);
            }
        }
    }

    [HarmonyPatch(typeof(BeatmapObjectSpawnMovementData), nameof(BeatmapObjectSpawnMovementData.HighestJumpPosYForLineLayer))]
    internal class BeatmapObjectSpawnMovementDataHighestJumpPosYForLineLayer
    {
        private static void Postfix(NoteLineLayer lineLayer, ref float __result, float ____topLinesHighestJumpPosY, float ____jumpOffsetY, float ____upperLinesHighestJumpPosY)
        {
            if (!Plugin.active) return;
            float delta = ____topLinesHighestJumpPosY - ____upperLinesHighestJumpPosY;
            switch ((int)lineLayer)
            {
                case >= 1000:
                case <= -1000:
                    __result = ____upperLinesHighestJumpPosY - delta - delta + ____jumpOffsetY + (int)lineLayer * (delta / 1000f);
                    break;
                case > 2:
                case < 0:
                    __result = ____upperLinesHighestJumpPosY - delta + ____jumpOffsetY + (int)lineLayer * delta;
                    break;
            }
        }
    }

    [HarmonyPatch(typeof(BeatmapObjectSpawnMovementData), nameof(BeatmapObjectSpawnMovementData.LineYPosForLineLayer))]
    internal class BeatmapObjectSpawnMovementDataLineYPosForLineLayer
    {
        private static void Postfix(NoteLineLayer lineLayer, ref float __result, float ____topLinesYPos, float ____upperLinesYPos)
        {
            if (!Plugin.active) return;
            float delta = ____topLinesYPos - ____upperLinesYPos;
            switch ((int)lineLayer)
            {
                case >= 1000:
                case <= -1000:
                    __result = ____upperLinesYPos - delta - delta + (int)lineLayer * (delta / 1000f);
                    break;
                case > 2:
                case < 0:
                    __result = ____upperLinesYPos - delta + (int)lineLayer * delta;
                    break;
            }
        }
    }
}
