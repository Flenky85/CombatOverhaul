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
    internal static class KiShoutAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.KiShout)
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_UseMax = true;
                    c.m_Max = 16;
                    c.m_AffectedByIntensifiedMetamagic = false;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];
                    dmg.Value.DiceType = DiceType.D6;
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
                    "With a guttural bark, you unleash a sudden blast of sonic energy that strikes your opponent.The target takes 1d6 " +
                    "points of sonic damage per level(maximum 16d6) and is stunned for 1 round; a successful Fortitude save reduces the " +
                    "damage by half and negates the stun."
                )
                .Configure();
        }
    }
}
