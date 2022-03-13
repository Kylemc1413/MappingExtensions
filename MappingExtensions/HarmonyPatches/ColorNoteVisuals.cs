using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(ColorNoteVisuals), nameof(ColorNoteVisuals.HandleNoteControllerDidInit))]
    internal class ColorNoteVisualsHandleNoteControllerDidInit
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            return new CodeMatcher(instructions, il)
                .MatchForward(true, new CodeMatch(OpCodes.Bne_Un),
                    new CodeMatch(OpCodes.Ldarg_0))
                .ThrowIfInvalid("Couldn't match label's destination")
                .CreateLabel(out Label destination)
                .MatchBack(false, new CodeMatch(OpCodes.Bne_Un))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Beq, destination),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(NoteData), nameof(NoteData.cutDirection))),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ColorNoteVisualsHandleNoteControllerDidInit), nameof(ShouldConvertToDotNote))))
                .SetOpcodeAndAdvance(OpCodes.Brfalse)
                .InstructionEnumeration();
        }

        private static bool ShouldConvertToDotNote(NoteCutDirection noteCutDirection)
        {
            return Plugin.active && (int)noteCutDirection >= 2000 && (int)noteCutDirection <= 2360;
        }
    }
}
