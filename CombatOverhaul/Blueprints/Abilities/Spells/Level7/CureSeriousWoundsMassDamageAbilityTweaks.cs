using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class CureSeriousWoundsMassDamageAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CureSeriousWoundsMassDamage)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    r.m_Progression = ContextRankProgression.AsIs;
                    r.m_UseMax = true;
                    r.m_Max = 16; 
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var save = (ContextActionSavingThrow)c.Actions.Actions[0];
                    var dmg = (ContextActionDealDamage)save.Actions.Actions[0];

                    dmg.Value.DiceType = DiceType.D3;
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
                })
                .SetDescriptionValue(
                    "You channel positive energy to cure 1d3 points of damage per caster level (maximum 16d3) on " +
                    "each creature in the radius. Like other cure spells, mass cure light wounds deals damage to " +
                    "undead in its area rather than curing them. An undead creature can apply spell resistance, " +
                    "and can attempt a Will save to take half damage."
                )
                .Configure();
        }
    }
}
