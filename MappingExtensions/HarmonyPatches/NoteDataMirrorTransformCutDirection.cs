using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(NoteCutDirectionExtensions))]
    [HarmonyPatch("Mirrored", MethodType.Normal)]
    class NoteDataMirrorTransformCutDirection
    { 
        static void Prefix(ref NoteCutDirection cutDirection, ref NoteCutDirection __state)
        {
            __state = cutDirection;
            
        }

        static void Postfix(ref NoteCutDirection __result, ref NoteCutDirection __state)
        {
            if (!Plugin.active) return;
            if ((int)__state >= 1000)
            {
                int cutdir = (int)__state;
                int newdir = 2360 - cutdir;
                __result = (NoteCutDirection)newdir;
            }

        }
    }
}
