using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.MVVM._PCView.ActionBar;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UI.UnitSettings;
using TMPro;

namespace CombatOverhaul.Magic.UI
{
    /// <summary>
    /// Opción A: Cambia SOLO el texto del contador para hechizos de nivel 1 de la party en combate.
    /// No altera gating ni recursos, solo UI.
    /// </summary>
    [HarmonyPatch(typeof(ActionBarBaseSlotView), "SetResourceCount")]
    internal static class ActionBarBaseSlotView_ShowManaCost
    {
        private const int Level1Cost = 10;

        static void Postfix(ActionBarBaseSlotView __instance)
        {
            try
            {
                // 1) Conseguir ViewModel y el slot mecánico
                var vmProp = AccessTools.Property(__instance.GetType(), "ViewModel");
                var vm = vmProp?.GetValue(__instance, null) as ActionBarSlotVM;
                var slot = vm?.MechanicActionBarSlot as MechanicActionBarSlotSpell;
                var ad = slot?.Spell;
                var bp = ad?.Blueprint;
                if (ad == null || bp == null || !bp.IsSpell) return;

                // 2) Filtros de nuestro sistema
                if (ad.SpellLevel != 1) return; // Solo nivel 1
                // Excluir variantes de toque (evita confusión visual)
                //if (bp.GetComponent<AbilityEffectStickyTouch>() != null || bp.GetComponent<AbilityDeliverTouch>() != null) return;

                UnitEntityData caster = ad.Caster?.Unit;
                if (caster == null || !caster.IsPlayerFaction) return;

                // 3) Sobrescribir el texto del contador por el coste de maná
                var countField = AccessTools.Field(typeof(ActionBarBaseSlotView), "m_ResourceCount");
                var label = countField?.GetValue(__instance) as TextMeshProUGUI;
                if (label == null) return;

                label.text = Level1Cost.ToString();
            }
            catch
            {
                // swallow
            }
        }
    }
}
