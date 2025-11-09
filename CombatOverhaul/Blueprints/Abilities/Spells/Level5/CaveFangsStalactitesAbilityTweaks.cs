using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;


namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class CaveFangsStalactitesAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CaveFangsStalactites)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 12 };
                })
                .SetDuration12RoundsShared()
                .SetDescriptionValue(
                    "Shards of rock drop from above, dealing 3d8 points of bludgeoning and piercing damage (Reflex half). " +
                    "A creature that fails its Reflex save is pinned to the ground under stalactites and rubble, gaining " +
                    "the entangled condition until it can free itself with a successful DC 15 Strength check or DC 20 Mobility check."
                )
                .Configure();
        }
    }
}
