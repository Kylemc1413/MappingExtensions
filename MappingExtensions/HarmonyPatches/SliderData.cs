using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(SliderData), nameof(SliderData.Mirror))]
    internal class SliderDataMirror
    {
        private static void Prefix(SliderData __instance, out string __state)
        {
            // Directional mirroring of the head and tail are handled properly by NoteCutDirection.Mirrored
            __state = $"{__instance.headLineIndex};{__instance.tailLineIndex}";
        }

        private static void Postfix(SliderData __instance, string __state)
        {
            string[] lineIndexes = __state.Split(';');
            int headLineIndex = int.Parse(lineIndexes[0]);
            int tailLineIndex = int.Parse(lineIndexes[1]);
            if (headLineIndex is > 3 or < 0)
            {
                switch (headLineIndex)
                {
                    case >= 1000 or <= -1000:
                    {
                        int newIndex = headLineIndex;
                        var leftSide = false;
                        if (newIndex <= -1000)
                            newIndex += 2000;

                        if (newIndex >= 4000)
                            leftSide = true;

                        newIndex = 5000 - newIndex;
                        if (leftSide)
                            newIndex -= 2000;

                        __instance.headLineIndex = newIndex;
                        break;
                    }
                    case > 3:
                    {
                        int diff = (headLineIndex - 3) * 2;
                        int newLaneCount = 4 + diff;
                        __instance.headLineIndex = newLaneCount - diff - 1 - headLineIndex;
                        break;
                    }
                    case < 0:
                    {
                        int diff = (0 - headLineIndex) * 2;
                        int newLaneCount = 4 + diff;
                        __instance.headLineIndex = newLaneCount - diff - 1 - headLineIndex;
                        break;
                    }
                }
            }
            if (tailLineIndex is > 3 or < 0)
            {
                switch (tailLineIndex)
                {
                    case >= 1000 or <= -1000:
                    {
                        int newIndex = tailLineIndex;
                        var leftSide = false;
                        if (newIndex <= -1000)
                            newIndex += 2000;

                        if (newIndex >= 4000)
                            leftSide = true;

                        newIndex = 5000 - newIndex;
                        if (leftSide)
                            newIndex -= 2000;

                        __instance.tailLineIndex = newIndex;
                        break;
                    }
                    case > 3:
                    {
                        int diff = (tailLineIndex - 3) * 2;
                        int newLaneCount = 4 + diff;
                        __instance.tailLineIndex = newLaneCount - diff - 1 - tailLineIndex;
                        break;
                    }
                    case < 0:
                    {
                        int diff = (0 - tailLineIndex) * 2;
                        int newLaneCount = 4 + diff;
                        __instance.tailLineIndex = newLaneCount - diff - 1 - tailLineIndex;
                        break;
                    }
                }
            }
        }
    }
}
