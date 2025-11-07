using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using static Kingmaker.Armies.TacticalCombat.Grid.TacticalCombatGrid;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level9
{
    [AutoRegister]
    internal static class MindBlankCommunalAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.MindBlankCommunal)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var applySelf = (ContextActionApplyBuff)c.Actions.Actions[0];
                    applySelf.UseDurationSeconds = false;
                    applySelf.DurationValue.Rate = DurationRate.Rounds;
                    applySelf.DurationValue.DiceType = DiceType.Zero;
                    applySelf.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    applySelf.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 6 
                    };

                    var party = (ContextActionPartyMembers)c.Actions.Actions[1];
                    var applyAlly = (ContextActionApplyBuff)party.Action.Actions[0];
                    applyAlly.UseDurationSeconds = false;
                    applyAlly.DurationValue.Rate = DurationRate.Rounds;
                    applyAlly.DurationValue.DiceType = DiceType.Zero;
                    applyAlly.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    applyAlly.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 6 
                    };
                })
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "This spell functions likemind blank, except it affects all party members."
                )
                .Configure();
        }
    }
}
