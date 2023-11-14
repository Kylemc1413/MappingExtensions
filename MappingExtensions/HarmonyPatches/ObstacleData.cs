using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(ObstacleData), nameof(ObstacleData.Mirror))]
    internal class ObstacleDataMirror
    {
        private static void Prefix(ObstacleData __instance, out int __state)
        {
            __state = __instance.lineIndex;
        }

        private static void Postfix(ObstacleData __instance, int __state)
        {
            bool precisionWidth = __instance.width is >= 1000 or <= -1000;
            if (__state is <= 3 and >= 0 && !precisionWidth) return;
            if (__state is >= 1000 or <= -1000 || precisionWidth) // precision lineIndex
            {
                int newIndex = __state;
                switch (newIndex)
                {
                    case <= -1000: // normalize index values, we'll fix them later
                        newIndex += 1000;
                        break;
                    case >= 1000:
                        newIndex += -1000;
                        break;
                    default:
                        newIndex *= 1000; // convert lineIndex to precision if not already
                        break;
                }
                newIndex = (newIndex - 2000) * -1 + 2000; //flip lineIndex

                int newWidth = __instance.width; // normalize wall width
                if (newWidth is < 1000 and > -1000)
                {
                    newWidth *= 1000;
                }
                else
                {
                    if(newWidth >= 1000)
                        newWidth -= 1000;
                    if (newWidth <= -1000)
                        newWidth += 1000;
                }
                newIndex -= newWidth;

                if (newIndex < 0) // this is where we fix them
                {
                    newIndex -= 1000;
                }
                else
                {
                    newIndex += 1000;
                }
                __instance.lineIndex = newIndex;
            }
            else // state > -1000 || state < 1000 assumes no precision width
            {
                int mirrorLane = (__state - 2) * -1 + 2; // flip lineIndex
                __instance.lineIndex = mirrorLane - __instance.width; // adjust for wall width
            }
        }
    }
}
