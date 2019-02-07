using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(NoteData),
new Type[] {
            typeof(int)})]
    [HarmonyPatch("MirrorLineIndex", MethodType.Normal)]
    class NoteDataMirrorLineIndex
    {
        static bool Prefix(int lineCount, ref NoteData __instance)
        {
            if (__instance.lineIndex > 3 || __instance.lineIndex < 0 || __instance.flipLineIndex > 3 || __instance.flipLineIndex < 0)
            {
                if (__instance.lineIndex >= 1000 || __instance.lineIndex <= -1000)
                {
                    int newIndex = __instance.lineIndex;
                    bool leftSide = false;
                    if (newIndex <= -1000)
                    {
                        newIndex += 2000;
                    }

                    if (newIndex >= 4000)
                        leftSide = true;


                    newIndex = 5000 - newIndex;
                    if (leftSide)
                        newIndex -= 2000;
                    __instance.SetProperty("lineIndex", newIndex);
                }
                else if (__instance.lineIndex > 3)
                {
                    int diff = ((__instance.lineIndex - 3) * 2);
                    int newlaneCount = 4 + diff;
                    __instance.SetProperty("lineIndex", newlaneCount - diff - 1 - __instance.lineIndex);

                }
                else if (__instance.lineIndex < 0)
                {
                    int diff = ((0 - __instance.lineIndex) * 2);
                    int newlaneCount = 4 + diff;
                    __instance.SetProperty("lineIndex", newlaneCount - diff - 1 - __instance.lineIndex);
                }

                if (__instance.flipLineIndex >= 1000 || __instance.flipLineIndex <= -1000)
                {
                    int newIndex = __instance.flipLineIndex;
                    bool leftSide = false;
                    if (newIndex <= -1000)
                    {
                        newIndex += 2000;
                    }

                    if (newIndex >= 4000)
                        leftSide = true;


                    newIndex = 5000 - newIndex;
                    if (leftSide)
                        newIndex -= 2000;

                    __instance.SetProperty("flipLineIndex", newIndex);
                }

                else if (__instance.flipLineIndex > 3)
                {
                    int diff = ((__instance.flipLineIndex - 3) * 2);
                    int newlaneCount = 4 + diff;
                    __instance.SetProperty("flipLineIndex", newlaneCount - diff - 1 - __instance.flipLineIndex);

                }
                else if (__instance.flipLineIndex < 0)
                {
                    int diff = ((0 - __instance.flipLineIndex) * 2);
                    int newlaneCount = 4 + diff;
                    __instance.SetProperty("flipLineIndex", newlaneCount - diff - 1 - __instance.flipLineIndex);
                }

                return false;
            }

            return true;

        }

    }
}
