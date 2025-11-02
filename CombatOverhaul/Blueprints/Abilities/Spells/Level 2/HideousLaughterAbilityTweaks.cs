using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using static Kingmaker.Blueprints.Classes.Spells.SuppressSpellSchool;
using static Pathfinding.Util.RetainedGizmos;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class HideousLaughterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HideousLaughter)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.Default,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 6
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var original = c.Actions.Actions;
                    var saved = (ContextActionConditionalSaved)original[0];
                    var fireDamage = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Sonic
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        HalfIfSaved = true,
                    };

                    c.Actions.Actions = new GameAction[] { fireDamage, saved };

                    var failBuff = (ContextActionApplyBuff)saved.Failed.Actions[0];
                    failBuff.DurationValue.Rate = DurationRate.Rounds;
                    failBuff.DurationValue.DiceType = DiceType.D3;
                    failBuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 };
                    failBuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .EditComponent<SpellDescriptorComponent>(sd =>
                {
                    sd.Descriptor.m_IntValue |= (int)SpellDescriptor.Sonic;
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "This spell afflicts the subject with uncontrollable laughter. It collapses into gales of manic laughter, falling " +
                    "prone. The subject can take no actions while laughing, but is not considered helpless. After the spell ends, it can " +
                    "act normally. On the creature's next turn, it may attempt a new Will saving throw to end the effect. A creature with " +
                    "an Intelligence score of 2 or lower is not affected.\n" +
                    "In addition, the target takes 1d4 points of sonic damage per caster level(maximum 6d4); a successful Will save halves this damage."
                )
                .Configure();
        }
    }
}
