using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;
namespace MappingExtensions.Harmony_Patches
{

    [HarmonyPatch(typeof(ObstacleController),
new Type[] {
typeof(ObstacleData),
typeof(Vector3),
typeof(Vector3),
typeof(Vector3),
typeof(float),
typeof(float),
typeof(float),
typeof(float),
})]
    [HarmonyPatch("Init", MethodType.Normal)]
    class ObstacleControllerInit
    {
        static void Prefix(ObstacleData obstacleData, ref Vector3 startPos,
            ref Vector3 midPos, ref Vector3 endPos, float move1Duration, float move2Duration, float startTimeOffset, ref float singleLineWidth)
        {/*
            if (obstacleData.width >= 1000)
            {
                float width = (float)obstacleData.width - 1000;
                Console.WriteLine("Width: " + width + " | singleLineWidth: " + singleLineWidth);
                singleLineWidth /= 1000;
            Console.WriteLine("singleLineWidth: " + singleLineWidth);
                //Offset
                float properB = ((width * 0.0006f) - 0.6f) * 0.5f;
                Vector3 offset = new Vector3(properB + (((width * singleLineWidth) - singleLineWidth) * 0.5f), 0, 0);
                Console.WriteLine("proper num: " + properB + " | offset: " + offset.x);
                startPos -= offset.x < 0? offset*-1 : offset;
                midPos -= offset.x < 0 ? offset * -1 : offset;
                endPos -= offset.x < 0 ? offset * -1 : offset;
            }
            */

        }

        static void Postfix(ref ObstacleController __instance, ObstacleData obstacleData, Vector3 startPos,
            Vector3 midPos, Vector3 endPos, float move1Duration, float move2Duration, float startTimeOffset, float singleLineWidth,
            ref bool ____initialized, ref Vector3 ____startPos, ref Vector3 ____endPos, ref Vector3 ____midPos, ref StretchableObstacle ____stretchableObstacle, ref Bounds ____bounds, ref float ____height)
        {
            if (obstacleData.width >= 1000 || ((int)obstacleData.obstacleType >= 1000 && (int)obstacleData.obstacleType <= 4000))
            {
                float num = 0;
                if (obstacleData.width >= 1000)
                {
                    float width = (float)obstacleData.width - 1000;
                    float precisionLineWidth = singleLineWidth / 1000;
                    num = width * precisionLineWidth;
                    Vector3 b = new Vector3((num - singleLineWidth) * 0.5f, 0f, 0f);
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
                    multiplier = ( (float)((int)obstacleData.obstacleType) - 1000) / 1000;
                }
                ____stretchableObstacle.SetSize(num * 0.98f, ____height * multiplier, length);
                ____bounds = ____stretchableObstacle.bounds;




            }
        }

    }

}

