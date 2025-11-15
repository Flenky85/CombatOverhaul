using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class SoulOfStoneAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SoulOfStoneAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDescriptionValue(
                    "At 12th level, as a swift action, a student of stone can spend 3 ki point to gain tremorsense 15 feet until his next turn.\n" +
                    "At 16th level, the range of this tremorsense increases to 30 feet.\n" +
                    "Tremorsense: You can perceive the world by creating high - pitched noises and listening to their echoes.This gives you blindsight of equal range."
                )
                .Configure();
        }
    }
}
