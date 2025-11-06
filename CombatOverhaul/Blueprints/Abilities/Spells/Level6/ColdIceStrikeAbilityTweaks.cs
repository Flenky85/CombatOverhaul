using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class ColdIceStrikeAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ColdIceStrike)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var deal = (ContextActionDealDamage)c.Actions.Actions[0];
                    deal.Value.DiceType = DiceType.D8;
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_Max = 14;                 
                })
                .SetDescriptionValue(
                    "You create a shredding flurry of ice slivers, which blast from your hand in a line. The line deals 1d8 " +
                    "points of cold damage per caster level (maximum 14d8)."
                )
                .Configure();
        }
    }
}
