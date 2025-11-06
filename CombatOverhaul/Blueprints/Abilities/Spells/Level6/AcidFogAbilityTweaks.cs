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
    internal static class AcidFogAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.AcidFog)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var spawn = (ContextActionSpawnAreaEffect)c.Actions.Actions[0];
                    spawn.DurationValue.Rate = DurationRate.Rounds;
                    spawn.DurationValue.DiceType = DiceType.D3;
                    spawn.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    spawn.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "Acid fog creates a billowing mass of highly acidic, misty vapors that slow down creatures.Creatures " +
                    "moving through acid fog move at half their normal speed and take a –2 penalty on all melee attack and " +
                    "melee damage rolls.The vapors prevent effective ranged weapon attacks, granting all creatures inside the " +
                    "fog a + 8 bonus to AC against normal ranged attacks.Each round on your turn, starting when you cast the " +
                    "spell, the fog deals 4d6 points of acid damage to each creature and object within it."
                )
                .Configure();
        }
    }
}
