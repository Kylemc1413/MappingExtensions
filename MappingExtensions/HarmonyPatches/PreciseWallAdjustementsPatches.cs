using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using SongCore.Utilities;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.ObstacleConverter), nameof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.ObstacleConverter.GetHeightForObstacleType))]
    internal static class BeatmapDataLoaderObstacleConverterGetHeightForObstacleTypePatch
    {
        private enum Mode
        {
            PreciseHeight,
            PreciseHeightStart
        }

        private static void Postfix(ref int __result, BeatmapSaveDataVersion2_6_0AndEarlier.ObstacleType obstacleType)
        {
            var type = (int)obstacleType;

            if (type is < 1000 or > 4005000)
            {
                return;
            }

            int obsHeight;

            var mode = type is >= 4001 and <= 4005000 ? Mode.PreciseHeightStart : Mode.PreciseHeight;
            if (mode == Mode.PreciseHeightStart)
            {
                type -= 4001;
                obsHeight = type / 1000;
            }
            else
            {
                obsHeight = type - 1000;
            }

            __result = (int)(obsHeight / 1000f * 5 * 1000 + 1000);
        }
    }

    [HarmonyPatch(typeof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.ObstacleConverter), nameof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.ObstacleConverter.GetLayerForObstacleType))]
    internal static class BeatmapDataLoaderObstacleConverterGetLayerForObstacleTypePatch
    {
        private enum Mode
        {
            PreciseHeight,
            PreciseHeightStart
        }

        private static void Postfix(ref int __result, BeatmapSaveDataVersion2_6_0AndEarlier.ObstacleType obstacleType)
        {
            var type = (int)obstacleType;

            if (type is < 1000 or > 4005000)
            {
                return;
            }

            var startHeight = 0;

            var mode = type is >= 4001 and <= 4005000 ? Mode.PreciseHeightStart : Mode.PreciseHeight;
            if (mode == Mode.PreciseHeightStart)
            {
                type -= 4001;
                startHeight = type % 1000;
            }

            // Math that is accurate in shape/proportions but has walls being too high.
            __result = (int)(startHeight / 750f * 5 * 1000 + 1334);
        }
    }

    [HarmonyPatch(typeof(BeatmapObjectSpawnMovementData), nameof(BeatmapObjectSpawnMovementData.GetObstacleSpawnData))]
    internal static class BeatmapObjectSpawnMovementDataGetObstacleSpawnDataPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // Replaces the obstacle width.
            return new CodeMatcher(instructions)
                .MatchEndForward(
                    new CodeMatch(i => i.opcode == OpCodes.Callvirt && ((MethodBase)i.operand).Name == $"get_{nameof(ObstacleController.width)}"),
                    new CodeMatch(OpCodes.Conv_R4),
                    new CodeMatch())
                .ThrowIfInvalid()
                .Insert(Transpilers.EmitDelegate<Func<float, float>>(obstacleWidth =>
                {
                    if (!Plugin.active || obstacleWidth is < 1000 and > -1000)
                    {
                        return obstacleWidth;
                    }

                    if (obstacleWidth <= -1000)
                    {
                        obstacleWidth += 2000;
                    }

                    return (obstacleWidth - 1000) / 1000;
                }))
                .InstructionEnumeration();
        }
    }

    [HarmonyPatch(typeof(ObstacleController), nameof(ObstacleController.Init))]
    internal static class ObstacleControllerInitPatch
    {
        private static void Prefix(ObstacleData obstacleData, ref ObstacleSpawnData obstacleSpawnData)
        {
            if (!Plugin.active)
            {
                return;
            }

            var obstacleHeight = obstacleSpawnData.obstacleHeight;

            var height = (float)obstacleData.height;
            switch (height)
            {
                case <= -1000:
                    obstacleHeight = (height + 2000) / 1000;
                    break;
                case >= 1000:
                    obstacleHeight = (height - 1000) / 1000;
                    break;
                case > 2:
                    obstacleHeight = height;
                    break;
            }

            obstacleHeight *= StaticBeatmapObjectSpawnMovementData.layerHeight;

            obstacleSpawnData = new ObstacleSpawnData(obstacleSpawnData.moveOffset, obstacleSpawnData.obstacleWidth, obstacleHeight);
        }
    }
}
