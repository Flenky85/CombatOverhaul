using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level2
{
    [AutoRegister]
    internal static class AcidArrowBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.AcidArrowBuff)
                .EditComponent<AddFactContextActions>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.NewRound.Actions[0]; 
                    dmg.Value.DiceType = DiceType.D8;                        
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2
                    };
                })
                .Configure();
        }
    }
}
