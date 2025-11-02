using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints.Classes.Spells;
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


namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class GlitterdustAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Glitterdust)
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 6;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    c.SavingThrowType = SavingThrowType.Will;

                    var damage = new ContextActionDealDamage
                    {
                        DamageType = new DamageTypeDescription
                        {
                            Type = DamageType.Energy,
                            Energy = DamageEnergyType.Electricity
                        },
                        Value = new ContextDiceValue
                        {
                            DiceType = DiceType.D4,
                            DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank },
                            BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 }
                        },
                        HalfIfSaved = true
                    };

                    c.Actions.Actions = new GameAction[] { damage, c.Actions.Actions[0] };
                })
                .EditComponent<SpellDescriptorComponent>(sd =>
                {
                    sd.Descriptor.m_IntValue |= (int)SpellDescriptor.Electricity;
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "A cloud of golden particles covers everyone and everything in the area, causing creatures to become blinded " +
                    "and visibly outlining invisible things for the duration of the spell. All within the area are covered by the " +
                    "dust, which cannot be removed and continues to sparkle until it fades. Each round at the end of their turn, " +
                    "blinded creatures may attempt new Will saving throws to end the blindness effect. Any creature covered by the " +
                    "dust takes a –40 penalty on Stealth checks.\n" +
                    "In addition, each creature in the area takes 1d4 points of electricity damage " +
                    "per caster level (maximum 6d4); a successful Will save halves this damage."
                )
                .Configure();
        }
    }
}
