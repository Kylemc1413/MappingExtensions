﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(BeatmapDataObstaclesAndBombsTransform))]
    [HarmonyPatch("CreateTransformedData", MethodType.Normal)]

    class BeatmapDataObstaclesAndBombsTransformCreateTransformedData
    {
        static bool Prefix(BeatmapData beatmapData, GameplayModifiers.EnabledObstacleType enabledObstaclesType, bool noBombs, ref BeatmapData __result)
        {
            BeatmapLineData[] beatmapLinesData = beatmapData.beatmapLinesData;
            int[] array = new int[beatmapLinesData.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 0;
            }
            int num = 0;
            for (int j = 0; j < beatmapLinesData.Length; j++)
            {
                num += beatmapLinesData[j].beatmapObjectsData.Length;
            }
            List<BeatmapObjectData> list = new List<BeatmapObjectData>(num);
            bool flag;
            do
            {
                flag = false;
                float num2 = 999999f;
                int num3 = 0;
                for (int k = 0; k < beatmapLinesData.Length; k++)
                {
                    BeatmapObjectData[] beatmapObjectsData = beatmapLinesData[k].beatmapObjectsData;
                    int num4 = array[k];
                    if (num4 < beatmapObjectsData.Length)
                    {
                        flag = true;
                        float time = beatmapObjectsData[num4].time;
                        if (time < num2)
                        {
                            num2 = time;
                            num3 = k;
                        }
                    }
                }
                if (flag)
                {
                    if (ReflectionUtil.InvokeMethod<bool>(typeof(BeatmapDataObstaclesAndBombsTransform), "ShouldUseBeatmapObject", beatmapLinesData[num3].beatmapObjectsData[array[num3]], enabledObstaclesType, noBombs))
                    {
                        list.Add(beatmapLinesData[num3].beatmapObjectsData[array[num3]].GetCopy());
                    }
                    array[num3]++;
                }
            }
            while (flag);
            int[] array2 = new int[beatmapLinesData.Length];
            for (int l = 0; l < list.Count; l++)
            {
                BeatmapObjectData beatmapObjectData = list[l];
                int numC = beatmapObjectData.lineIndex > 3 ? 3 : beatmapObjectData.lineIndex < 0 ? 0 : beatmapObjectData.lineIndex;
                array2[numC]++;
            }
            BeatmapLineData[] array3 = new BeatmapLineData[beatmapLinesData.Length];
            for (int m = 0; m < beatmapLinesData.Length; m++)
            {
                array3[m] = new BeatmapLineData();
                array3[m].beatmapObjectsData = new BeatmapObjectData[array2[m]];
                array[m] = 0;
            }
            for (int n = 0; n < list.Count; n++)
            {
                BeatmapObjectData beatmapObjectData2 = list[n];
                int lineIndex = beatmapObjectData2.lineIndex > 3 ? 3 : beatmapObjectData2.lineIndex < 0 ? 0 : beatmapObjectData2.lineIndex;
                array3[lineIndex].beatmapObjectsData[array[lineIndex]] = beatmapObjectData2;
                array[lineIndex]++;
            }
            BeatmapEventData[] array4 = new BeatmapEventData[beatmapData.beatmapEventData.Length];
            for (int num5 = 0; num5 < beatmapData.beatmapEventData.Length; num5++)
            {
                BeatmapEventData beatmapEventData = beatmapData.beatmapEventData[num5];
                array4[num5] = beatmapEventData;
            }
            __result = new BeatmapData(array3, array4);
            return false;
        }

        private static float GetRealTimeFromBPMTime(float bmpTime, float beatsPerMinute, float shuffle, float shufflePeriod)
        {
            float num = bmpTime;
            if (shufflePeriod > 0f)
            {
                bool flag = (int)(num * (1f / shufflePeriod)) % 2 == 1;
                if (flag)
                {
                    num += shuffle * shufflePeriod;
                }
            }
            if (beatsPerMinute > 0f)
            {
                num = num / beatsPerMinute * 60f;
            }
            return num;
        }


        private static void ProcessBasicNotesInTimeRow(List<NoteData> notes, float nextRowTime)
        {
            if (notes.Count == 2)
            {
                NoteData noteData = notes[0];
                NoteData noteData2 = notes[1];
                if (noteData.noteType != noteData2.noteType && ((noteData.noteType == NoteType.NoteA && noteData.lineIndex > noteData2.lineIndex) || (noteData.noteType == NoteType.NoteB && noteData.lineIndex < noteData2.lineIndex)))
                {
                    noteData.SetNoteFlipToNote(noteData2);
                    noteData2.SetNoteFlipToNote(noteData);
                }
            }
            for (int i = 0; i < notes.Count; i++)
            {
                notes[i].timeToNextBasicNote = nextRowTime - notes[i].time;
            }
        }
        private static bool ShouldUseBeatmapObject(BeatmapObjectData beatmapObjectData, GameplayModifiers.EnabledObstacleType enabledObstaclesType, bool noBombs)
        {
            if (beatmapObjectData.beatmapObjectType == BeatmapObjectType.Obstacle)
            {
                if (enabledObstaclesType == GameplayModifiers.EnabledObstacleType.NoObstacles)
                {
                    return false;
                }
                if (enabledObstaclesType == GameplayModifiers.EnabledObstacleType.FullHeightOnly)
                {
                    ObstacleData obstacleData = beatmapObjectData as ObstacleData;
                    return obstacleData.obstacleType == ObstacleType.FullHeight;
                }
            }
            else if (beatmapObjectData.beatmapObjectType == BeatmapObjectType.Note)
            {
                NoteData noteData = beatmapObjectData as NoteData;
                if (noteData.noteType == NoteType.Bomb)
                {
                    return !noBombs;
                }
            }
            return true;
        }

        private static void MirrorTransformBeatmapObjects(List<BeatmapObjectData> beatmapObjects, int beatmapLineCount)
        {
            for (int i = 0; i < beatmapObjects.Count; i++)
            {
                BeatmapObjectData beatmapObjectData = beatmapObjects[i];
                beatmapObjectData.MirrorLineIndex(beatmapLineCount);
                if (beatmapObjectData.beatmapObjectType == BeatmapObjectType.Note)
                {
                    NoteData noteData = beatmapObjectData as NoteData;
                    if (noteData != null)
                    {
                        noteData.SwitchNoteType();
                        noteData.MirrorTransformCutDirection();
                    }
                }
            }
        }
    }


}
