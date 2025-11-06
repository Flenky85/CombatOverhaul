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
    internal static class ForcePunchEffectAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ForcePunchEffect)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var deal = (ContextActionDealDamage)c.Actions.Actions[0];
                    deal.Value.DiceType = DiceType.D6; 
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_UseMax = true;
                    cfg.m_Max = 8; 
                })
                .SetDescriptionValue(
                    "This spell charges your hand with telekinetic force. Your successful melee touch attack " +
                    "deals 1d6 points of force damage per level (maximum 8d6) and causes the target to be pushed " +
                    "away from you in a straight line up to 5 feet per two caster levels. A successful Fortitude " +
                    "save negates the movement but not the damage."
                )
                .Configure();
        }
    }
}
