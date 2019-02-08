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
        static void Prefix(int lineCount, ref NoteData __instance, ref string __state)
        {
     
            __state = $"{__instance.lineIndex};{__instance.flipLineIndex}";
        }

        static void Postfix(int lineCount, ref NoteData __instance, ref string __state)
        {
            int lineIndex = int.Parse(__state.Split(';')[0]);
            int flipLineIndex = int.Parse(__state.Split(';')[1]);

            if (lineIndex > 3 || lineIndex < 0)
            {
                if (lineIndex >= 1000 || lineIndex <= -1000)
                {
                    int newIndex = lineIndex;
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
                else if (lineIndex > 3)
                {
                    int diff = ((lineIndex - 3) * 2);
                    int newlaneCount = 4 + diff;
                    __instance.SetProperty("lineIndex", newlaneCount - diff - 1 - lineIndex);

                }
                else if (lineIndex < 0)
                {
                    int diff = ((0 - lineIndex) * 2);
                    int newlaneCount = 4 + diff;
                    __instance.SetProperty("lineIndex", newlaneCount - diff - 1 - lineIndex);
                }
            }
            if (flipLineIndex > 3 || flipLineIndex < 0)
            {
                if (flipLineIndex >= 1000 || flipLineIndex <= -1000)
                {
                    int newIndex = flipLineIndex;
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

                else if (flipLineIndex > 3)
                {
                    int diff = ((flipLineIndex - 3) * 2);
                    int newlaneCount = 4 + diff;
                    __instance.SetProperty("flipLineIndex", newlaneCount - diff - 1 - flipLineIndex);

                }
                else if (flipLineIndex < 0)
                {
                    int diff = ((0 - flipLineIndex) * 2);
                    int newlaneCount = 4 + diff;
                    __instance.SetProperty("flipLineIndex", newlaneCount - diff - 1 - flipLineIndex);
                }
            }


        }

    }

}

