using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class ElementalFistAblilityAcidTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ElementalFistAblilityAcid)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 2; })
                .Configure();
        }
    }
}
