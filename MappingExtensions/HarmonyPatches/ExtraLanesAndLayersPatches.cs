using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using SongCore.Utilities;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapDataLoaderVersion3.BeatmapDataLoader.ObstacleConverter), nameof(BeatmapDataLoaderVersion3.BeatmapDataLoader.ObstacleConverter.GetNoteLineLayer))]
    internal static class BeatmapDataLoaderObstacleConverterGetNoteLineLayerPatch
    {
        private static void Postfix(ref NoteLineLayer __result, int lineLayer)
        {
            if (lineLayer > 2)
            {
                __result = (NoteLineLayer)lineLayer;
            }
        }
    }

    [HarmonyPatch(typeof(BeatmapTypeConverters), nameof(BeatmapTypeConverters.ConvertNoteLineLayer), typeof(int))]
    internal static class BeatmapTypeConvertersConvertNoteLineLayerPatch
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
    internal static class BeatmapTypeConvertersConvertNoteLineLayerPatch2
    {
        private static void Postfix(ref NoteLineLayer __result, BeatmapSaveDataCommon.NoteLineLayer layer)
        {
            if ((int)layer is > 2 or < 0)
            {
                __result = (NoteLineLayer)layer;
            }
        }
    }

    [HarmonyPatch(typeof(BeatmapObjectsInTimeRowProcessor), nameof(BeatmapObjectsInTimeRowProcessor.HandleCurrentTimeSliceAllNotesAndSlidersDidFinishTimeSlice))]
    internal static class BeatmapObjectsInTimeRowProcessorHandleCurrentTimeSliceAllNotesAndSlidersDidFinishTimeSlicePatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // Prevents an IndexOutOfRangeException when processing precise line indexes.
            return new CodeMatcher(instructions)
                .MatchEndForward(
                    new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Callvirt),
                    new CodeMatch(OpCodes.Ldelem_Ref))
                .ThrowIfInvalid()
                .Insert(
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Ldc_I4_3),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Math), nameof(Math.Clamp), new[] { typeof(int), typeof(int), typeof(int) })))
                .InstructionEnumeration();
        }

        // TODO: Make this less compiler-generated garbage.
        private static void Postfix(BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<BeatmapDataItem> allObjectsTimeSlice)
        {
            IEnumerable<NoteData> enumerable = allObjectsTimeSlice.items.OfType<NoteData>();
            if (!enumerable.Any(x => x.lineIndex is > 3 or < 0))
            {
                return;
            }
            IEnumerable<SliderData> enumerable2 = allObjectsTimeSlice.items.OfType<SliderData>();
            IEnumerable<BeatmapObjectsInTimeRowProcessor.SliderTailData> enumerable3 = allObjectsTimeSlice.items.OfType<BeatmapObjectsInTimeRowProcessor.SliderTailData>();
            Dictionary<int, List<NoteData>> notesInColumnsProcessingDictionaryOfLists = new Dictionary<int, List<NoteData>>();
            foreach (NoteData noteData in enumerable)
            {
                if (!notesInColumnsProcessingDictionaryOfLists.ContainsKey(noteData.lineIndex))
                {
                    notesInColumnsProcessingDictionaryOfLists[noteData.lineIndex] = new List<NoteData>(3);
                }
                List<NoteData> list = notesInColumnsProcessingDictionaryOfLists[noteData.lineIndex];
                bool flag = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].noteLineLayer > noteData.noteLineLayer)
                    {
                        list.Insert(i, noteData);
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    list.Add(noteData);
                }
            }
            foreach (List<NoteData> list2 in notesInColumnsProcessingDictionaryOfLists.Values)
            {
                for (int j = 0; j < list2.Count; j++)
                {
                    list2[j].SetBeforeJumpNoteLineLayer((NoteLineLayer)j);
                }
            }
            foreach (SliderData sliderData in enumerable2)
            {
                foreach (NoteData noteData2 in enumerable)
                {
                    if (BeatmapObjectsInTimeRowProcessor.SliderHeadPositionOverlapsWithNote(sliderData, noteData2))
                    {
                        sliderData.SetHeadBeforeJumpLineLayer(noteData2.beforeJumpNoteLineLayer);
                    }
                }
            }
            foreach (SliderData sliderData2 in enumerable2)
            {
                foreach (SliderData sliderData3 in enumerable2)
                {
                    if (sliderData2 != sliderData3 && BeatmapObjectsInTimeRowProcessor.SliderHeadPositionOverlapsWithBurstTail(sliderData2, sliderData3))
                    {
                        sliderData2.SetHeadBeforeJumpLineLayer(sliderData3.tailBeforeJumpLineLayer);
                    }
                }
                foreach (BeatmapObjectsInTimeRowProcessor.SliderTailData sliderTailData in enumerable3)
                {
                    if (BeatmapObjectsInTimeRowProcessor.SliderHeadPositionOverlapsWithBurstTail(sliderData2, sliderTailData.slider))
                    {
                        sliderData2.SetHeadBeforeJumpLineLayer(sliderTailData.slider.tailBeforeJumpLineLayer);
                    }
                }
            }
            foreach (BeatmapObjectsInTimeRowProcessor.SliderTailData sliderTailData2 in enumerable3)
            {
                SliderData slider = sliderTailData2.slider;
                foreach (NoteData noteData3 in enumerable)
                {
                    if (BeatmapObjectsInTimeRowProcessor.SliderTailPositionOverlapsWithNote(slider, noteData3))
                    {
                        slider.SetTailBeforeJumpLineLayer(noteData3.beforeJumpNoteLineLayer);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(NoteJump), nameof(NoteJump.Init))]
    internal static class NoteJumpInitPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // Prevents an IndexOutOfRangeException when using negative line indexes/layers.
            return new CodeMatcher(instructions)
                .MatchStartForward(
                    new CodeMatch(OpCodes.Ldelem),
                    new CodeMatch(OpCodes.Ldc_R4),
                    new CodeMatch(OpCodes.Call))
                .ThrowIfInvalid()
                .Insert(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Math), nameof(Math.Abs), new[] { typeof(int) })))
                .InstructionEnumeration();
        }
    }
}
