using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;


namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class StoneskinAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Stoneskin)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)

                .SetMaterialComponent(new BlueprintAbility.MaterialComponentData
                {
                    m_Item = null,
                    Count = 0
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];

                    var applyTrue = (ContextActionApplyBuff)root.IfTrue.Actions[0];
                    applyTrue.UseDurationSeconds = false;
                    applyTrue.DurationValue.Rate = DurationRate.Rounds;
                    applyTrue.DurationValue.DiceType = DiceType.Zero;
                    applyTrue.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    applyTrue.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 3
                    };

                    var applyFalse = (ContextActionApplyBuff)root.IfFalse.Actions[0];
                    applyFalse.UseDurationSeconds = false;
                    applyFalse.DurationValue.Rate = DurationRate.Rounds;
                    applyFalse.DurationValue.DiceType = DiceType.Zero;
                    applyFalse.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    applyFalse.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 3
                    };
                })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "The warded creature gains resistance to blows, cuts, stabs, and slashes. The subject" +
                    " gains DR 10/adamantine. It ignores the first 10 points of damage each time it takes" +
                    " damage from a weapon, though an adamantine weapon overcomes the reduction. Once the " +
                    "spell has prevented a total of 5 points of damage per caster level (maximum 50 points)," +
                    " it is discharged."
                )
                .Configure();

            
        }
    }
}
