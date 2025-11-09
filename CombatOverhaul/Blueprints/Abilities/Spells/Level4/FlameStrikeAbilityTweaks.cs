using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class FlameStrikeAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.FlameStrike)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_UseMax = true;
                    r.m_Max = 10;
                })
                .SetDescriptionValue(
                    "A flame strike evokes a vertical column of divine fire. The spell deals 1d6 points of damage per caster " +
                    "level (maximum 10d6). Half the damage is fire damage, but the other half results directly from divine " +
                    "power and is therefore not subject to being reduced by resistance to fire-based attacks."
                )
                .Configure();
        }
    }
}
