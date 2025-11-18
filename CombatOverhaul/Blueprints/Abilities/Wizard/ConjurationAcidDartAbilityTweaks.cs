using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class ConjurationAcidDartAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ConjurationAcidDartAbility)
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_Progression = ContextRankProgression.AsIs;
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "As a standard action, you can unleash an acid dart targeting any foe within 30 feet as a " +
                    "ranged touch attack. The acid dart deals 1d6 points of acid damage + 1 for every wizard levels you possess." +
                    "This attack ignores spell resistance."
                )
                .Configure();
        }
    }
}
