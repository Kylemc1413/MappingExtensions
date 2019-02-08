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
            })]
    [HarmonyPatch("MirrorTransformCutDirection", MethodType.Normal)]
    class NoteDataMirrorTransformCutDirection
    { 
        static void Prefix(ref NoteData __instance, ref NoteCutDirection __state)
        {
            __state = __instance.cutDirection;
            
          
        }

        static void Postfix(ref NoteData __instance, ref NoteCutDirection __state)
        {
            if ((int)__state >= 1000)
            {
                int cutdir = (int)__state;
                int newdir = 2360 - cutdir;
                __instance.SetProperty("cutDirection", (NoteCutDirection)newdir);
            }

        }
    }
}
