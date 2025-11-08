using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level1
{
    [AutoRegister]
    internal static class AcidMawDamageBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.AcidMawDamageBuff)
                .EditComponent<AddFactContextActions>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.NewRound.Actions[0];
                    dmg.Value.DiceType = DiceType.D6; 
                })
                .Configure();
        }
    }
}
