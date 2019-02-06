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
        static bool Prefix(ref NoteData __instance)
        {
            if ((int)__instance.cutDirection >= 1000)
            {
                int cutdir = (int)__instance.cutDirection;
                int angle =  cutdir - 1000;

                angle = angle > 180 ? ((angle - 360) * -1) : 360 - angle;

                int newdir = angle + 1000;

                __instance.SetProperty("cutDirection", (NoteCutDirection)newdir);
                return false;
            }




            return true;
        }

    }
}
