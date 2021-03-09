﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
namespace MappingExtensions.Harmony_Patches
{
    [HarmonyPatch(typeof(FlyingScoreSpawner))]
    [HarmonyPatch("SpawnFlyingScore", MethodType.Normal)]

    class FlyingScoreSpawnerSpawnFlyingScore
    {
        static void Prefix(in NoteCutInfo noteCutInfo, ref int noteLineIndex, int multiplier, Vector3 pos, Color color)
        {
            if (!Plugin.active) return;
            if (noteLineIndex < 0)
                noteLineIndex = 0;
            if (noteLineIndex > 3)
                noteLineIndex = 3;
        }
    }
}
