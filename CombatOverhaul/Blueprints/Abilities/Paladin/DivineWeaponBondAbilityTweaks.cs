using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    internal static class DivineWeaponBondAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DivineWeaponBond)
              .OnConfigure(bp =>
              {
                  bp.ActionType = UnitCommand.CommandType.Swift;
                  bp.m_IsFullRoundAction = false;
              })

              .EditComponent<AbilityResourceLogic>(c => 
              { 
                  c.Amount = 3; 
              })

              .EditComponent<AbilityEffectRunAction>(c =>
              {
                  var a0 = (ContextActionWeaponEnchantPool)c.Actions.Actions[0];
                  a0.DurationValue = new ContextDurationValue
                  {
                      Rate = DurationRate.Rounds,
                      DiceType = DiceType.Zero,
                      DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                      BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 4 },
                      m_IsExtendable = true
                  };

                  var a1 = (ContextActionApplyBuff)c.Actions.Actions[1];
                  a1.Permanent = false;
                  a1.UseDurationSeconds = false;
                  a1.SameDuration = false;
                  a1.DurationValue = new ContextDurationValue
                  {
                      Rate = DurationRate.Rounds,
                      DiceType = DiceType.Zero,
                      DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                      BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 4 },
                      m_IsExtendable = true
                  };
              })
              .Configure();
        }
    }
}
