using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapTypeConverters), nameof(BeatmapTypeConverters.ConvertNoteLineLayer), typeof(int))]
    internal class BeatmapDataLoaderConvertNoteLineLayer
    {
        private static void Postfix(int layer, ref NoteLineLayer __result)
        {
            if (layer is > 2 or < 0)
                __result = (NoteLineLayer)layer;
        }
    }

    [HarmonyPatch(typeof(BeatmapTypeConverters), nameof(BeatmapTypeConverters.ConvertNoteLineLayer), typeof(BeatmapSaveDataCommon.NoteLineLayer))]
    internal class BeatmapDataLoaderConvertNoteLineLayer2
    {
        private static void Postfix(BeatmapSaveDataCommon.NoteLineLayer layer, ref NoteLineLayer __result)
        {
            if ((int)layer is > 2 or < 0)
                __result = (NoteLineLayer)layer;
        }
    }
}
