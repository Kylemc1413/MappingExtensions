using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapDataLoaderVersion3.BeatmapDataLoader.ObstacleConverter), nameof(BeatmapDataLoaderVersion3.BeatmapDataLoader.ObstacleConverter.GetNoteLineLayer))]
    internal class BeatmapDataLoaderObstacleConverterGetNoteLineLayerPatch
    {
        private static void Postfix(ref NoteLineLayer __result, int lineLayer)
        {
            if (lineLayer > 2)
            {
                __result = (NoteLineLayer)lineLayer;
            }
        }
    }

    [HarmonyPatch(typeof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.BasicEventConverter), nameof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.BasicEventConverter.SpawnRotationForEventValue))]
    internal class BeatmapSaveDataBasicEventConverterSpawnRotationForEventValuePatch
    {
        private static void Postfix(ref float __result, int index)
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

    [HarmonyPatch(typeof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.ObstacleConverter), nameof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.ObstacleConverter.GetHeightForObstacleType))]
    internal class BeatmapDataLoaderObstacleConverterGetHeightForObstacleTypePatch
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
    internal class BeatmapDataLoaderObstacleConverterGetLayerForObstacleTypePatch
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
}
