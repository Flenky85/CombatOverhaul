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
    internal static class FireballAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Fireball)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var deal = (ContextActionDealDamage)c.Actions.Actions[0];
                    deal.Value.DiceType = DiceType.D6;
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_Max = 8;                 
                })
                .SetDescriptionValue(
                    "A fireball spell generates a searing explosion of flame that detonates with a low roar and deals " +
                    "1d6 points of fire damage per caster level (maximum 8d6) to every creature within the area. A successful " +
                    "Reflex save halves this damage."
                )
                .Configure();
        }
    }
}
