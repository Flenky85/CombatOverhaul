using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class AirDomainBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.AirDomainBaseAbility)
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_Progression = ContextRankProgression.AsIs;
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "As a standard action, you can unleash an arc of electricity targeting any foe within 30 feet as a ranged touch attack. " +
                    "This arc of electricity deals 1d6 points of electricity damage + 1 point for every levels you possess in the class that " +
                    "gave you access to this domain."
                )
                .Configure();
        }
    }
}
