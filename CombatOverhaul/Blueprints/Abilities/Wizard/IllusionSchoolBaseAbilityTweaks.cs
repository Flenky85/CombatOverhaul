using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class IllusionSchoolBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.IllusionSchoolBaseAbility)
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_Progression = ContextRankProgression.AsIs;
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "As a standard action, you can fire a shimmering ray at any foe within 30 feet as a ranged touch attack. " +
                    "The ray causes creatures to be blinded for 1 round. Creatures with more Hit Dice than your wizard level " +
                    "are dazzled for 1 round instead."
                )
                .Configure();
        }
    }
}
