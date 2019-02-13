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
        static void Prefix(int lineCount, ref ObstacleData __instance, ref int __state)
        {
            __state = __instance.lineIndex;
        }

        static void Postfix(int lineCount, ref ObstacleData __instance, ref int __state)
        {
            bool precisionWidth = __instance.width >= 1000 && __instance.width <= 4000;
            Console.WriteLine("Width: " + __instance.width);
            if (__state > 3 || __state < 0 || precisionWidth)
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
                    if (!precisionWidth)
                    {
                        newIndex = 5000 - __instance.width - newIndex;
                        if (leftSide)
                            newIndex -= 2000;
                        __instance.SetProperty("lineIndex", newIndex);
                    }
                    else
                    {
                        float preciseIndex = (newIndex - 1000f) / 1000f;
                        float preciseWidth = (__instance.width - 1000f) / 1000f;
                        Console.WriteLine(preciseIndex + " Index");
                        Console.WriteLine(preciseWidth + " Width");
                        float newPlacement = (((4 - preciseWidth - preciseIndex) * 1000f) + 1000f);
                        if (newPlacement < 1000)
                            newPlacement -= 2000f;
                        Console.WriteLine(newPlacement + " Precise");
                        __instance.SetProperty("lineIndex", (int)newPlacement);
                    }

                }
                else if (__state > 3)
                {
                        int diff = ((__state - 3) * 2);
                        int newlaneCount = 4 + diff;
                        __instance.SetProperty("lineIndex", newlaneCount - diff - __instance.width - __state);
                }
                else if (__state < 0)
                {
                        int diff = ((0 - __state) * 2);
                    int newlaneCount = 4 + diff;
                    __instance.SetProperty("lineIndex", newlaneCount - diff - __instance.width - __state);
                }
                else if (precisionWidth)
                {
                    float preciseWidth = (__instance.width - 1000f) / 1000f;
                    float mirroredIndex = (((4 - preciseWidth - __state) * 1000f) + 1000f);
                    Console.WriteLine(mirroredIndex + " Mirror Index");
                    Console.WriteLine(preciseWidth + " Width");
                    if (mirroredIndex < 1000)
                        mirroredIndex -= 2000f;
                    Console.WriteLine(mirroredIndex.ToString() + " Standard");
                    __instance.SetProperty("lineIndex", (int)mirroredIndex);
                }

            }
        }

    }
}
