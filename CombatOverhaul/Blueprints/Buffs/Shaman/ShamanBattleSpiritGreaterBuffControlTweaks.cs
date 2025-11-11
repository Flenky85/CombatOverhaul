using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace CombatOverhaul.Blueprints.Buffs.Shaman
{
    [AutoRegister]
    internal static class ShamanBattleSpiritGreaterBuffControlTweaks
    {
        public static void Register()
        {
            BuffConfigurator.New("ShamanBattleSpiritGreaterBuffControl", BuffsGuids.ShamanBattleSpiritGreaterBuffControl)
                .SetFlags(BlueprintBuff.Flags.HiddenInUi)
                .Configure();
        }
    }
}
