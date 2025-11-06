using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class InvisibilityMassAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.InvisibilityMass)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var party = (ContextActionPartyMembers)c.Actions.Actions[0];
                    var mainBuff = (ContextActionApplyBuff)party.Action.Actions[0];
                    mainBuff.Permanent = false;
                    mainBuff.UseDurationSeconds = false;
                    mainBuff.DurationValue.Rate = DurationRate.Rounds;
                    mainBuff.DurationValue.DiceType = DiceType.Zero;
                    mainBuff.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    mainBuff.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 6
                    };
                })
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
