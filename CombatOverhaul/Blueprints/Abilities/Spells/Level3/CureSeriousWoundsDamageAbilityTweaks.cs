using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class CureSeriousWoundsDamageAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CureSeriousWoundsDamage)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    r.m_Progression = ContextRankProgression.AsIs;
                    r.m_UseMax = true;
                    r.m_Max = 8;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var save = (ContextActionSavingThrow)c.Actions.Actions[0];
                    var dmg = (ContextActionDealDamage)save.Actions.Actions[0];

                    dmg.Value.DiceType = DiceType.D4;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                    dmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };

                    dmg.HalfIfSaved = true;
                })
                .SetDescriptionValue(
                    "When laying your hand upon a living creature, you channel positive energy that cures 1d4 points of damage per caster level " +
                    "(maximum 8d4). Since undead are powered by negative energy, this spell deals damage to them instead of curing their wounds. " +
                    "An undead creature can apply spell resistance, and can attempt a Will save to take half damage."
                )
                .Configure();
        }
    }
}
