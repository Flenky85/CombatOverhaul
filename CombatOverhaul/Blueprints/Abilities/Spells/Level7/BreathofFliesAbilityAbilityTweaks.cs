using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class BreathofFliesAbilityAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.BreathofFliesAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg1 = (ContextActionDealDamage)c.Actions.Actions[1];
                    dmg1.Value.DiceType = DiceType.D4;
                    dmg1.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        Value = 0
                    };
                    var dmg2 = (ContextActionDealDamage)c.Actions.Actions[2];
                    dmg2.Value.DiceType = DiceType.D4;
                    dmg2.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        Value = 0
                    };
                })
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel; 
                    r.m_Progression = ContextRankProgression.AsIs;
                    r.m_UseMax = true;
                    r.m_Max = 16;
                })
                .SetDescriptionValue(
                    "You open a portal to nothingness within yourself and let the hungering void consume everything. " +
                    "All enemies within 30 feet take 1d4 damage per caster level (16d4 maximun) and gain 1d4 negative levels. Half " +
                    "of this damage is bludgeoning, and the other half is negative energy. A successful Fortitude " +
                    "saving throw reduces the damage by half and the number of negative levels gained to 1. " +
                    "Regardless of whether the saving throw is successful, all creatures are also subject to " +
                    "a pull attempt. The force has a Strength modifier equal to your Intelligence, Wisdom, or " +
                    "Charisma modifier (whichever is highest). The CMB for the force's pull uses your caster " +
                    "level as its base attack bonus, adding the force's Strength modifier. If the pull maneuver " +
                    "is successful, the creature is subject to a trip maneuver attempt using the same rules."
                )
                .Configure();
        }
    }
}
