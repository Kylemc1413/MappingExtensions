using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using SongCore.UI;
using SongCore.Utilities;
using UnityEngine;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(SliderMeshController), nameof(SliderMeshController.CutDirectionToControlPointPosition))]
    internal static class SliderMeshControllerCutDirectionToControlPointPositionPatch
    {
        private static void Postfix(ref Vector3 __result, NoteCutDirection noteCutDirection)
        {
            if (!Plugin.active)
            {
                return;
            }

            var direction = (int)noteCutDirection;
            if (direction is >= 1000 and <= 1360 or >= 2000 and <= 2360)
            {
                __result = noteCutDirection.Rotation() * Vector3.down;
            }
        }
    }

    [HarmonyPatch(typeof(NoteCutDirectionExtensions), nameof(NoteCutDirectionExtensions.Direction))]
    internal static class NoteCutDirectionExtensionsDirectionPatch
    {
        private static void Postfix(ref Vector2 __result, NoteCutDirection cutDirection)
        {
            if (!Plugin.active)
            {
                return;
            }

            var direction = (int)cutDirection;
            if (direction is >= 1000 and <= 1360 or >= 2000 and <= 2360)
            {
                var newDirection = cutDirection.Rotation() * Vector3.down;
                __result = new Vector2(newDirection.x, newDirection.y);
            }
        }
    }

    [HarmonyPatch(typeof(NoteCutDirectionExtensions), nameof(NoteCutDirectionExtensions.RotationAngle))]
    internal static class NoteCutDirectionExtensionsRotationAnglePatch
    {
        private static void Postfix(ref float __result, NoteCutDirection cutDirection)
        {
            if (!Plugin.active)
            {
                return;
            }

            var direction = (int)cutDirection;
            if (direction is >= 1000 and <= 1360)
            {
                __result = 1000 - direction;
            }
            else if (direction is >= 2000 and <= 2360)
            {
                __result = 2000 - direction;
            }
        }
    }

    [HarmonyPatch(typeof(ColorNoteVisuals), nameof(ColorNoteVisuals.HandleNoteControllerDidInit))]
    internal static class ColorNoteVisualsHandleNoteControllerDidInitPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            // Converts normal notes to arrowless notes when using a certain rotation range.
            return new CodeMatcher(instructions, il)
                .MatchEndForward(
                    new CodeMatch(OpCodes.Bne_Un),
                    new CodeMatch(OpCodes.Ldarg_0))
                .ThrowIfInvalid()
                .CreateLabel(out var destination)
                .MatchStartBackwards(new CodeMatch(OpCodes.Bne_Un))
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Beq, destination),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(NoteData), nameof(NoteData.cutDirection))),
                    Transpilers.EmitDelegate<Func<NoteCutDirection, bool>>(cutDirection => Plugin.active && (int)cutDirection is >= 2000 and <= 2360))
                .SetOpcodeAndAdvance(OpCodes.Brfalse)
                .InstructionEnumeration();
        }
    }

    [HarmonyPatch(typeof(BeatmapTypeConverters), nameof(BeatmapTypeConverters.ConvertNoteCutDirection))]
    internal static class BeatmapTypeConvertersConvertNoteCutDirectionPatch
    {
        private static void Postfix(ref NoteCutDirection __result, BeatmapSaveDataCommon.NoteCutDirection noteCutDirection)
        {
            // This happens in menu, so we can't rely on Plugin.Active.
            if (RequirementsUI.instance.diffData == null || !RequirementsUI.instance.diffData.additionalDifficultyData._requirements.Any(r => r.StartsWith("Mapping Extensions", StringComparison.Ordinal)))
            {
                return;
            }

            var direction = (int)noteCutDirection;
            if (direction is >= 1000 and <= 1360 or >= 2000 and <= 2360)
            {
                __result = (NoteCutDirection)direction;
            }
        }
    }

    [HarmonyPatch(typeof(NoteBasicCutInfoHelper), nameof(NoteBasicCutInfoHelper.GetBasicCutInfo))]
    internal static class NoteBasicCutInfoHelperGetBasicCutInfoPatch
    {
        private static void Prefix(ref NoteCutDirection cutDirection)
        {
            if (!Plugin.active)
            {
                return;
            }

            if ((int)cutDirection is >= 2000 and <= 2360)
            {
                cutDirection = NoteCutDirection.Any;
            }
        }
    }

    [HarmonyPatch(typeof(RotationTimeProcessor), nameof(RotationTimeProcessor.SpawnRotationForEventValue))]
    internal static class RotationTimeProcessorSpawnRotationForEventValuePatch
    {
        private static void Postfix(ref int __result, int index)
        {
            if (BS_Utils.Plugin.LevelData.IsSet && !BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.beatmapKey.beatmapCharacteristic.requires360Movement)
            {
                return;
            }

            if (index is >= 1000 and <= 1720)
            {
                __result = index - 1360;
            }
        }
    }
}
