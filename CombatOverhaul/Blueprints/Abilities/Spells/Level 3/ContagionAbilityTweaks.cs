using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class ContagionAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Contagion)
                .SetDescriptionValue(
                    "The subject contracts one of the following diseases: blinding sickness, bubonic plague, " +
                    "cackle fever, mindfire, or shakes. The disease is contracted immediately (the onset period " +
                    "does not apply).\n" +
                    "Blinding Sickness effect: 1d4 Str damage, target must make an additional Fort save or be " +
                    "permanently blinded.\n" +
                    "Bubonic Plague effect: 1d4 Con damage and target is fatigued.\n" +
                    "Cackle Fever effect: 1d6 Wis damage.\n" +
                    "Mindfire effect: 1d4 Int damage.\n" +
                    "Shakes effect: 1d8 Dex damage.\n" +
                    "Additionally, the target takes 1d4 points of negative energy damage per caster level(maximum 8d4).A successful Will save halves this damage."
                )
                .Configure();
        }
    }
}
