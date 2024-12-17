using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(NoteData), nameof(NoteData.Mirror))]
    internal class NoteDataMirrorPatch
    {
        private static void Prefix(NoteData __instance, out int[] __state)
        {
            __state = new[] { __instance.lineIndex, __instance.flipLineIndex };
        }

        private static void Postfix(NoteData __instance, int[] __state)
        {
            var lineIndex = __state[0];
            var flipLineIndex = __state[1];

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
}
