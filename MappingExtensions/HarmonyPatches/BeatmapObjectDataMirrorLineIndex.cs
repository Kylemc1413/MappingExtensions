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
        static void Prefix(int lineCount, ref BeatmapObjectData __instance, ref int __state)
        {
            __state = __instance.lineIndex;
        }

        static void Postfix(int lineCount, ref BeatmapObjectData __instance, ref int __state)
        {
            if (!Plugin.active) return;
            if (__state > 3 || __state < 0)
                {
                    if (__state >= 1000 || __state <= -1000)
                    {
                        int newIndex = __state;
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

                    else if (__state > 3)
                    {
                        int diff = ((__state - 3) * 2);
                        int newlaneCount = 4 + diff;
                        __instance.SetProperty("lineIndex", newlaneCount - diff - 1 - __state);

                    }
                    else if (__state < 0)
                    {
                        int diff = ((0 - __state) * 2);
                        int newlaneCount = 4 + diff;
                        __instance.SetProperty("lineIndex", newlaneCount - diff - 1 - __state);
                    }
                }
        }

    }
}
