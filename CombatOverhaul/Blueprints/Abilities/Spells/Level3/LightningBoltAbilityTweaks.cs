using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class LightningBoltAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.LightningBolt)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var deal = (ContextActionDealDamage)c.Actions.Actions[0];
                    deal.Value.DiceType = DiceType.D8;
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_Max = 8;                 
                })
                .SetDescriptionValue(
                    "You release a powerful stroke of electrical energy that deals 1d8 points of electricity damage per " +
                    "caster level (maximum 8d8) to each creature within its area. The bolt begins at your fingertips."
                )
                .Configure();
        }
    }
}
