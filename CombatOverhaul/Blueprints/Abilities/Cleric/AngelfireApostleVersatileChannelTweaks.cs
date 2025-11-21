using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Cleric
{
    [AutoRegister]
    internal static class AngelfireApostleVersatileChannelTweaks
    {
        public static void Register()
        {
            var buffs = new[]
            {
                AbilitiesGuids.AngelfireApostleVersatileChannelBreathOfLifeCast,
                AbilitiesGuids.AngelfireApostleVersatileChannelHealCast,
                AbilitiesGuids.AngelfireApostleVersatileChannelNeutralizePoison,
                AbilitiesGuids.AngelfireApostleVersatileChannelRemoveBlindness,
                AbilitiesGuids.AngelfireApostleVersatileChannelRemoveDisease,
                AbilitiesGuids.AngelfireApostleVersatileChannelRemoveParalysis,
                AbilitiesGuids.AngelfireApostleVersatileChannelRestoration,
                AbilitiesGuids.AngelfireApostleVersatileChannelRestorationLesser,
                AbilitiesGuids.AngelfireApostleVersatileChannelResurrection,
            };

            foreach (var id in buffs)
            {
                AbilityConfigurator.For(id)
                    .EditComponent<AbilityResourceLogic>(c =>
                    {
                        c.Amount = 6;
                    })
                    .Configure();
            }
        }
    }
}
