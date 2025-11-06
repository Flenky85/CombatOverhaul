using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level1
{
    [AutoRegister]
    internal static class ShockingGraspEffectAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShockingGraspEffect)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var deal = (ContextActionDealDamage)c.Actions.Actions[0];
                    deal.Value.DiceType = DiceType.D8; 
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_UseMax = true;
                    cfg.m_Max = 4; 
                })
                .SetDescriptionValue(
                    "Your successful melee touch attack deals 1d8 points of electricity damage per caster level (maximum 4d8)."
                )
                .Configure();
        }
    }
}
