using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level2
{
    [AutoRegister]
    internal static class AcidArrowAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.AcidArrow)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];
                    dmg.DamageType = new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Acid };
                    dmg.Value.DiceType = DiceType.D4;

                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                    dmg.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };

                    var apply = (ContextActionApplyBuff)c.Actions.Actions[1];
                    apply.UseDurationSeconds = false;
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDiceAlternative
                    };
                })
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.Default,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 6,
                    m_AffectedByIntensifiedMetamagic = false
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    if (cfg.m_Type != AbilityRankType.DamageDiceAlternative) return;

                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.Custom;
                    cfg.m_CustomProgression = new[]
                    {
                        new ContextRankConfig.CustomProgressionItem { BaseValue = 1,  ProgressionValue = 1 }, 
                        new ContextRankConfig.CustomProgressionItem { BaseValue = 3,  ProgressionValue = 2 }, 
                        new ContextRankConfig.CustomProgressionItem { BaseValue = 6,  ProgressionValue = 3 }, 
                        new ContextRankConfig.CustomProgressionItem { BaseValue = 20, ProgressionValue = 3 }, 
                    };
                    cfg.m_UseMax = true;
                    cfg.m_Max = 3;
                    cfg.m_AffectedByIntensifiedMetamagic = false;
                })
                .SetDescriptionValue(
                    "An arrow of acid springs from your hand and speeds to its target. You must succeed " +
                    "on a ranged touch attack to hit your target. On a hit, the arrow deals 1d4 points of " +
                    "acid damage per caster level (maximum 6d4) with no splash damage. The acid then lingers " +
                    "for 1 round, plus 1 additional round at caster level 3 and again at caster level 6 " +
                    "(maximum 3 rounds), dealing 2d8 points of acid damage each round unless neutralized."
                )
                .Configure();
        }
    }
}
