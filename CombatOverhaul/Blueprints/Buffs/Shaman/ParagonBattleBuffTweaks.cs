using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace CombatOverhaul.Blueprints.Buffs.Shaman
{
    [AutoRegister]
    internal static class ParagonBattleBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.New("ParagonBattleBuff", BuffsGuids.ParagonBattleBuff)
                .SetFlags(BlueprintBuff.Flags.HiddenInUi)
                .Configure();
        }
    }
}
