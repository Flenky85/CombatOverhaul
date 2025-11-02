using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.Designers.Mechanics.Buffs;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class ExpeditiousRetreatBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.TouchOfGracelessness)
                .EditComponent<BuffMovementSpeed>(c => { c.Value = 15; })
                .Configure();
        }
    }
}
