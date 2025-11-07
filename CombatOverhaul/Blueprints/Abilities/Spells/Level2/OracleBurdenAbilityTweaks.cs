using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level2
{
    [AutoRegister]
    internal static class OracleBurdenAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.OracleBurden)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var saved = (ContextActionConditionalSaved)c.Actions.Actions[0];

                    var cond0 = (Conditional)saved.Failed.Actions[0];
                    var buff0 = (ContextActionApplyBuff)cond0.IfTrue.Actions[0];
                    buff0.DurationValue.Rate = DurationRate.Rounds;
                    buff0.DurationValue.DiceType = DiceType.D3;
                    buff0.DurationValue.DiceCountValue = ContextValues.Constant(2);
                    buff0.DurationValue.BonusValue = ContextValues.Constant(0);

                    var cond1 = (Conditional)saved.Failed.Actions[1];
                    var buff1 = (ContextActionApplyBuff)cond1.IfTrue.Actions[0];
                    buff1.DurationValue.Rate = DurationRate.Rounds;
                    buff1.DurationValue.DiceType = DiceType.D3;
                    buff1.DurationValue.DiceCountValue = ContextValues.Constant(2);
                    buff1.DurationValue.BonusValue = ContextValues.Constant(0);

                    var cond2 = (Conditional)saved.Failed.Actions[2];
                    var buff2 = (ContextActionApplyBuff)cond2.IfTrue.Actions[0];
                    buff2.DurationValue.Rate = DurationRate.Rounds;
                    buff2.DurationValue.DiceType = DiceType.D3;
                    buff2.DurationValue.DiceCountValue = ContextValues.Constant(2);
                    buff2.DurationValue.BonusValue = ContextValues.Constant(0);

                    var cond3 = (Conditional)saved.Failed.Actions[3];
                    var buff3 = (ContextActionApplyBuff)cond3.IfTrue.Actions[0];
                    buff3.DurationValue.Rate = DurationRate.Rounds;
                    buff3.DurationValue.DiceType = DiceType.D3;
                    buff3.DurationValue.DiceCountValue = ContextValues.Constant(2);
                    buff3.DurationValue.BonusValue = ContextValues.Constant(0);

                    var cond4 = (Conditional)saved.Failed.Actions[4];
                    var buff4 = (ContextActionApplyBuff)cond4.IfTrue.Actions[0];
                    buff4.DurationValue.Rate = DurationRate.Rounds;
                    buff4.DurationValue.DiceType = DiceType.D3;
                    buff4.DurationValue.DiceCountValue = ContextValues.Constant(2);
                    buff4.DurationValue.BonusValue = ContextValues.Constant(0);

                    var cond5 = (Conditional)saved.Failed.Actions[5];
                    var buff5 = (ContextActionApplyBuff)cond5.IfTrue.Actions[0];
                    buff5.DurationValue.Rate = DurationRate.Rounds;
                    buff5.DurationValue.DiceType = DiceType.D3;
                    buff5.DurationValue.DiceCountValue = ContextValues.Constant(2);
                    buff5.DurationValue.BonusValue = ContextValues.Constant(0);

                    var cond6 = (Conditional)saved.Failed.Actions[6];
                    var buff6 = (ContextActionApplyBuff)cond6.IfTrue.Actions[0];
                    buff6.DurationValue.Rate = DurationRate.Rounds;
                    buff6.DurationValue.DiceType = DiceType.D3;
                    buff6.DurationValue.DiceCountValue = ContextValues.Constant(2);
                    buff6.DurationValue.BonusValue = ContextValues.Constant(0);

                    var cond7 = (Conditional)saved.Failed.Actions[7];
                    var buff7 = (ContextActionApplyBuff)cond7.IfTrue.Actions[0];
                    buff7.DurationValue.Rate = DurationRate.Rounds;
                    buff7.DurationValue.DiceType = DiceType.D3;
                    buff7.DurationValue.DiceCountValue = ContextValues.Constant(2);
                    buff7.DurationValue.BonusValue = ContextValues.Constant(0);

                    var cond8 = (Conditional)saved.Failed.Actions[8];
                    var buff8 = (ContextActionApplyBuff)cond8.IfTrue.Actions[0];
                    buff8.DurationValue.Rate = DurationRate.Rounds;
                    buff8.DurationValue.DiceType = DiceType.D3;
                    buff8.DurationValue.DiceCountValue = ContextValues.Constant(2);
                    buff8.DurationValue.BonusValue = ContextValues.Constant(0);
                })
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
