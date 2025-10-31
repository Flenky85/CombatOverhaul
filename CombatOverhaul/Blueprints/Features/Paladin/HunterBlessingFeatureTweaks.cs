using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.Utility;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class HunterBlessingFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.DivineWeaponBond)
                .SetDescriptionValue(
                    "At 11th level, a divine hunter can expend a use of her smite evil ability as a swift action " +
                    "to grant herself and all allies within 10 feet the Deadly Aim, Precise Shot, and Improved " +
                    "Precise Shot feats, even if they lack the prerequisites. The effects last for 4 rounds. Evil " +
                    "creatures gain no benefit from this ability."
                )
                .Configure();
        }
    }
}
