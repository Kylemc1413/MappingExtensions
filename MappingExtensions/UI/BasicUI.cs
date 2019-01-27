using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomUI.GameplaySettings;
using CustomUI.Settings;

namespace MappingExtensions.UI
{
    class BasicUI
    {
        public static void CreateUI()
        {
            //This will create the UI for the plugin when called, keep in mind that the mod will require CustomUI when executing this as it calls functions etc from the library
            CreateGameplayOptionsUI();
            CreateSettingsUI();



        }



        public static void CreateSettingsUI()
        {
            //This will create a menu tab in the settings menu for your plugin
            var pluginSettingsSubmenu = SettingsUI.CreateSubMenu("Submenu Name");

            var exampleToggle = pluginSettingsSubmenu.AddBool("Example Toggle");
            //Fetch your initial value for the option from within the braces, or simply have it default to a value
            exampleToggle.GetValue += delegate { return false; };
            exampleToggle.SetValue += delegate (bool value)
            {
                //Whatever execution you want to occur after setting the value

            };

        }

        public static void CreateGameplayOptionsUI()
        {

            //Example submenu option
            var pluginSubmenu = GameplaySettingsUI.CreateSubmenuOption(GameplaySettingsPanels.ModifiersLeft, "Plugin Name", "MainMenu", "pluginMenu1", "You can keep all your plugin's gameplay options nested within this one button");

            //Example Toggle Option within a submenu
            var exampleOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.ModifiersLeft, "Example Toggle", "pluginMenu1", "Put a toggle for a setting you want easily accessible in game here.");
            exampleOption.GetValue = /* Fetch the initial value for the option here*/ false;
            exampleOption.OnToggle += (value) => { /*  You can execute whatever you want to occur when the value is toggled here, usually that would include updating wherever the value is pulled from   */};
            exampleOption.AddConflict("Conflicting Option Name"); //You can add conflicts with other gameplay options settings here, preventing both from being active at the same time, including that of other mods



        }





    }
}
