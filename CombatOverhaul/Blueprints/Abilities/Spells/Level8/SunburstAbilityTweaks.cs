using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level8
{
    [AutoRegister]
    internal static class SunburstAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Sunburst)
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    if (cfg.m_Type == AbilityRankType.Default)
                    {
                        cfg.m_UseMax = true;
                        cfg.m_Max = 18;
                        cfg.m_AffectedByIntensifiedMetamagic = false;
                    }
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];

                    var undeadDmg = (ContextActionDealDamage)root.IfTrue.Actions[0];
                    undeadDmg.Value.DiceType = DiceType.D8;
                    undeadDmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default,
                        Value = 0
                    };
                    undeadDmg.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var livingDmg = (ContextActionDealDamage)root.IfFalse.Actions[0];
                    livingDmg.Value.DiceType = DiceType.D3;
                    livingDmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default,
                        Value = 0
                    };
                    livingDmg.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .SetDescriptionValue(
                    "Sunburst causes a globe of searing radiance to explode silently from a point you select. " +
                    "All creatures in the globe are blinded and take 1d3 points of damage per caster lvl (masimun 18d3). A successful Reflex " +
                    "save negates the blindness and reduces the damage by half.\n" +
                    "An undead creature caught within the globe takes 1d8 points of damage per caster level (maximum 18d8), " +
                    "or half damage with a successful Reflex save."
                )
                .Configure();
        }
    }
}
