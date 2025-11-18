using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class TurnUndeadNecromancyTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.TurnUndeadNecromancy)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)cond.Failed.Actions[0];

                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.D2;
                    apply.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 1
                    };
                    apply.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 4; 
                })
                .SetDuration1d2RoundsShared()
                .SetDescriptionValue(
                    "You can, as a standard action, use one of your uses of channel positive energy to cause all undead within 30 " +
                    "feet of you to flee, as if frightened. Undead receive a Will save to negate the effect. The DC for this Will " +
                    "save is equal to 10 + half your wizard level + your Charisma modifier. Undead that fail their save flee for 1d2 rounds. " +
                    "Undead receive a new saving throw each round to end the effect. If you use channel energy in this way, it has no other " +
                    "effect (it does not heal or harm nearby creatures).\n" +
                    "This ability have a cooldown of 4 rounds."
                )
                .Configure();
        }
    }
}
