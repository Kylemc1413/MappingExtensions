using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(NoteData), nameof(NoteData.Mirror))]
    internal static class NoteDataMirrorPatch
    {
        private static void Prefix(NoteData __instance, out (int, int) __state)
        {
            __state = (__instance.lineIndex, __instance.flipLineIndex);
        }

        private static void Postfix(NoteData __instance, (int, int) __state)
        {
            var lineIndex = __state.Item1;
            var flipLineIndex = __state.Item2;

            if (lineIndex is > 3 or < 0)
            {
                switch (lineIndex)
                {
                    case >= 1000 or <= -1000:
                    {
                        var leftSide = false;

                        if (lineIndex <= -1000)
                        {
                            lineIndex += 2000;
                        }

                        if (lineIndex >= 4000)
                        {
                            leftSide = true;
                        }

                        lineIndex = 5000 - lineIndex;

                        if (leftSide)
                        {
                            lineIndex -= 2000;
                        }

                        __instance.lineIndex = lineIndex;
                        break;
                    }
                    case > 3:
                    {
                        // TODO: Find better naming for that variable.
                        var diff = (lineIndex - 3) * 2;
                        var newLaneCount = 4 + diff;
                        __instance.lineIndex = newLaneCount - diff - 1 - lineIndex;
                        break;
                    }
                    case < 0:
                    {
                        // TODO: Find better naming for that variable.
                        var diff = (0 - lineIndex) * 2;
                        var newLaneCount = 4 + diff;
                        __instance.lineIndex = newLaneCount - diff - 1 - lineIndex;
                        break;
                    }
                }
            }

            if (flipLineIndex is > 3 or < 0)
            {
                switch (flipLineIndex)
                {
                    case >= 1000 or <= -1000:
                    {
                        var leftSide = false;

                        if (flipLineIndex <= -1000)
                        {
                            flipLineIndex += 2000;
                        }

                        if (flipLineIndex >= 4000)
                        {
                            leftSide = true;
                        }

                        flipLineIndex = 5000 - flipLineIndex;

                        if (leftSide)
                        {
                            flipLineIndex -= 2000;
                        }

                        __instance.flipLineIndex = flipLineIndex;
                        break;
                    }
                    case > 3:
                    {
                        // TODO: Find better naming for that variable.
                        var diff = (flipLineIndex - 3) * 2;
                        var newLaneCount = 4 + diff;
                        __instance.flipLineIndex = newLaneCount - diff - 1 - flipLineIndex;
                        break;
                    }
                    case < 0:
                    {
                        // TODO: Find better naming for that variable.
                        var diff = (0 - flipLineIndex) * 2;
                        var newLaneCount = 4 + diff;
                        __instance.flipLineIndex = newLaneCount - diff - 1 - flipLineIndex;
                        break;
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(ObstacleData), nameof(ObstacleData.Mirror))]
    internal static class ObstacleDataMirrorPatch
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

    [HarmonyPatch(typeof(SliderData), nameof(SliderData.Mirror))]
    internal static class SliderDataMirrorPatch
    {
        private static void Prefix(SliderData __instance, out (int, int) __state)
        {
            // Directional mirroring of the head and tail are handled properly by NoteCutDirection.Mirrored.
            __state = (__instance.headLineIndex, __instance.tailLineIndex);
        }

        private static void Postfix(SliderData __instance, (int, int) __state)
        {
            var headLineIndex = __state.Item1;
            var tailLineIndex = __state.Item2;

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

    [HarmonyPatch(typeof(NoteCutDirectionExtensions), nameof(NoteCutDirectionExtensions.Mirrored))]
    internal static class NoteCutDirectionExtensionsMirroredPatch
    {
        private static void Prefix(out NoteCutDirection __state, NoteCutDirection cutDirection)
        {
            __state = cutDirection;
        }

        private static void Postfix(ref NoteCutDirection __result, NoteCutDirection __state)
        {
            var direction = (int)__state;
            if (direction is >= 1000 and <= 1360)
            {
                var newDirection = 2360 - direction;
                __result = (NoteCutDirection)newDirection;
            }
            else if (direction is >= 2000 and <= 2360)
            {
                var newDirection = 4360 - direction;
                __result = (NoteCutDirection)newDirection;
            }
        }
    }
}
