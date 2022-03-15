using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(ObstacleController), nameof(ObstacleController.Init))]
    internal class ObstacleControllerInit
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Conv_R4),
                    new CodeMatch(OpCodes.Ldarg_S),
                    new CodeMatch(OpCodes.Mul))
                .ThrowIfInvalid("Couldn't match set condition")
                .SetInstruction(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ObstacleControllerInit), nameof(GetObstacleWidth))))
                .InstructionEnumeration();
        }

        private static void Prefix(ObstacleData obstacleData, ref float height)
        {
            if (!Plugin.active) return;
            switch (obstacleData.height)
            {
                case <= -1000:
                    height = ((float)obstacleData.height + 2000) / 1000 * StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance;
                        break;
                case >= 1000:
                    height = ((float)obstacleData.height - 1000) / 1000 * StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance;
                    break;
                case > 2:
                    height = obstacleData.height * StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance;
                    break;
            }
        }

        private static float GetObstacleWidth(float width, float singleLineWidth)
        {
            if (!Plugin.active || !(width >= 1000 || width <= -1000))
                return width * singleLineWidth;
            if (width <= -1000)
                width += 2000;
            return (width - 1000) / 1000 * singleLineWidth;
        }
    }
}
