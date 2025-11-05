using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class VengefulCometsAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.VengefulComets)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var repeat = c.Actions.Actions[1] as ContextActionRepeatedActions;
                    repeat.Value.DiceType = DiceType.Zero;
                    repeat.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    repeat.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Rank, ValueRank = AbilityRankType.StatBonus };

                    var apply = repeat.Actions.Actions[0] as ContextActionApplyBuff;
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 }; 
                })

                .EditComponent<ContextRankConfig>(cfg =>
                {
                    if (cfg.m_Type != AbilityRankType.StatBonus) return; 
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.DivStep;
                    cfg.m_StepLevel = 2;      
                    cfg.m_UseMax = true;
                    cfg.m_Max = 4;            
                })
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "You conjure a number of miniature comets of ice (up to one per two caster levels, maximum 4) to orbit around " +
                    "you for 6 rounds, granting you a circumstance bonus equal to the number of comets on all saving throws against " +
                    "fire effects.\n" +
                    "Once per round when you are affected by a spell cast by an enemy, you fire one of your comets in retaliation. " +
                    "This is a ranged touch attack that deals 1d8 points of bludgeoning damage and 3d8 points of cold damage.If the " +
                    "spell you're retaliating against had the fire descriptor, you fire one additional comet."
                )
                .Configure();
        }
    }
}
