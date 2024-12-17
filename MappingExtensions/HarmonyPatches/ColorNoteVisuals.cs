using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using SongCore.Utilities;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(ColorNoteVisuals), nameof(ColorNoteVisuals.HandleNoteControllerDidInit))]
    internal class ColorNoteVisualsHandleNoteControllerDidInitPatch
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
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ColorNoteVisualsHandleNoteControllerDidInitPatch), nameof(ShouldConvertToDotNote))))
                .SetOpcodeAndAdvance(OpCodes.Brfalse)
                .InstructionEnumeration();
        }

        private static bool ShouldConvertToDotNote(NoteCutDirection cutDirection)
        {
            return Plugin.active && (int)cutDirection is >= 2000 and <= 2360;
        }
    }
}
