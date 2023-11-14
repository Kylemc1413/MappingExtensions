using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapObjectsInTimeRowProcessor), nameof(BeatmapObjectsInTimeRowProcessor.HandleCurrentTimeSliceAllNotesAndSlidersDidFinishTimeSlice))]
    internal class BeatmapObjectsInTimeRowProcessorHandleCurrentTimeSliceAllNotesAndSlidersDidFinishTimeSlice
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // Prevents an IndexOutOfRangeException when processing precise line indexes.
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
}
