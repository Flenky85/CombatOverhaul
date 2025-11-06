using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level4
{
    [AutoRegister]
    internal static class SerenityBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.SerenityBuff)
                .EditComponent<AddInitiatorAttackRollTrigger>(c =>
                {
                    var cond = (Conditional)c.Action.Actions[0];
                    var dmg = (ContextActionDealDamage)cond.IfTrue.Actions[1];

                    dmg.Value.DiceType = DiceType.D6; 
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 4 
                    };
                })
                .Configure();
        }
    }
}
