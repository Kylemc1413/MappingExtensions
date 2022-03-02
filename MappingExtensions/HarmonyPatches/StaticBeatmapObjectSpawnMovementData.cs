using HarmonyLib;

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
                case >= 1000:
                case <= -1000:
                    __result = StaticBeatmapObjectSpawnMovementData.kUpperLinesYPos - delta - delta + (int)lineLayer * (delta / 1000f);
                    break;
                case > 2:
                case < 0:
                    __result = StaticBeatmapObjectSpawnMovementData.kUpperLinesYPos - delta + (int)lineLayer * delta;
                    break;
            }
        }
    }
}
