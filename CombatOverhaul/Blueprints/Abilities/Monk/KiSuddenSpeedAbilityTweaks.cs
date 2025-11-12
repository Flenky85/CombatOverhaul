using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class KiSuddenSpeedAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.KiSuddenSpeed)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                    apply.DurationValue.m_IsExtendable = false;
                })
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "A monk with this ki power can spend 3 point from his ki pool as a swift action to grant " +
                    "himself a sudden burst of speed. This increases the monk's base land speed by 20 feet for 6 rounds."
                )
                .Configure();
        }
    }
}
