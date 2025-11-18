using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class TelekineticFistAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.TelekineticFist)
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_Progression = ContextRankProgression.AsIs;
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "As a standard action, you can strike with a telekinetic fist, targeting any foe within 30 feet as a ranged touch attack. " +
                    "The telekinetic fist deals 1d4 points of bludgeoning damage + 1 for every wizard levels you possess."
                )
                .Configure();
        }
    }
}
