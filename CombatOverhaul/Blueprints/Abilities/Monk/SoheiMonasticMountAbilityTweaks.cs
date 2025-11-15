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
    internal static class SoheiMonasticMountAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SoheiMonasticMountAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var onPet = (ContextActionsOnPet)c.Actions.Actions[0];
                    var apply = (ContextActionApplyBuff)onPet.Actions.Actions[0];

                    apply.UseDurationSeconds = false;
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    apply.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 6
                    };
                    apply.DurationValue.m_IsExtendable = true;
                })
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "At 1st level a sohei gains a horse as a mount. A sohei may spend 3 point from his ki pool to " +
                    "grant his mount temporary hit points equal to twice his level for 6 rounds."
                )
                .Configure();
        }
    }
}
