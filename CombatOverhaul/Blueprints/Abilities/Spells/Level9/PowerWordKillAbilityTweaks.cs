using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level9
{
    [AutoRegister]
    internal static class PowerWordKillAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.PowerWordKill)
                .EditComponent<AbilityTargetHPCondition>(c =>
                {
                    c.CurrentHPLessThan = 151;
                    c.OverrideCurrentHPLessThan = 151;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];              
                    var elseCond = (Conditional)root.IfFalse.Actions[0];       
                    var hpCheck = (ContextConditionCompareTargetHP)elseCond.ConditionsChecker.Conditions[0];

                    hpCheck.Value = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 151
                    };
                })
                .SetDescriptionValue(
                    "You utter a single word of power that instantly kills one creature of your choice, whether the creature can " +
                    "hear the word or not. Any creature that currently has 151 or more hit point is unaffected by power word kill."
                )
                .Configure();
        }
    }
}
