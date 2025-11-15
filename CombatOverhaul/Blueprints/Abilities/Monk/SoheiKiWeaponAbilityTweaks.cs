using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class SoheiKiWeaponAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SoheiKiWeaponAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDescriptionValue(
                    "At 4th level, as a swift action, a sohei may spend 3 point from his ki pool to grant " +
                    "any weapon he wields (including his unarmed strike) a +1 enhancement bonus on attack " +
                    "and damage rolls, increasing by +1 per four levels after 4th to a maximum bonus of +5 " +
                    "at 20th level, until the end of his next turn."
                )
                .Configure();
        }
    }
}
