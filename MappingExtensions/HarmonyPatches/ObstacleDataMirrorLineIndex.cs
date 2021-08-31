using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(ObstacleData),
new Type[] {
            typeof(int)})]
    [HarmonyPatch("Mirror", MethodType.Normal)]
    class ObstacleDataMirrorLineIndex
    {
        static void Prefix(int lineCount, ref ObstacleData __instance, ref int __state)
        {
            __state = __instance.lineIndex;
        }

        static void Postfix(int lineCount, ref ObstacleData __instance, ref int __state)
        {
            if (!Plugin.active) return;
            bool precisionWidth = __instance.width >= 1000;
            //   Console.WriteLine("Width: " + __instance.width);
            if (__state > 3 || __state < 0 || precisionWidth)
            {
                if (__state >= 1000 || __state <= -1000 || precisionWidth) // precision lineIndex
                {
                    int newIndex = __state;
                    if (newIndex <= -1000) // normalize index values, we'll fix them later
                    {
                        newIndex += 1000;
                    }
                    else if (newIndex >= 1000)
                    {
                        newIndex += -1000;
                    }
                    else
                    {
                        newIndex = newIndex * 1000; //convert lineIndex to precision if not already
                    }
                    newIndex = (((newIndex - 2000) * -1) + 2000); //flip lineIndex

                    int newWidth = __instance.width; //normalize wall width
                    if (newWidth < 1000)
                    {
                        newWidth = newWidth * 1000;
                    }
                    else
                    {
                        newWidth -= 1000;
                    }
                    newIndex = newIndex - newWidth;

                    if (newIndex < 0)
                    { //this is where we fix them
                        newIndex -= 1000;
                    }
                    else
                    {
                        newIndex += 1000;
                    }
                    __instance.SetProperty("lineIndex", newIndex);
                }
                else // state > -1000 || state < 1000 assumes no precision width
                {
                    int mirrorLane = (((__state - 2) * -1) + 2); //flip lineIndex
                    __instance.SetProperty("lineIndex", mirrorLane - __instance.width); //adjust for wall width
                }

            }
        }

    }
}
