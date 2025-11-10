using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Others
{
    [AutoRegister]
    internal static class MountedBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.MountedBuff)
                .RemoveFromFlags(BlueprintBuff.Flags.RemoveOnRest)
                .Configure();
        }
    }
}
