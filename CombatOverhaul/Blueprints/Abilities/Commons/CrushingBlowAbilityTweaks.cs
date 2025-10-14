using CombatOverhaul.Guids;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace CombatOverhaul.Blueprints.Abilities.Commons
{
    internal static class CrushingBlowAbilityTweaks
    {
        public static void Register()
        {
            var id = AbilitiesGuids.CrushingBlow;

            var ability = ResourcesLibrary.TryGetBlueprint<BlueprintAbility>(id);
            if (ability == null) return;

            AbilityConfigurator.For(id)
                .OnConfigure(bp => bp.m_IsFullRoundAction = false)
                .Configure();
        }
    }
}
