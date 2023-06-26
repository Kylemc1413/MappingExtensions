using System.Linq;
using System.Reflection;
using UnityEngine.SceneManagement;
using HarmonyLib;
using IPA;
using SongCore.Data;
using IPALogger = IPA.Logging.Logger;

namespace MappingExtensions
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        private static Harmony _harmony = null!;
        internal static IPALogger Log { get; private set; } = null!;
        internal static bool active;

        [Init]
        public Plugin(IPALogger logger)
        {
            Log = logger;
        }

        [OnEnable]
        public void OnEnable()
        {
            SongCore.Collections.RegisterCapability("Mapping Extensions");
            SongCore.Collections.RegisterCapability("Mapping Extensions-Precision Placement");
            SongCore.Collections.RegisterCapability("Mapping Extensions-Extra Note Angles");
            SongCore.Collections.RegisterCapability("Mapping Extensions-More Lanes");
            _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "com.kyle1413.BeatSaber.MappingExtensions");
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
            if(!BS_Utils.Plugin.LevelData.IsSet)
            {
                active = false;
                return;
            }
            IDifficultyBeatmap? diff = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap;
            ExtraSongData.DifficultyData? songData = SongCore.Collections.RetrieveDifficultyData(diff);
            if(songData != null && songData.additionalDifficultyData._requirements.Contains("Mapping Extensions"))
                active = true;
        }

        public static void ForceActivateForSong()
        {
            active = true;
        }

        [OnDisable]
        public void OnDisable()
        {
            SongCore.Collections.DeregisterizeCapability("Mapping Extensions");
            SongCore.Collections.DeregisterizeCapability("Mapping Extensions-Precision Placement");
            SongCore.Collections.DeregisterizeCapability("Mapping Extensions-Extra Note Angles");
            SongCore.Collections.DeregisterizeCapability("Mapping Extensions-More Lanes");
            _harmony.UnpatchSelf();
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
    }
}
