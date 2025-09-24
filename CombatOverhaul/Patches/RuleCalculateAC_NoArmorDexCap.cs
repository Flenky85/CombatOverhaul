/*using System.Reflection;
using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;

namespace CombatOverhaul.Patches
{
    [HarmonyPatch(typeof(RuleCalculateAC), nameof(RuleCalculateAC.OnTrigger))]
    internal static class RuleCalculateAC_NoArmorDexCap
    {
        static void Postfix(RuleCalculateAC __instance)
        {
            // Si está desprevenido, no aplica DEX igualmente; no hay nada que “devolver”.
            if (__instance.IsTargetFlatFooted) return;

            var unit = __instance?.Target;
            if (unit?.Stats?.Dexterity == null) return;

            int dexBonus = unit.Stats.Dexterity.Bonus;
            if (dexBonus <= 0) return; // nada que devolver si no hay bono positivo

            int cap = GetArmorMaxDex(unit);
            if (cap == int.MaxValue || cap >= dexBonus) return; // sin cap o cap >= bono: no hay ajuste

            int refund = dexBonus - cap; // parte de DEX que la armadura estaba capando
            if (refund > 0) __instance.Result += refund;
        }

        private static int GetArmorMaxDex(UnitEntityData unit)
        {
            // Armadura equipada
            var armor = unit?.Body?.Armor?.MaybeArmor;
            if (armor == null) return int.MaxValue;

            var type = armor.Blueprint?.Type;
            if (type == null) return int.MaxValue;

            // Intento 1: propiedad pública/privada
            var p = type.GetType().GetProperty("MaxDexterityBonus",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (p != null)
            {
                object v = p.GetValue(type, null);
                if (v is int i1) return i1;
            }

            // Intento 2: campo privado habitual
            var f = type.GetType().GetField("m_MaxDexterityBonus",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (f != null)
            {
                object v = f.GetValue(type);
                if (v is int i2) return i2;
            }

            return int.MaxValue; // si no podemos leerlo, asumimos “sin cap”
        }
    }
}*/
