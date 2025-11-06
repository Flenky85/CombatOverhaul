using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level8
{
    [AutoRegister]
    internal static class HorridWiltingAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HorridWilting)
                .EditComponents<ContextRankConfig>(
                    rc =>
                    {
                        rc.m_UseMax = true;
                        rc.m_Max = 18;                 
                    },
                    rc => rc.name == "$AbilityRankConfig$b1d0c610-d9bf-4112-8c4e-546a6b8318b5"
                )
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "This spell evaporates moisture from the body of each subject living creature, causing flesh " +
                    "to wither and crack and crumble to dust. This deals 1d6 points of damage per caster level " +
                    "(maximum 18d6). This spell is especially devastating to water elementals and plant creatures, " +
                    "which instead take 1d8 points of damage per caster level (maximum 18d8)."
                )
                .Configure();
        }
    }
}
