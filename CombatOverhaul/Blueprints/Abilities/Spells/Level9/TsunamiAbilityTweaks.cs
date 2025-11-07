using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level9
{
    [AutoRegister]
    internal static class TsunamiAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Tsunami)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.Default,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs,
                    m_UseMax = true,
                    m_Max = 20,
                    m_AffectedByIntensifiedMetamagic = true
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var save = c.Actions.Actions.OfType<ContextActionSavingThrow>().First();
                    var dmg = save.Actions.Actions.OfType<ContextActionDealDamage>().First();

                    dmg.DamageType = new DamageTypeDescription
                    {
                        Type = DamageType.Force,
                        Energy = DamageEnergyType.Magic
                    };

                    dmg.Value.DiceType = DiceType.D8;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                })
                .EditComponent<SpellDescriptorComponent>(sd =>
                {
                    sd.Descriptor.m_IntValue |= (long)SpellDescriptor.Force;
                })
                .SetDescriptionValue(
                    "You create a massive wave of water that then moves in a straight line across land at a speed of 50 feet per round. " +
                    "Creatures struck by a tsunami take 1d8 per caster level (20d8 maximun) points of force damage (a Fortitude save halves this damage).\n" +
                    "In addition, every creature must succeed on a combat maneuver check, Athletics check, or Mobility check against the " +
                    "DC of this spell or be knocked prone and carried along by the wave.\n" +
                    "Freedom of movement prevents a creature from being carried along by a tsunami but does not prevent damage caused by it hitting a creature."
                )
                .Configure();
        }
    }
}
