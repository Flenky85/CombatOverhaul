using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Commons
{
    internal static class ShiftersEdgeFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.ShiftersEdge)
                .RemoveComponents(c => c is AddFacts)
                .SetDescriptionValue(
                    "You use your shapechanging powers to make your natural attacks especially lethal.\n" +
                    "If your Dexterity bonus is higher than your Strength bonus, your melee attacks with your natural attacks " +
                    "gain additional damage equal to +1% per point of your Dexterity bonus and +1% per point of your Strength bonus.")
                .Configure();
        }
    }
}
