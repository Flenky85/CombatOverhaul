using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class KiColdIceStrikeAbilityTweaks
    {
        public static void Register()
        {
            var abilites = new[]
            {
                AbilitiesGuids.KiColdIceStrike,
                AbilitiesGuids.DrunkenKiColdIceStrike,
                AbilitiesGuids.ScaledFistColdIceStrike,
            };
            foreach (var id in abilites)
            {
                AbilityConfigurator.For(id)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var deal = (ContextActionDealDamage)c.Actions.Actions[0];
                    deal.Value.DiceType = DiceType.D8;
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_Max = 14;
                })
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 6; })
                .SetDescriptionValue(
                    "A monk with this ki power can spend 6 points from his ki pool as a swift action to create a " +
                    "shredding flurry of ice slivers, which blast from his hand in a line. " +
                    "The line deals 1d6 points of cold damage per caster level (maximum 14d8)."
                )
                .Configure();
            }
        }
    }
}
