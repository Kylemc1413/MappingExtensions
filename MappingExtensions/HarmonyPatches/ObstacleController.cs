using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(ObstacleController), nameof(ObstacleController.Init))]
    internal class ObstacleControllerInitPatch
    {
        private static void Prefix(ObstacleData obstacleData, ref ObstacleSpawnData obstacleSpawnData)
        {
            if (!Plugin.active)
            {
                return;
            }

            var obstacleHeight = obstacleSpawnData.obstacleHeight;

            var height = (float)obstacleData.height;
            switch (height)
            {
                case <= -1000:
                    obstacleHeight = (height + 2000) / 1000 * StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance;
                    break;
                case >= 1000:
                    obstacleHeight = (height - 1000) / 1000 * StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance;
                    break;
                case > 2:
                    obstacleHeight = height * StaticBeatmapObjectSpawnMovementData.kNoteLinesDistance;
                    break;
            }

            obstacleSpawnData = new ObstacleSpawnData(obstacleSpawnData.moveOffset, obstacleSpawnData.obstacleWidth, obstacleHeight);
        }
    }
}
