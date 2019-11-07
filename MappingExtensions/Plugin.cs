using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionPlugin;
using Harmony;

namespace MappingExtensions
{
    public class Plugin : IPlugin
    {
        public string Name => "MappingExtensions";
        public string Version => "1.3.2";
        public static HarmonyInstance harmony;
        internal static bool patched = false;
        internal static bool active;
        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SongCore.Collections.RegisterCapability("Mapping Extensions");
            SongCore.Collections.RegisterCapability("Mapping Extensions-Precision Placement");
            SongCore.Collections.RegisterCapability("Mapping Extensions-Extra Note Angles");
            SongCore.Collections.RegisterCapability("Mapping Extensions-More Lanes");
            harmony = HarmonyInstance.Create("com.kyle1413.BeatSaber.MappingExtensions");
            ApplyPatches();
        }

        private void SceneManagerOnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
       //     Console.WriteLine("Switching to Scene: " + newScene.name + "With handle: " + newScene.handle);
            if (newScene.name == "MenuCore")
                active = false;
            if (newScene.name == "GameCore")
            {
                CheckActivation();
            }
            Harmony_Patches.ObstacleControllerInit.currentObstacleColor = null;


        }

        public static void ForceActivateForSong()
        {
            active = true;
        }
        void CheckActivation()
        {
            var diff = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap;
            var songData = SongCore.Collections.RetrieveDifficultyData(diff);
            if (songData != null)
            {
                if (songData.additionalDifficultyData._requirements.Contains("Mapping Extensions"))
                {
        //            Console.WriteLine("Active");
                    active = true;
                }
                else
                {
                    active = false;
        //            Console.WriteLine("InActive");
                }

            }
            else
            {
        //        Console.WriteLine("Null Data");
                active = false;
            }

        }
        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {


        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level)
        {

        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {


        }

        internal static void ApplyPatches()
        {
     //       Console.WriteLine("Patching");
            try
            {
                if(!patched)
                {
                harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
                patched = true;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        internal static void RemovePatches()
        {
       //     Console.WriteLine("UnPatching");
            try
            {
                if(patched)
                {
                harmony.UnpatchAll("com.kyle1413.BeatSaber.MappingExtensions");
                    patched = false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void OnFixedUpdate()
        {
        }
    }
}
