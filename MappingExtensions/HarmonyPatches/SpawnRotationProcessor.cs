using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(SpawnRotationProcessor), nameof(SpawnRotationProcessor.RotationForEventValue))]
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
}
