using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class ConeOfColdAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ConeOfCold)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_Max = 12; 
                })
                .SetDescriptionValue(
                    "Cone of cold creates an area of extreme cold, originating at your hand and extending outward in a cone. " +
                    "It drains heat, dealing 1d6 points of cold damage per caster level (maximum 12d6)."
                )
                .Configure();
        }
    }
}
