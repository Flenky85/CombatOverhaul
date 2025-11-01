using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Paladin
{
    [AutoRegister]
    internal static class WeaponBondResourcesTweaks
    {
        public static void Register()
        {
            var res = BlueprintTool.Get<BlueprintAbilityResource>(AbilitiesResourcesGuids.WeaponBond);
            var amt = res.m_MaxAmount;
            amt.BaseValue = 3;

            AbilityResourceConfigurator.For(AbilitiesResourcesGuids.WeaponBond)
              .SetMaxAmount(amt)
              .Configure();
        }
    }
}
