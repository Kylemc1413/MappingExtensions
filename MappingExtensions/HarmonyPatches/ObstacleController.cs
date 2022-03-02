using HarmonyLib;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(ObstacleController), nameof(ObstacleController.Init))]
    internal class ObstacleControllerInit
    {
        private enum _mode { preciseHeight, preciseHeightStart };

        private static void Postfix(ObstacleData obstacleData, Vector3 startPos, Vector3 midPos, Vector3 endPos, float move2Duration, float singleLineWidth, 
             ref Vector3 ____startPos, ref Vector3 ____endPos, ref Vector3 ____midPos, StretchableObstacle ____stretchableObstacle, ref Bounds ____bounds, ColorManager ____colorManager, ref float height)
        {
            if (!Plugin.active) return;
            if (obstacleData.width >= 1000 || (int)obstacleData.obstacleType >= 1000 && (int)obstacleData.obstacleType <= 4000 || (int)obstacleData.obstacleType >= 4001 && (int)obstacleData.obstacleType <= 4005000)
            {
                _mode mode = (int)obstacleData.obstacleType >= 4001 && (int)obstacleData.obstacleType <= 4100000 ? _mode.preciseHeightStart : _mode.preciseHeight;
                int obsHeight;
                var startHeight = 0;
                if(mode == _mode.preciseHeightStart)
                {
                    var value = (int)obstacleData.obstacleType;
                    value -= 4001;
                    obsHeight = value / 1000;
                    startHeight = value % 1000;
                }
                else
                {
                    var value = (int)obstacleData.obstacleType;
                    obsHeight = value - 1000;
                }
                float num;
                if (obstacleData.width >= 1000 || mode == _mode.preciseHeightStart)
                {
                    float width = (float)obstacleData.width - 1000;
                    float precisionLineWidth = singleLineWidth / 1000;
                    num = width * precisionLineWidth;
                    Vector3 b = new((num - singleLineWidth) * 0.5f, 4 * ((float)startHeight / 1000), 0f); // Change y of b for start height
                    ____startPos = startPos + b;
                    ____midPos = midPos + b;
                    ____endPos = endPos + b;
                }
                else
                    num = obstacleData.width * singleLineWidth;

                float num2 = (____endPos - ____midPos).magnitude / move2Duration;
                float length = num2 * obstacleData.duration;
                float multiplier = 1;
                if ((int)obstacleData.obstacleType >= 1000)
                    multiplier = obsHeight / 1000f;
                
                ____stretchableObstacle.SetSizeAndColor(Mathf.Abs(num * 0.98f),Mathf.Abs(height * multiplier), Mathf.Abs(length), ____colorManager.GetObstacleEffectColor());
                ____bounds = ____stretchableObstacle.bounds;
            }
        }
    }
}
