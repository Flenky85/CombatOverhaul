using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class FalseLifeGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.FalseLifeGreater)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.Div2;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 5;
                })
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "You harness the power of unlife to grant yourself a limited ability to avoid death.While this spell is in effect, " +
                    "you gain temporary hit points equal to 1d6 per 2 caster levels(maximum 5d6)."
                )
                .Configure();
        }
    }
}
