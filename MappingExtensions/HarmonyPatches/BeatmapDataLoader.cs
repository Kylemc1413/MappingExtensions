using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapDataLoader), "ConvertNoteLineLayer")]
    internal class BeatmapDataLoaderConvertNoteLineLayer
    {
        private static void Postfix(int layer, ref NoteLineLayer __result)
        {
            if (layer is > 2 or < 0)
                __result = (NoteLineLayer)layer;
        }
    }

    [HarmonyPatch(typeof(BeatmapDataLoader.ObstacleConvertor), "GetNoteLineLayer")]
    internal class BeatmapDataLoaderObstacleConvertorGetNoteLineLayer
    {
        private static void Postfix(int lineLayer, ref NoteLineLayer __result)
        {
            if (lineLayer is > 2 or < 0)
                __result = (NoteLineLayer)lineLayer;
        }
    }
}
