using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class KiQuiveringPalmAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.KiQuiveringPalm)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 8; })
                .SetDescriptionValue(
                    "A monk can set up vibrations within the body of another creature that can thereafter be fatal if the monk so desires. " +
                    "You make touch attack and if it succeeds the target must make a Fortitude saving throw (DC 10 + 1/2 the monk's level " +
                    "+ the monk's Wis modifier) or die. Using this ability is a standard action that costs 8 ki points."
                )
                .Configure();
        }
    }
}
