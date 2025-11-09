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
    internal static class CaveFangsStalagmitesAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CaveFangsStalagmites)
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
                    "Piercing spires of rock erupt up from the ground, dealing 3d8 points of piercing damage and knocking the " +
                    "creature prone (a successful Reflex saving throw halves this damage and avoids being knocked prone). " +
                    "Once the stalagmites appear, they function thereafter as spike stones for 1 minute per caster level " +
                    "and then crumble to dust."
                )
                .Configure();
        }
    }
}
