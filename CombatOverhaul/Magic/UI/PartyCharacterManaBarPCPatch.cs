using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.Party;

namespace CombatOverhaul.Magic.UI
{
    // Mantén este archivo finísimo: solo el hook
    [HarmonyPatch(typeof(PartyCharacterPCView), "BindViewImplementation")]
    internal static class PartyCharacterManaBarPCPatch
    {
        static void Postfix(PartyCharacterPCView __instance)
        {
            PartyManaUI.Ensure(__instance);
        }
    }
}
