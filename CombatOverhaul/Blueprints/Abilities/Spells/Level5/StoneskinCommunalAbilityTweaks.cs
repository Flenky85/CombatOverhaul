using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;


namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class StoneskinCommunalAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.StoneskinCommunal)
                .SetMaterialComponent(new BlueprintAbility.MaterialComponentData
                {
                    m_Item = null,
                    Count = 0
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.UseDurationSeconds = false;
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero; 
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 };
                })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "All allies within 30 feet gain resistance to blows, cuts, stabs, and slashes. The subject" +
                    " gains DR 10/adamantine. It ignores the first 10 points of damage each time it takes" +
                    " damage from a weapon, though an adamantine weapon overcomes the reduction. Once the " +
                    "spell has prevented a total of 5 points of damage per caster level (maximum 50 points)," +
                    " it is discharged."
                )
                .Configure();

            
        }
    }
}
