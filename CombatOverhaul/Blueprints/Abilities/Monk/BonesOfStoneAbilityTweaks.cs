using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class BonesOfStoneAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.BonesOfStoneAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDescriptionValue(
                    "At 7th level, as a swift action, a student of stone can spend 3 ki point to gain DR 2/magic until the beginning of his next turn.\n" +
                    "At 10th level, he can spend 3 ki point to gain DR 2 / chaotic until his next turn.\n" +
                    "At 15th level, he can spend 3 ki point to gain DR 5 / chaotic until his next turn."
                )
                .Configure();
        }
    }
}
