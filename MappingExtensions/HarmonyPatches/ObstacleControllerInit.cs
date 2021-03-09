using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(ObstacleController))]
    [HarmonyPatch("Init", MethodType.Normal)]
    class ObstacleControllerInit
    {
        enum Mode { preciseHeight, preciseHeightStart };

        static void Postfix(ref ObstacleController __instance, ObstacleData obstacleData, float worldRotation, Vector3 startPos,
            Vector3 midPos, Vector3 endPos, float move1Duration, float move2Duration, float singleLineWidth, 
             ref Vector3 ____startPos, ref Vector3 ____endPos, ref Vector3 ____midPos, ref StretchableObstacle ____stretchableObstacle, ref Bounds ____bounds, ref ColorManager ____colorManager, ref float height)
        {
            if (!Plugin.active) return;
            if (obstacleData.width >= 1000 || ( ((int)obstacleData.obstacleType >= 1000 && (int)obstacleData.obstacleType <= 4000) || ((int)obstacleData.obstacleType >= 4001 && (int)obstacleData.obstacleType <= 4005000)))
            {
                Mode mode = ((int)obstacleData.obstacleType >= 4001 && (int)obstacleData.obstacleType <= 4100000) ? Mode.preciseHeightStart : Mode.preciseHeight;
                int obsHeight = 0;
                int startHeight = 0;
                if(mode == Mode.preciseHeightStart)
                {
                    int value = (int)obstacleData.obstacleType;
                    value -= 4001;
                    obsHeight = value / 1000;
                    startHeight = value % 1000;
               //     Console.WriteLine(height + "<---Height       StartHeight---> " + startHeight);
                }
                else
                {
                    int value = (int)obstacleData.obstacleType;
                    obsHeight = value - 1000;
                }
                float num = 0;
                if ( (obstacleData.width >= 1000) || (mode == Mode.preciseHeightStart) )
                {
                    
                    float width = (float)obstacleData.width - 1000;
                    float precisionLineWidth = singleLineWidth / 1000;
                    num = width * precisionLineWidth;                      //Change y of b for start height
                    Vector3 b = new Vector3((num - singleLineWidth) * 0.5f, 4 * ((float)startHeight/1000), 0f);
                    ____startPos = startPos + b;
                    ____midPos = midPos + b;
                    ____endPos = endPos + b;

                }
                else
                    num = (float)obstacleData.width * singleLineWidth;

                float num2 = (____endPos - ____midPos).magnitude / move2Duration;
                float length = num2 * obstacleData.duration;
                float multiplier = 1f;
                if ((int)obstacleData.obstacleType >= 1000)
                {
                    multiplier = (float)obsHeight / 1000f;
                }
                ____stretchableObstacle.SetSizeAndColor(Mathf.Abs(num * 0.98f),Mathf.Abs(height * multiplier), Mathf.Abs(length), ____colorManager.GetObstacleEffectColor());
                ____bounds = ____stretchableObstacle.bounds;
              //  ____stretchableObstacle.transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, 180f));



            }
        }

    }

}



/*
  
  int top = 2340;
            int lower = 620;
            //store
            int storedValue = top * 1000 + lower + 4001;

            //load
            if (storedValue > 4000)
            {
                storedValue -= 4001;
                int retrievedTop = storedValue/1000; //is the floor even necessary?
                int retrievedLower = storedValue % 1000;
                Console.WriteLine(retrievedTop  + " " + retrievedLower);
            }

    */

