using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;
namespace MappingExtensions.Harmony_Patches
{
    [HarmonyPatch(typeof(FlyingScoreSpawner),
          new Type[] {
            typeof(NoteCutInfo),
            typeof(int),
          typeof(int),
        typeof(Vector3),
        typeof(Color),
        typeof(SaberAfterCutSwingRatingCounter) })]
    [HarmonyPatch("SpawnFlyingScore", MethodType.Normal)]

    class FlyingScoreSpawnerSpawnFlyingScore
    {
        static void Prefix(NoteCutInfo noteCutInfo, ref int noteLineIndex, int multiplier, Vector3 pos, Color color, SaberAfterCutSwingRatingCounter saberAfterCutSwingRatingCounter)
        {
            if (!Plugin.active) return;
            if (noteLineIndex < 0)
                noteLineIndex = 0;
            if (noteLineIndex > 3)
                noteLineIndex = 3;
        }
    }
}
