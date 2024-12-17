using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using SongCore.Utilities;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(ObstacleController), nameof(ObstacleController.Init))]
    internal class ObstacleControllerInitPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // Replaces the obstacle width assignment with our own.
            return new CodeMatcher(instructions)
                .MatchEndForward(
                    new CodeMatch(OpCodes.Conv_R4),
                    new CodeMatch(OpCodes.Ldarg_S),
                    new CodeMatch(OpCodes.Mul))
                .ThrowIfInvalid()
                .SetInstruction(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ObstacleControllerInitPatch), nameof(GetObstacleWidth))))
                .InstructionEnumeration();
        }

        private static void Prefix(ObstacleData obstacleData, ref float height)
        {
            if (!Plugin.active)
            {
                return;
            }

            var obstacleHeight = (float)obstacleData.height;
            switch (obstacleHeight)
            {
                case <= -1000:
                    height = (obstacleHeight + 2000) / 1000 * StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance;
                    break;
                case >= 1000:
                    height = (obstacleHeight - 1000) / 1000 * StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance;
                    break;
                case > 2:
                    height = obstacleHeight * StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance;
                    break;
            }
        }

        private static float GetObstacleWidth(float width, float singleLineWidth)
        {
            if (!Plugin.active || !(width is >= 1000 or <= -1000))
            {
                return width * singleLineWidth;
            }

            if (width <= -1000)
            {
                width += 2000;
            }

            return (width - 1000) / 1000 * singleLineWidth;
        }
    }
}
