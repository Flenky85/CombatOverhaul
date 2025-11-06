using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.Designers.Mechanics.Buffs;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level1
{
    [AutoRegister]
    internal static class ExpeditiousRetreatBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.TouchOfGracelessnessBuff)
                .EditComponent<BuffMovementSpeed>(c => { c.Value = 15; })
                .Configure();
        }
    }
}
