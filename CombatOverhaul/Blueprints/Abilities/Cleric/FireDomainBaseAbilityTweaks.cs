using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class FireDomainBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.FireDomainBaseAbility)
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_Progression = ContextRankProgression.AsIs;
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "As a standard action, you can unleash a scorching bolt of divine fire from your outstretched hand. " +
                    "You can target any single foe within 30 feet as a ranged touch attack with this bolt of fire. If you hit " +
                    "the foe, the fire bolt deals 1d6 points of fire damage + 1 point for every level you possess in the " +
                    "class that gave you access to this domain."
                )
                .Configure();
        }
    }
}
