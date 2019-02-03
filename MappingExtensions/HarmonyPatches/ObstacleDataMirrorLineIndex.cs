using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(ObstacleData),
new Type[] {
            typeof(int)})]
    [HarmonyPatch("MirrorLineIndex", MethodType.Normal)]
    class ObstacleDataMirrorLineIndex
    {
        static bool Prefix(int lineCount, ref ObstacleData __instance)
        {
            if (__instance.lineIndex > 3 || __instance.lineIndex < 0)
            {
                if (__instance.lineIndex >= 1000 || __instance.lineIndex <= -1000)
                {
                    int newIndex = __instance.lineIndex;

                    if (newIndex <= -1000)
                        newIndex += 2000;

                    newIndex = 5001 - __instance.width - newIndex;
                    if (newIndex <= 1000)
                        newIndex -= 2000;

                    __instance.SetProperty("lineIndex", newIndex);
                    return false;
                }
                if (__instance.lineIndex > 3)
                {
                    int diff = ((__instance.lineIndex - 3) * 2);
                    int newlaneCount = 4 + diff;
                    __instance.SetProperty("lineIndex", newlaneCount - diff - __instance.width - __instance.lineIndex);

                }
                else if (__instance.lineIndex < 0)
                {
                    int diff = ((0 - __instance.lineIndex) * 2);
                    int newlaneCount = 4 + diff;
                    __instance.SetProperty("lineIndex", newlaneCount - diff - __instance.width - __instance.lineIndex);
                }
                return false;

            }
            return true;
        }

    }
}
