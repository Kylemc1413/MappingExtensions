using BeatmapSaveDataVersion3;
using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapSaveData), "SpawnRotationForEventValue")]
    internal class SpawnRotationProcessorRotationForEventValue
    {
        private static void Postfix(int index, ref float __result)
        {
            if (!Plugin.active) return;
            if (BS_Utils.Plugin.LevelData.IsSet && !BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.requires360Movement) return;
            if (index is >= 1000 and <= 1720)
                __result = index - 1360;
        }
    }

    [HarmonyPatch(typeof(BeatmapSaveData), "GetLayerForObstacleType")]
    internal class BeatmapSaveDataGetLayerForObstacleType
    {
        private static void Postfix(BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData.ObstacleType obstacleType, ref int __result)
        {
            if ((int)obstacleType > 3 || (int)obstacleType < 0)
                __result = (int)obstacleType;
        }
    }
}
