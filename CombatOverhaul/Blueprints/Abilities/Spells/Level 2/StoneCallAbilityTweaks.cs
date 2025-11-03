using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class StoneCallAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.StoneCall)
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = AbilityRankType.DamageDice;
                    c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_UseMax = true;
                    c.m_Max = 6;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];
                    dmg.Value.DiceType = DiceType.D3;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice
                    };
                })
                .EditComponent<AdditionalAbilityEffectRunActionOnClickedTarget>(c =>
                {
                    var spawn = (ContextActionSpawnAreaEffect)c.Action.Actions[0];
                    spawn.DurationValue = new ContextDurationValue
                    {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.D3,
                        DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 2 },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                        m_IsExtendable = true
                    };
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "A rain of dirt, gravel, and small pebbles fills the area, dealing 1d3 points of bludgeoning " +
                    "damage per caster level (maximum 6d3) to every creature in the area. This damage occurs only " +
                    "once, when the spell is cast. For the remaining duration of the spell, the debris covers the " +
                    "ground, making the entire area difficult terrain."
                )
                .Configure();
        }
    }
}
