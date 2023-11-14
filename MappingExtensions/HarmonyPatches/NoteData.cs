using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(NoteData), nameof(NoteData.Mirror))]
    internal class NoteDataMirror
    {
        private static void Prefix(NoteData __instance, out string __state)
        {
            __state = $"{__instance.lineIndex};{__instance.flipLineIndex}";
        }

        private static void Postfix(NoteData __instance, string __state)
        {
            string[] lineIndexes = __state.Split(';');
            int lineIndex = int.Parse(lineIndexes[0]);
            int flipLineIndex = int.Parse(lineIndexes[1]);
            if (lineIndex is > 3 or < 0)
            {
                switch (lineIndex)
                {
                    case >= 1000 or <= -1000:
                    {
                        int newIndex = lineIndex;
                        var leftSide = false;
                        if (newIndex <= -1000)
                            newIndex += 2000;

                        if (newIndex >= 4000)
                            leftSide = true;

                        newIndex = 5000 - newIndex;
                        if (leftSide)
                            newIndex -= 2000;

                        __instance.lineIndex = newIndex;
                        break;
                    }
                    case > 3:
                    {
                        int diff = (lineIndex - 3) * 2;
                        int newLaneCount = 4 + diff;
                        __instance.lineIndex = newLaneCount - diff - 1 - lineIndex;
                        break;
                    }
                    case < 0:
                    {
                        int diff = (0 - lineIndex) * 2;
                        int newLaneCount = 4 + diff;
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
                        int newIndex = flipLineIndex;
                        var leftSide = false;
                        if (newIndex <= -1000)
                            newIndex += 2000;

                        if (newIndex >= 4000)
                            leftSide = true;

                        newIndex = 5000 - newIndex;
                        if (leftSide)
                            newIndex -= 2000;

                        __instance.flipLineIndex = newIndex;
                        break;
                    }
                    case > 3:
                    {
                        int diff = (flipLineIndex - 3) * 2;
                        int newLaneCount = 4 + diff;
                        __instance.flipLineIndex = newLaneCount - diff - 1 - flipLineIndex;
                        break;
                    }
                    case < 0:
                    {
                        int diff = (0 - flipLineIndex) * 2;
                        int newLaneCount = 4 + diff;
                        __instance.flipLineIndex = newLaneCount - diff - 1 - flipLineIndex;
                        break;
                    }
                }
            }
        }
    }
}
