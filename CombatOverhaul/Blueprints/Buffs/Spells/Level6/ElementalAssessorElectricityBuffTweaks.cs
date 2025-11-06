using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level6
{
    [AutoRegister]
    internal static class ElementalAssessorElectricityBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.ElementalAssessorElectricityBuff)
                .EditComponent<AddFactContextActions>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.NewRound.Actions[0];
                    dmg.Value.DiceType = DiceType.D8;
                })
                .Configure();
        }
    }
}
