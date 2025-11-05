using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class BestowCurseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.BestowCurse)
                .SetDescriptionValue(
                    "You place a curse on the subject. Choose one of the following:\n" +
                    "Curse of Feeble Body — The subject suffers a –6 penalty to Constitution score.\n" +
                    "Curse of Weakness — The subject suffers a –6 penalty to Strength and Dexterity scores.\n" +
                    "Curse of Idiocy — The subject suffers a –6 penalty to Intelligence, Wisdom, and Charisma scores.\n" +
                    "Curse of Deterioration — The subject suffers a –4 penalty on attack rolls, saves, ability checks, and skill checks.\n" +
                    "Additionally, the target takes 1d4 points of negative energy damage per caster level(maximum 8d4).A successful Will save halves this damage."
                )
                .Configure();
        }
    }
}
