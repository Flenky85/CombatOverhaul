using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class FinalJusticeFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.FinalJustice)
                .SetDescriptionValue(
                    "At 11th level, a tortured crusader can expend 6 chargess of her smite evil ability " +
                    "to declare a final justice against a target. She gains the same benefits as when " +
                    "using smite evil, but the duration of final justice's benefits is only 3 rounds and " +
                    "the bonus damage from smite evil is doubled on every attack against the target. If " +
                    "the target is already affected by the tortured crusader's smite evil, using this " +
                    "ability will remove the smite evil effect from the target."
                )
                .Configure();
        }
    }
}
