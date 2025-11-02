using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class BoneshakerAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Boneshaker)
                  .EditComponent<ContextRankConfig>(cfg =>
                  {
                      cfg.m_UseMax = true;
                      cfg.m_Max = 6; 
                  })
                  .EditComponent<AbilityEffectRunAction>(c =>
                  {
                      var cond = (Conditional)c.Actions.Actions[0];
                      var dmg = (ContextActionDealDamage)cond.IfFalse.Actions[0];

                      dmg.DamageType = new DamageTypeDescription
                      {
                          Type = DamageType.Energy,
                          Energy = DamageEnergyType.NegativeEnergy
                      };
                      dmg.Value.DiceType = DiceType.D8;
                      dmg.HalfIfSaved = true; 
                  })
                .SetDescriptionValue(
                    "You take control of a target living creature's skeleton. A creature has its skeleton rattle within its " +
                    "flesh, causing it grievous harm. The target takes 3d8 points of damage, plus 1d8 additional points of " +
                    "damage per 2 caster levels (maximum total 6d8 at caster level 6)."
                )
                .Configure();
        }
    }
}
