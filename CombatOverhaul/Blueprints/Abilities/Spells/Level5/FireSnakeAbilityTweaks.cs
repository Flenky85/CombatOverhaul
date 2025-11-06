using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class FireSnakeAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.FireSnake)
                .EditComponent<ContextRankConfig>(r =>
                {
                    if (r.m_BaseValueType == ContextRankBaseValueType.CasterLevel)
                    {
                        r.m_UseMax = true;
                        r.m_Max = 12; 
                    }
                })
                .SetDescriptionValue(
                    "You create a 50-foot-long line of flames. The fire snake affects all enemies in the line, " +
                    "avoiding allies. Creatures in the path of the fire snake take 1d6 points of fire damage " +
                    "per caster level (maximum 12d6)."
                )
                .Configure();
        }
    }
}
