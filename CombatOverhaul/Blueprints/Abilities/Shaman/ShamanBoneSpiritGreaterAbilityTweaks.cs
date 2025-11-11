using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanBoneSpiritGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanBoneSpiritGreaterAbility)
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 5;
                })
                .SetDescriptionValue(
                    "The shaman gains DR 3/magic. This DR increases by 1 for every 4 shaman levels she possesses beyond 8th. " +
                    "In addition, as a standard action she can cause jagged pieces of bone to explode from her body in a 10-foot-radius burst. " +
                    "This deals 1d6 points of piercing damage for every 2 shaman levels she possesses. " +
                    "A successful Reflex saving throw halves this damage. " +
                    "After using this ability, the shaman must wait 5 rounds before she can use it again."
                )
                .Configure();
        }
    }
}
