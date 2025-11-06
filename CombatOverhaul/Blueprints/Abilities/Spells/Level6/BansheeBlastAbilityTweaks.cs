using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;


namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class BansheeBlastAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.BansheeBlast)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_UseMax = true;
                    r.m_Max = 14;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var outerSaved = (ContextActionConditionalSaved)c.Actions.Actions[1];
                    var willSave = (ContextActionSavingThrow)outerSaved.Failed.Actions[0];
                    var innerSaved = (ContextActionConditionalSaved)willSave.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)innerSaved.Failed.Actions[0];

                    apply.UseDurationSeconds = false;
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
                .SetDuration1d2RoundsShared()
                .SetDescriptionValue(
                    "You create a cone of spectral energy resembling screaming elven ghosts that deals 1d4 points of damage per caster level " +
                    "(maximum 14d4); a successful Reflex save halves this damage. Any creature that fails its Reflex save must succeed at a " +
                    "Will save or become frightened for 1d2 rounds."
                )
                .Configure();
        }
    }
}
