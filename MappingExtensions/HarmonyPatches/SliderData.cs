using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(SliderData), nameof(SliderData.Mirror))]
    internal class SliderDataMirrorPatch
    {
        private static void Prefix(SliderData __instance, out int[] __state)
        {
            // Directional mirroring of the head and tail are handled properly by NoteCutDirection.Mirrored.
            __state = new[] { __instance.headLineIndex, __instance.tailLineIndex };
        }

        private static void Postfix(SliderData __instance, int[] __state)
        {
            var headLineIndex = __state[0];
            var tailLineIndex = __state[1];

            if (headLineIndex is > 3 or < 0)
            {
                switch (headLineIndex)
                {
                    case >= 1000 or <= -1000:
                    {
                        var leftSide = false;

                        if (headLineIndex <= -1000)
                        {
                            headLineIndex += 2000;
                        }

                        if (headLineIndex >= 4000)
                        {
                            leftSide = true;
                        }

                        headLineIndex = 5000 - headLineIndex;

                        if (leftSide)
                        {
                            headLineIndex -= 2000;
                        }

                        __instance.headLineIndex = headLineIndex;
                        break;
                    }
                    case > 3:
                    {
                        // TODO: Find better naming for that variable.
                        var diff = (headLineIndex - 3) * 2;
                        var newLaneCount = 4 + diff;
                        __instance.headLineIndex = newLaneCount - diff - 1 - headLineIndex;
                        break;
                    }
                    case < 0:
                    {
                        // TODO: Find better naming for that variable.
                        var diff = (0 - headLineIndex) * 2;
                        var newLaneCount = 4 + diff;
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
                        var leftSide = false;

                        if (tailLineIndex <= -1000)
                        {
                            tailLineIndex += 2000;
                        }

                        if (tailLineIndex >= 4000)
                        {
                            leftSide = true;
                        }

                        tailLineIndex = 5000 - tailLineIndex;

                        if (leftSide)
                        {
                            tailLineIndex -= 2000;
                        }

                        __instance.tailLineIndex = tailLineIndex;
                        break;
                    }
                    case > 3:
                    {
                        // TODO: Find better naming for that variable.
                        var diff = (tailLineIndex - 3) * 2;
                        var newLaneCount = 4 + diff;
                        __instance.tailLineIndex = newLaneCount - diff - 1 - tailLineIndex;
                        break;
                    }
                    case < 0:
                    {
                        // TODO: Find better naming for that variable.
                        var diff = (0 - tailLineIndex) * 2;
                        var newLaneCount = 4 + diff;
                        __instance.tailLineIndex = newLaneCount - diff - 1 - tailLineIndex;
                        break;
                    }
                }
            }
        }
    }
}
