using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(SpawnRotationProcessor))]
    [HarmonyPatch("RotationForEventValue", MethodType.Normal)]
    class SpawnRotationProcessorRotationForEventValue
    {
        static void Postfix(int index, ref float __result)
        {
            if (!Plugin.active) return;
            if (BS_Utils.Plugin.LevelData.IsSet && !BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.requires360Movement) return;
            if (index >= 1000 && index <= 1720)
                __result = index - 1360;
        }
    }
}
