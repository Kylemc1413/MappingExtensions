using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using IPA.Utilities;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapObjectsInTimeRowProcessor), nameof(BeatmapObjectsInTimeRowProcessor.HandleCurrentTimeSliceAllNotesAndSlidersDidFinishTimeSlice))]
    internal class BeatmapObjectsInTimeRowProcessorHandleCurrentTimeSliceAllNotesAndSlidersDidFinishTimeSlice
    {
        // TODO: Remove this when BSIPA gets a fix for missing GameAssemblies.
        private static readonly FieldAccessor<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<BeatmapDataItem>, List<BeatmapDataItem>>.Accessor TimeSliceContainerItemsAccessor =
            FieldAccessor<BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<BeatmapDataItem>, List<BeatmapDataItem>>.GetAccessor("_items");

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // Prevents an IndexOutOfRangeException when using irregular line indexes.
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Callvirt),
                    new CodeMatch(OpCodes.Ldelem_Ref))
                .ThrowIfInvalid("Couldn't match insert condition")
                .Insert(new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Ldc_I4_3),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Mathf), nameof(Mathf.Clamp), new[] { typeof(int), typeof(int), typeof(int) })))
                .InstructionEnumeration();
        }

        private static void Postfix(BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<BeatmapDataItem> allObjectsTimeSlice)
        {
            IEnumerable<NoteData> enumerable = TimeSliceContainerItemsAccessor(ref allObjectsTimeSlice).OfType<NoteData>();
            IEnumerable<SliderData> enumerable2 = TimeSliceContainerItemsAccessor(ref allObjectsTimeSlice).OfType<SliderData>();
            IEnumerable<BeatmapObjectsInTimeRowProcessor.SliderTailData> enumerable3 = TimeSliceContainerItemsAccessor(ref allObjectsTimeSlice).OfType<BeatmapObjectsInTimeRowProcessor.SliderTailData>();
            if (!enumerable.Any(x => x.lineIndex > 3 || x.lineIndex < 0))
            {
                return;
            }
            Dictionary<int, List<NoteData>> notesInColumnsReusableProcessingDictionaryOfLists = new Dictionary<int, List<NoteData>>();
            foreach (NoteData noteData in enumerable)
            {
                if (!notesInColumnsReusableProcessingDictionaryOfLists.ContainsKey(noteData.lineIndex))
                {
                    notesInColumnsReusableProcessingDictionaryOfLists[noteData.lineIndex] = new List<NoteData>(3);
                }
                List<NoteData> list = notesInColumnsReusableProcessingDictionaryOfLists[noteData.lineIndex];
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
            foreach (List<NoteData> list2 in notesInColumnsReusableProcessingDictionaryOfLists.Values)
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
                    if (SliderHeadPositionOverlapsWithNote(sliderData, noteData2))
                    {
                        sliderData.SetHeadBeforeJumpLineLayer(noteData2.beforeJumpNoteLineLayer);
                    }
                }
            }
            foreach (SliderData sliderData2 in enumerable2)
            {
                foreach (SliderData sliderData3 in enumerable2)
                {
                    if (sliderData2 != sliderData3 && SliderHeadPositionOverlapsWithBurstTail(sliderData2, sliderData3))
                    {
                        sliderData2.SetHeadBeforeJumpLineLayer(sliderData3.tailBeforeJumpLineLayer);
                    }
                }
                foreach (BeatmapObjectsInTimeRowProcessor.SliderTailData sliderTailData in enumerable3)
                {
                    if (SliderHeadPositionOverlapsWithBurstTail(sliderData2, sliderTailData.slider))
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
                    if (SliderTailPositionOverlapsWithNote(slider, noteData3))
                    {
                        slider.SetTailBeforeJumpLineLayer(noteData3.beforeJumpNoteLineLayer);
                    }
                }
            }
        }

        private static bool SliderHeadPositionOverlapsWithNote(SliderData slider, NoteData note)
        {
            return slider.headLineIndex == note.lineIndex && slider.headLineLayer == note.noteLineLayer;
        }

        private static bool SliderTailPositionOverlapsWithNote(SliderData slider, NoteData note)
        {
            return slider.tailLineIndex == note.lineIndex && slider.tailLineLayer == note.noteLineLayer;
        }

        private static bool SliderHeadPositionOverlapsWithBurstTail(SliderData slider, SliderData sliderTail)
        {
            return slider.sliderType == SliderData.Type.Normal && sliderTail.sliderType == SliderData.Type.Burst && slider.headLineIndex == sliderTail.tailLineIndex && slider.headLineLayer == sliderTail.tailLineLayer;
        }
    }
}
