using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Paladin
{
    internal static class SmiteEvilTweaks
    {
        public static void Register()
        {
            Log.Info("Antes de leer el blueprint.");

            var res = ResourcesLibrary.TryGetBlueprint<BlueprintAbilityResource>(AbilitiesResourcesGuids.SmiteEvil);
            if (res == null) return;

            Log.Info($"Antes: {res.m_MaxAmount.BaseValue}");

            var amt = res.m_MaxAmount;
            amt.BaseValue = 3;
            res.m_MaxAmount = amt;

            Log.Info($"Después: {res.m_MaxAmount.BaseValue}");
        }
    }
}
