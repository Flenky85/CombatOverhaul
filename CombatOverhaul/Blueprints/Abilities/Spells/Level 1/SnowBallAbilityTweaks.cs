using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class SnowBallAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SnowBall)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var deal = (ContextActionDealDamage)c.Actions.Actions[0];
                    deal.Value.DiceType = DiceType.D6; 
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_UseMax = true;
                    cfg.m_Max = 4; 
                })
                .SetDescriptionValue(
                    "You conjure a ball of packed ice and snow that you can throw at a single target " +
                    "as a ranged touch attack. The snowball deals 1d6 points of cold damage per caster " +
                    "level (maximum 4d6) on a successful hit, and the target must succeed at a Fortitude " +
                    "saving throw or be staggered for 1 round."
                )
                .Configure();
        }
    }
}
