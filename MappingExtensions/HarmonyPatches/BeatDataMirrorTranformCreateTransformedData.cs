using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using System.Reflection.Emit;
using System.Reflection;
namespace MappingExtensions.Harmony_Patches
{
    /*
    [HarmonyPatch(typeof(BeatmapDataMirrorTransform))]
    [HarmonyPatch("CreateTransformedData", MethodType.Normal)]

    class BeatDataMirrorTranformCreateTransformedData
    {
        static readonly MethodInfo clampMethod = SymbolExtensions.GetMethodInfo(() => Clamp(0, 0, 0));
        static readonly CodeInstruction[] clampInstructions = new CodeInstruction[] { new CodeInstruction(OpCodes.Ldc_I4_0),
            new CodeInstruction(OpCodes.Ldc_I4_3), new CodeInstruction(OpCodes.Call, clampMethod) };

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instructionList = instructions.ToList();

            for (int i = 0; i < instructionList.Count; i++)
            {
                if (instructionList[i].opcode == OpCodes.Callvirt)
                {
                    var method = (MethodInfo)(instructionList[i].operand);
                    if (method.Name == "get_lineIndex")
                    {
                        instructionList.InsertRange(i + 1, clampInstructions);
                        i += clampInstructions.Count();
                    }
                }

            }

            return instructionList.AsEnumerable();
        }
        static int Clamp(int input, int min, int max)
        {

            return Math.Min(Math.Max(input, min), max);
        }
    }
    */

}
