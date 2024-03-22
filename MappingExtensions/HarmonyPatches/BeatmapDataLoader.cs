using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapDataLoaderVersion3.BeatmapDataLoader.ObstacleConverter), nameof(BeatmapDataLoaderVersion3.BeatmapDataLoader.ObstacleConverter.GetNoteLineLayer))]
    internal class BeatmapDataLoaderObstacleConvertorGetNoteLineLayer
    {
        private static void Postfix(int lineLayer, ref NoteLineLayer __result)
        {
            if (lineLayer > 2)
                __result = (NoteLineLayer)lineLayer;
        }
    }

    [HarmonyPatch(typeof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.BasicEventConverter), nameof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.BasicEventConverter.SpawnRotationForEventValue))]
    internal class BeatmapSaveDataSpawnRotationForEventValue
    {
        private static void Postfix(int index, ref float __result)
        {
            if (BS_Utils.Plugin.LevelData.IsSet && !BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.beatmapKey.beatmapCharacteristic.requires360Movement) return;
            if (index is >= 1000 and <= 1720)
                __result = index - 1360;
        }
    }

    [HarmonyPatch(typeof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.ObstacleConverter), nameof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.ObstacleConverter.GetHeightForObstacleType))]
    internal class BeatmapSaveDataGetHeightForObstacleType
    {
        private enum Mode { preciseHeight, preciseHeightStart };

        private static void Postfix(BeatmapSaveDataVersion2_6_0AndEarlier.ObstacleType obstacleType, ref int __result)
        {
            if ((int)obstacleType >= 1000 && (int)obstacleType <= 4000 || (int)obstacleType >= 4001 && (int)obstacleType <= 4005000)
            {
                Mode mode = (int)obstacleType >= 4001 && (int)obstacleType <= 4100000 ? Mode.preciseHeightStart : Mode.preciseHeight;
                int obsHeight;
                var value = (int)obstacleType;
                if(mode == Mode.preciseHeightStart)
                {
                    value -= 4001;
                    obsHeight = value / 1000;
                }
                else
                {
                    obsHeight = value - 1000;
                }
                float height = obsHeight / 1000f * 5f;
                height = height * 1000 + 1000;
                __result = (int)height;
            }
        }
    }

    [HarmonyPatch(typeof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.ObstacleConverter), nameof(BeatmapDataLoaderVersion2_6_0AndEarlier.BeatmapDataLoader.ObstacleConverter.GetLayerForObstacleType))]
    internal class BeatmapSaveDataGetLayerForObstacleType
    {
        private enum Mode { preciseHeight, preciseHeightStart };

        private static void Postfix(BeatmapSaveDataVersion2_6_0AndEarlier.ObstacleType obstacleType, ref int __result)
        {
            if ((int)obstacleType >= 1000 && (int)obstacleType <= 4000 || (int)obstacleType >= 4001 && (int)obstacleType <= 4005000)
            {
                Mode mode = (int)obstacleType >= 4001 && (int)obstacleType <= 4100000 ? Mode.preciseHeightStart : Mode.preciseHeight;
                var startHeight = 0;
                var value = (int)obstacleType;
                if(mode == Mode.preciseHeightStart)
                {
                    value -= 4001;
                    startHeight = value % 1000;
                }
                // Painful math behind this layer calculation logic:
                // https://media.discordapp.net/attachments/864240224400572467/952764516956004372/unknown.png
                // It's probably not 100% accurate but should be enough in most cases.

                //float layer = 6.3333f * startHeight + 833.333f;
                //if (layer >= 1000)
                //{
                //    layer += 446;
                //    layer *= 1.05f;
                //    __result = (int)layer;
                //}
                //else
                //{
                //    __result = 1000;
                //}

                // Alternative Math that is similarly accurate in shape/proportions but has walls being too high.
                float layer = startHeight / 750f * 5f;
                layer = layer * 1000 + 1334;
                __result = (int)layer;
            }
        }
    }
}
