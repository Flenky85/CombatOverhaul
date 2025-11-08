using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class BladeBarrierAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.BladeBarrier)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var spawn = (ContextActionSpawnAreaEffect)c.Actions.Actions[0];
                    spawn.DurationValue.Rate = DurationRate.Rounds;
                    spawn.DurationValue.DiceType = DiceType.D3;
                    spawn.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                    spawn.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "An immobile, vertical curtain of whirling blades shaped of pure force springs into existence. " +
                    "Any creature passing through the wall takes 1d6 points of damage per caster level " +
                    "(maximum 14d6), with a Reflex save for half damage."
                )
                .Configure();
        }
    }
}
