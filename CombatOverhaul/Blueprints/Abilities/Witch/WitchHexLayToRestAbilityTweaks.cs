using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Witch
{
    [AutoRegister]
    internal static class WitchHexLayToRestAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.WitchHexLayToRestAbility)
                .SetDescriptionValue(
                    "The witch may target a single undead creature with this hex as if with an undeath to death spell. " +
                    "A Will save negates this effect. Whether or not the save is successful, a creature cannot be the " +
                    "target of this hex again on new combat."
                )
                .Configure();
        }
    }
}
