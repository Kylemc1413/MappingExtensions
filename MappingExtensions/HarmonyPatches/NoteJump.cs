using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(NoteJump), nameof(NoteJump.Init))]
    internal class NoteJumpInit
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // Prevents an IndexOutOfRangeException when using negative line indexes/layers.
            return new CodeMatcher(instructions)
                .MatchForward(false, new CodeMatch(OpCodes.Ldelem),
                    new CodeMatch(OpCodes.Ldc_R4),
                    new CodeMatch(OpCodes.Call))
                .ThrowIfInvalid("Couldn't match insert condition")
                .Insert(new CodeInstruction(OpCodes.Call, SymbolExtensions.GetMethodInfo(() => Math.Abs(0))))
                .InstructionEnumeration();
        }
    }
}
