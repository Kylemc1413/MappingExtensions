using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapTypeConverters), nameof(BeatmapTypeConverters.ConvertNoteLineLayer), typeof(int))]
    internal class BeatmapTypeConvertersConvertNoteLineLayerPatch
    {
        private static void Postfix(ref NoteLineLayer __result, int layer)
        {
            if (layer is > 2 or < 0)
            {
                __result = (NoteLineLayer)layer;
            }
        }
    }

    [HarmonyPatch(typeof(BeatmapTypeConverters), nameof(BeatmapTypeConverters.ConvertNoteLineLayer), typeof(BeatmapSaveDataCommon.NoteLineLayer))]
    internal class BeatmapTypeConvertersConvertNoteLineLayerPatch2
    {
        private static void Postfix(ref NoteLineLayer __result, BeatmapSaveDataCommon.NoteLineLayer layer)
        {
            if ((int)layer is > 2 or < 0)
            {
                __result = (NoteLineLayer)layer;
            }
        }
    }
}
