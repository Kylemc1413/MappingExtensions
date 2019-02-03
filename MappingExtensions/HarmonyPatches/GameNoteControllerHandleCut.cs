using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;
namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(GameNoteController),
          new Type[] {
            typeof(Saber),
            typeof(Vector3),
          typeof(Quaternion),
        typeof(Vector3),
        typeof(bool)
          })]
    [HarmonyPatch("HandleCut", MethodType.Normal)]

    class GameNoteControllerHandleCut
    {/*
        static bool Prefix(GameNoteController __instance, Saber saber, Vector3 cutPoint, Quaternion orientation, Vector3 cutDirVec, bool allowBadCut,
            ref AudioTimeSyncController ____audioTimeSyncController, ref BoxCuttableBySaber ____bigCuttableBySaber, ref BoxCuttableBySaber ____smallCuttableBySaber, NoteBasicCutInfoSO ____noteBasicCutInfo)
        {
            if ((int)__instance.noteData.cutDirection >= 2000)
            {
                float timeDeviation = __instance.noteData.time - ____audioTimeSyncController.songTime;
                bool flag;
                bool flag2;
                bool flag3;
                float cutDirDeviation;
                ____noteBasicCutInfo.GetBasicCutInfo(__instance.noteTransform, __instance.noteData.noteType, NoteCutDirection.Any, saber.saberType, saber.bladeSpeed, cutDirVec, out flag, out flag2, out flag3, out cutDirDeviation);
                float swingRating = 0f;
                SaberAfterCutSwingRatingCounter afterCutSwingRatingCounter = null;
                if (flag && flag2 && flag3)
                {
                    swingRating = saber.ComputeSwingRating();
                    afterCutSwingRatingCounter = saber.CreateAfterCutSwingRatingCounter();
                }
                else if (!allowBadCut)
                {
                    return false;
                }
                Vector3 vector = orientation * Vector3.up;
                Plane plane = new Plane(vector, cutPoint);
                float cutDistanceToCenter = Mathf.Abs(plane.GetDistanceToPoint(__instance.noteTransform.position));
                NoteCutInfo noteCutInfo = new NoteCutInfo(flag2, flag, flag3, false, saber.bladeSpeed, cutDirVec, saber.saberType, swingRating, timeDeviation, cutDirDeviation, plane.ClosestPointOnPlane(__instance.transform.position), vector, afterCutSwingRatingCounter, cutDistanceToCenter);
                ____bigCuttableBySaber.canBeCut = false;
                ____smallCuttableBySaber.canBeCut = false;
                __instance.SendNoteWasCutEvent(noteCutInfo);
                return false;
            }
            return true;

        }
        */
    }

}
