using System.Linq;
using UnityEngine.SceneManagement;
using HarmonyLib;
using IPA;
using IPA.Loader;
using IPALogger = IPA.Logging.Logger;

namespace MappingExtensions
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        private readonly PluginMetadata _metadata;
        private readonly Harmony _harmony;

        internal static IPALogger Log { get; private set; } = null!;
        internal static bool active;

        [Init]
        public Plugin(IPALogger logger, PluginMetadata metadata)
        {
            Log = logger;
            _metadata = metadata;
            _harmony = new Harmony("com.kyle1413.BeatSaber.MappingExtensions");
        }

        [OnEnable]
        public void OnEnable()
        {
            SongCore.Collections.RegisterCapability("Mapping Extensions");
            SongCore.Collections.RegisterCapability("Mapping Extensions-Precision Placement");
            SongCore.Collections.RegisterCapability("Mapping Extensions-Extra Note Angles");
            SongCore.Collections.RegisterCapability("Mapping Extensions-More Lanes");
            _harmony.PatchAll(_metadata.Assembly);
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private static void OnActiveSceneChanged(Scene previousScene, Scene newScene)
        {
            if (newScene.name == BS_Utils.SceneNames.Menu)
                active = false;
            else if (newScene.name == BS_Utils.SceneNames.Game)
                CheckActivation();
        }

        private static void CheckActivation()
        {
            if (!BS_Utils.Plugin.LevelData.IsSet)
            {
                active = false;
                return;
            }

            var gameplayCoreSceneSetupData = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData;
            var difficultyData = SongCore.Collections.GetCustomLevelSongDifficultyData(gameplayCoreSceneSetupData.beatmapKey);
            if (difficultyData != null && difficultyData.additionalDifficultyData._requirements.Contains("Mapping Extensions"))
                active = true;
        }

        public static void ForceActivateForSong()
        {
            active = true;
        }

        [OnDisable]
        public void OnDisable()
        {
            SongCore.Collections.DeregisterCapability("Mapping Extensions");
            SongCore.Collections.DeregisterCapability("Mapping Extensions-Precision Placement");
            SongCore.Collections.DeregisterCapability("Mapping Extensions-Extra Note Angles");
            SongCore.Collections.DeregisterCapability("Mapping Extensions-More Lanes");
            _harmony.UnpatchSelf();
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
    }
}
