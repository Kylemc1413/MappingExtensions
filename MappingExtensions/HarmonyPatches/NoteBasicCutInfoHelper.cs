using HarmonyLib;

namespace MappingExtensions.HarmonyPatches
{
    [HarmonyPatch(typeof(NoteBasicCutInfoHelper), nameof(NoteBasicCutInfoHelper.GetBasicCutInfo))]
    internal class NoteBasicCutInfoHelperGetBasicCutInfo
    {
        private static void Prefix(ref NoteCutDirection cutDirection)
        {
            if (!Plugin.active) return;
            if ((int)cutDirection is >= 2000 and <= 2360)
                cutDirection = NoteCutDirection.Any;
        }
    }
}
