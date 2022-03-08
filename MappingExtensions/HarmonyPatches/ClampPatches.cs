using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(BeatmapObjectsInTimeRowProcessor), nameof(BeatmapObjectsInTimeRowProcessor.HandleCurrentTimeSliceAllNotesAndSlidersDidFinishTimeSlice))]
    internal class NoteProcessorClampPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Ldloc_S),
                    new CodeMatch(OpCodes.Callvirt),
                    new CodeMatch(OpCodes.Ldelem_Ref))
                .Insert(new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Ldc_I4_3),
                    new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(() => Clamp(0, 0, 0))))
                .InstructionEnumeration();
        }

        private static void Postfix(BeatmapObjectsInTimeRowProcessor.TimeSliceContainer<BeatmapDataItem> allObjectsTimeSlice, int ____numberOfLines)
        {
            IEnumerable<NoteData> enumerable = allObjectsTimeSlice.items.OfType<NoteData>();
            IEnumerable<SliderData> enumerable2 = allObjectsTimeSlice.items.OfType<SliderData>();
            IEnumerable<BeatmapObjectsInTimeRowProcessor.SliderTailData> enumerable3 = allObjectsTimeSlice.items.OfType<BeatmapObjectsInTimeRowProcessor.SliderTailData>();
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
            foreach (List<NoteData> list in notesInColumnsReusableProcessingDictionaryOfLists.Values)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    list[j].SetBeforeJumpNoteLineLayer((NoteLineLayer)j);
                }
            }
            foreach (SliderData sliderData in enumerable2)
            {
                foreach (NoteData noteData2 in enumerable)
                {
                    if (SliderHeadPositionOverlapsWithNote(sliderData, noteData2))
                    {
                        sliderData.SetHasHeadNote(true);
                        sliderData.SetHeadBeforeJumpLineLayer(noteData2.beforeJumpNoteLineLayer);
                        if (sliderData.sliderType == SliderData.Type.Burst)
                        {
                            noteData2.ChangeToBurstSliderHead();
                            if (noteData2.cutDirection == sliderData.tailCutDirection)
                            {
                                Vector2 line = StaticBeatmapObjectSpawnMovementData.Get2DNoteOffset(noteData2.lineIndex, ____numberOfLines, noteData2.noteLineLayer) - StaticBeatmapObjectSpawnMovementData.Get2DNoteOffset(sliderData.tailLineIndex, ____numberOfLines, sliderData.tailLineLayer);
                                float num = noteData2.cutDirection.Direction().SignedAngleToLine(line);
                                if (Mathf.Abs(num) <= 40f)
                                {
                                    noteData2.SetCutDirectionAngleOffset(num);
                                    sliderData.SetCutDirectionAngleOffset(num, num);
                                }
                            }
                        }
                        else
                        {
                            noteData2.ChangeToSliderHead();
                        }
                    }
                }
            }
            foreach (BeatmapObjectsInTimeRowProcessor.SliderTailData sliderTailData in enumerable3)
            {
                SliderData slider = sliderTailData.slider;
                foreach (NoteData noteData3 in enumerable)
                {
                    if (SliderTailPositionOverlapsWithNote(slider, noteData3))
                    {
                        slider.SetHasTailNote(true);
                        slider.SetTailBeforeJumpLineLayer(noteData3.beforeJumpNoteLineLayer);
                        noteData3.ChangeToSliderTail();
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

        private static int Clamp(int input, int min, int max)
        {
            return Math.Min(Math.Max(input, min), max);
        }
    }
}
