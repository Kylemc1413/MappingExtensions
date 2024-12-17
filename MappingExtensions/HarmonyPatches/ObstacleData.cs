using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(ObstacleData), nameof(ObstacleData.Mirror))]
    internal class ObstacleDataMirrorPatch
    {
        private static void Prefix(ObstacleData __instance, out int __state)
        {
            __state = __instance.lineIndex;
        }

        private static void Postfix(ObstacleData __instance, int __state)
        {
            var lineIndex = __state;
            var obstacleWidth = __instance.width;
            var precisionWidth = obstacleWidth is >= 1000 or <= -1000;

            if (lineIndex is <= 3 and >= 0 && !precisionWidth)
            {
                return;
            }

            if (lineIndex is >= 1000 or <= -1000 || precisionWidth) // precision lineIndex
            {
                switch (lineIndex)
                {
                    case <= -1000: // normalize index values, we'll fix them later
                        lineIndex += 1000;
                        break;
                    case >= 1000:
                        lineIndex += -1000;
                        break;
                    default:
                        lineIndex *= 1000; // convert lineIndex to precision if not already
                        break;
                }

                lineIndex = (lineIndex - 2000) * -1 + 2000; //flip lineIndex

                if (obstacleWidth is < 1000 and > -1000) // normalize wall width
                {
                    obstacleWidth *= 1000;
                }
                else
                {
                    if (obstacleWidth >= 1000)
                    {
                        obstacleWidth -= 1000;
                    }

                    if (obstacleWidth <= -1000)
                    {
                        obstacleWidth += 1000;
                    }
                }

                lineIndex -= obstacleWidth;

                if (lineIndex < 0) // this is where we fix them
                {
                    lineIndex -= 1000;
                }
                else
                {
                    lineIndex += 1000;
                }

                __instance.lineIndex = lineIndex;
            }
            else // lineIndex > -1000 || lineIndex < 1000 assumes no precision width
            {
                var mirrorLane = (lineIndex - 2) * -1 + 2; // flip lineIndex
                __instance.lineIndex = mirrorLane - obstacleWidth; // adjust for wall width
            }
        }
    }
}
