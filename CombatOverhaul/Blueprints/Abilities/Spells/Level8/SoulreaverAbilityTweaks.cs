using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level8
{
    [AutoRegister]
    internal static class SoulreaverAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Soulreaver)
                .EditComponents<ContextRankConfig>(
                    rc =>
                    {
                        rc.m_UseMax = true;
                        rc.m_Max = 18;
                        rc.m_AffectedByIntensifiedMetamagic = true;
                    },
                    rc => rc.m_BaseValueType == ContextRankBaseValueType.CasterLevel
                )
                .SetDescriptionValue(
                    "This potent death spell deals 1d6 points of damage per caster level (maximum 18d6) to living creatures in the area of effect."
                )
                .Configure();
        }
    }
}
