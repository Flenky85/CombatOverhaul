using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Hellknight
{
    [AutoRegister]
    internal static class HellknightDisciplineOnslaughtAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HellknightDisciplineOnslaughtAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDescriptionValue(
                    "As a free action, a Hellknight increases his base speed by 10 feet and gains a +4 bonus to his Strength for 1 round. " +
                    "If the Hellknight is mounted, these bonuses also apply to his mount\n." +
                    "This ability has a cooldown of 3 rounds."
                )
                .Configure();
        }
    }
}
