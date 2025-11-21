using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class MadnessDomainGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.MadnessDomainGreaterAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    apply.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 3
                    };
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 8;
                })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "At 8th level, you can emit a 30-foot aura of madness for 3 rounds. Enemies within this aura are affected by " +
                    "confusion unless they succeed at a Will save (DC 10 + 1/2 your level in the class that gave you access to " +
                    "this domain + your Wisdom modifier). The confusion effect ends immediately when a creature leaves the area " +
                    "or when the aura expires.\n" +
                    "The ability have a cooldown of 8 rounds."
                )
                .Configure();
        }
    }
}
