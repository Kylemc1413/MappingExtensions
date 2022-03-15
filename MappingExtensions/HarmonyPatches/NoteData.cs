using HarmonyLib;
using IPA.Utilities;

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
            int lineIndex = int.Parse(__state.Split(';')[0]);
            int flipLineIndex = int.Parse(__state.Split(';')[1]);
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

                        __instance.SetProperty("lineIndex", newIndex);
                        break;
                    }
                    case > 3:
                    {
                        int diff = (lineIndex - 3) * 2;
                        int newLaneCount = 4 + diff;
                        __instance.SetProperty("lineIndex", newLaneCount - diff - 1 - lineIndex);
                        break;
                    }
                    case < 0:
                    {
                        int diff = (0 - lineIndex) * 2;
                        int newLaneCount = 4 + diff;
                        __instance.SetProperty("lineIndex", newLaneCount - diff - 1 - lineIndex);
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

                        __instance.SetProperty("flipLineIndex", newIndex);
                        break;
                    }
                    case > 3:
                    {
                        int diff = (flipLineIndex - 3) * 2;
                        int newLaneCount = 4 + diff;
                        __instance.SetProperty("flipLineIndex", newLaneCount - diff - 1 - flipLineIndex);
                        break;
                    }
                    case < 0:
                    {
                        int diff = (0 - flipLineIndex) * 2;
                        int newLaneCount = 4 + diff;
                        __instance.SetProperty("flipLineIndex", newLaneCount - diff - 1 - flipLineIndex);
                        break;
                    }
                }
            }
        }
    }
}
