using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class AngelfireApostleVersatileChannelBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.AngelfireApostleVersatileChannelBaseAbility)
                .SetDescriptionValue(
                    "At 5th level, the angelfire apostle can spend one uses of his channel energy ability to cast " +
                    "remove blindness or restoration, lesser as a spell-like ability.\n" +
                    "At 7th level, he can choose remove disease or remove paralysis.\n" +
                    "At 9th level, he can choose neutralize poison.\n" +
                    "At 11th level, he can choose breath of life.\n" +
                    "At 13th level, he can choose heal.\n" +
                    "At 17th level, he can choose restoration.\n" +
                    "At 19th level, he can choose resurrection."
                )
                .Configure();
        }
    }
}
