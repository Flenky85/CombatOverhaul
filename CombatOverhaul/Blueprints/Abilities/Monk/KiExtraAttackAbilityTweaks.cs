using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class KiExtraAttackAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.KiExtraAttack)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDescriptionValue(
                    "By spending 3 point from his ki pool as a swift action, a monk can make one additional " +
                    "unarmed strike at his highest base attack bonus when making a flurry of blows attack. " +
                    "This bonus attack stacks with all bonus attacks gained from flurry of blows, as well as " +
                    "those from haste and similar effects."
                )
                .Configure();
        }
    }
}
