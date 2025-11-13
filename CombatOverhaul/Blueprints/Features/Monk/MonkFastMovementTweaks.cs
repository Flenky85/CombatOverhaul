using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Monk
{
    [AutoRegister]
    internal class MonkFastMovementTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.MonkFastMovement)
                .EditComponent<AddContextStatBonus>(c =>
                {
                    c.Multiplier = 5;
                })
                .SetDescriptionValue(
                    "At 3rd level, a monk gains a +5 enhancement bonus to his base speed, " +
                    "which increases by 5 feet every 3 levels thereafter. A monk in armor loses this extra speed."
                )
                .Configure();
        }
    }
}
