using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Hellknight
{
    [AutoRegister]
    internal static class HellKnightOrderOfTheGodclawTier1AbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HellKnightOrderOfTheGodclawTier1Ability)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 };
                    apply.DurationValue.m_IsExtendable = false;
                })
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 6; })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "This aura grants all allies in a 30-foot radius a +1 morale bonus to AC, " +
                    "attack and damage rolls against chaotic enemies. This bonus increases by +1 at 5th and 9th level.\n" +
                    "This ability has a cooldown of 6 rounds."
                )
                .Configure();
        }
    }
}
