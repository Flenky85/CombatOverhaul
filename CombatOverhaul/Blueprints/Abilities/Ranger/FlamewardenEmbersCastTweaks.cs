using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Ranger
{
    [AutoRegister]
    internal static class FlamewardenEmbersCastTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.FlamewardenEmbersCast)
                .SetDescriptionValue(
                    "At 9th level, a flamewarden can fan the last spark of a recently slain creature's life force back into a full flame. " +
                    "Once per combat as a standard action, a flamewarden can touch the corpse of a creature to grant it the effects of breath of life."
                )
                .Configure();
        }
    }
}
