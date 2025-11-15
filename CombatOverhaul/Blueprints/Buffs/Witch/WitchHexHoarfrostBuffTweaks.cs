using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Mechanics;

namespace CombatOverhaul.Blueprints.Buffs.Witch
{
    [AutoRegister]
    internal static class WitchHexHoarfrostBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.WitchHexHoarfrostBuff)
                .SetFrequency(DurationRate.Rounds)
                .Configure();
        }
    }
}
