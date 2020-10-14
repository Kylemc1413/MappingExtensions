using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
using IPA;
namespace MappingExtensions
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public static Harmony harmonyInstance;
        internal static bool patched = false;
        internal static bool active;
        [OnStart]
        public void OnApplicationStart()
        {

            SongCore.Collections.RegisterCapability("Mapping Extensions");
            SongCore.Collections.RegisterCapability("Mapping Extensions-Precision Placement");
            SongCore.Collections.RegisterCapability("Mapping Extensions-Extra Note Angles");
            SongCore.Collections.RegisterCapability("Mapping Extensions-More Lanes");
            harmonyInstance = new Harmony("com.kyle1413.BeatSaber.MappingExtensions");
            ApplyPatches();
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
       //     Console.WriteLine("Switching to Scene: " + newScene.name + "With handle: " + newScene.handle);
            if (newScene.name == "MenuCore")
                active = false;
            if (newScene.name == "GameCore")
            {
                CheckActivation();
            }


        }

        public static void ForceActivateForSong()
        {
            active = true;
        }
        void CheckActivation()
        {
            if(!BS_Utils.Plugin.LevelData.IsSet)
            {
                active = false;
                return;
            }    
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
        public void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {


        }

        public void OnApplicationQuit()
        {

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
                    harmonyInstance.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
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
                    harmonyInstance.UnpatchAll("com.kyle1413.BeatSaber.MappingExtensions");
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

        public void OnSceneUnloaded(Scene scene)
        {

        }
    }
}
