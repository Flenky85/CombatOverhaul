using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.MVVM._PCView.ActionBar;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UI.UnitSettings;
using TMPro;

namespace CombatOverhaul.Magic.UI
{
    [HarmonyPatch(typeof(ActionBarBaseSlotView), "SetResourceCount")]
    internal static class ActionBarBaseSlotView_ShowManaCost
    {
        static void Postfix(ActionBarBaseSlotView __instance)
        {
            try
            {
                var vmProp = AccessTools.Property(__instance.GetType(), "ViewModel");
                var vm = vmProp?.GetValue(__instance, null) as ActionBarSlotVM;
                var slot = vm?.MechanicActionBarSlot as MechanicActionBarSlotSpell;
                var ad = slot?.Spell;
                var bp = ad?.Blueprint;
                if (ad == null || bp == null || !bp.IsSpell) return;

                int level = ad.SpellLevel;
                if (level <= 0) return;

                UnitEntityData caster = ad.Caster?.Unit;
                if (caster == null || !caster.IsPlayerFaction) return;

                int cost = ManaCosts.FromLevel(level);
                if (cost <= 0) return;

                var countField = AccessTools.Field(typeof(ActionBarBaseSlotView), "m_ResourceCount");
                var label = countField?.GetValue(__instance) as TextMeshProUGUI;
                if (label == null) return;

                label.text = cost.ToString();
            }
            catch
            {
                // swallow
            }
        }
    }
}
