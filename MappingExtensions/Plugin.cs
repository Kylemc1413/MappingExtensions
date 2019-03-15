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
        public string Name => "Mapping Extensions";
        public string Version => "1.1.1";
        public static HarmonyInstance harmony;

        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SongLoaderPlugin.SongLoader.RegisterCapability("Mapping Extensions");
            SongLoaderPlugin.SongLoader.RegisterCapability("Mapping Extensions-Precision Placement");
            SongLoaderPlugin.SongLoader.RegisterCapability("Mapping Extensions-Extra Note Angles");
            SongLoaderPlugin.SongLoader.RegisterCapability("Mapping Extensions-More Lanes");
            harmony = HarmonyInstance.Create("com.kyle1413.BeatSaber.MappingExtensions");
            ApplyPatches();

            
        }

        private void SceneManagerOnActiveSceneChanged(Scene oldScene, Scene newScene)
        {




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

        public static void ApplyPatches()
        {

            try
            {

                harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

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
