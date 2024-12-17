using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using SongCore.Utilities;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(NoteJump), nameof(NoteJump.Init))]
    internal class NoteJumpInitPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // Prevents an IndexOutOfRangeException when using negative line indexes/layers.
            return new CodeMatcher(instructions)
                .MatchStartForward(
                    new CodeMatch(OpCodes.Ldelem),
                    new CodeMatch(OpCodes.Ldc_R4),
                    new CodeMatch(OpCodes.Call))
                .ThrowIfInvalid()
                .Insert(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Math), nameof(Math.Abs), new[] { typeof(int) })))
                .InstructionEnumeration();
        }
    }
}
