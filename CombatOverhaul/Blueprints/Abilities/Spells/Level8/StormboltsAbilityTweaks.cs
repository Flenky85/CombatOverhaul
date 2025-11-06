using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level8
{
    [AutoRegister]
    internal static class StormboltsAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Stormbolts)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0]; 
                    dmg.Value.DiceType = DiceType.D4;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                })
                .EditComponents<ContextRankConfig>(
                    rc =>
                    {
                        rc.m_UseMax = true;
                        rc.m_Max = 18;
                    },
                    rc => rc.m_BaseValueType == ContextRankBaseValueType.CasterLevel
                )
                .SetDescriptionValue(
                    "When you cast this spell, lightning spills forth from your body in all directions. " +
                    "Enemy creatures within the area take 1d4 points of electricity damage per caster level " +
                    "(maximum 18d4) and are stunned for 1 round. A successful Fortitude saving throw halves the " +
                    "damage and negates the stun effect."
                )
                .Configure();
        }
    }
}
