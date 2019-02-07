using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(BeatmapObjectData),
new Type[] {
            typeof(int)})]
    [HarmonyPatch("MirrorLineIndex", MethodType.Normal)]
    class BeatmapObjectDataMirrorLineIndex
    {
        static bool Prefix(int lineCount, ref BeatmapObjectData __instance)
        {
            if (__instance.lineIndex > 3 || __instance.lineIndex < 0)
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
                    return false;
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

                return false;

            }
            return true;
        }

    }
}
