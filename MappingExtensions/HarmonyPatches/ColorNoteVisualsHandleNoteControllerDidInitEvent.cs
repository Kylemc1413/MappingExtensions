using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;

namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(ColorNoteVisuals),
new Type[] {
            typeof(NoteController)
})]
    [HarmonyPatch("HandleNoteControllerDidInitEvent", MethodType.Normal)]
    class ColorNoteVisualsHandleNoteControllerDidInitEvent
    {
        /*
        static void Postfix(NoteController noteController, ref ColorManager ____colorManager, ref SpriteRenderer ____arrowGlowSpriteRenderer, ref SpriteRenderer ____circleGlowSpriteRenderer, ref MaterialPropertyBlockController[] ____materialPropertyBlockControllers, ref int ____colorID, ref Action ___didInitEvent, ref MeshRenderer ____arrowMeshRenderer)
        {
            if ((int)noteController.noteData.cutDirection >= 2000)
            {
                ____arrowMeshRenderer.enabled = false;
                ____arrowGlowSpriteRenderer.enabled = false;
                ____circleGlowSpriteRenderer.enabled = true;
            }


        }
        */




    }
}
