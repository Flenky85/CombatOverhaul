using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level2
{
    [AutoRegister]
    internal static class ProtectionFromAlignmentCommunalTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ProtectionFromAlignmentCommunal)
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
