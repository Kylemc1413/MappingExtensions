using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(RotationTimeProcessor), nameof(RotationTimeProcessor.SpawnRotationForEventValue))]
    internal class RotationTimeProcessorSpawnRotationForEventValuePatch
    {
        private static void Postfix(ref int __result, int index)
        {
            if (BS_Utils.Plugin.LevelData.IsSet && !BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.beatmapKey.beatmapCharacteristic.requires360Movement)
            {
                return;
            }

            if (index is >= 1000 and <= 1720)
            {
                __result = index - 1360;
            }
        }
    }
}
