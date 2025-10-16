using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Races
{
    internal static class SlowHalflingFeatureTweaks
    {
        public static void Register()
        {
            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.SlowHalfling);
            if (feat == null) return;

            FeatureConfigurator.For(feat)
                .EditComponent<AddStatBonus>(c =>
                {
                    if (c.Stat == StatType.Speed && c.Descriptor == ModifierDescriptor.Racial)
                        c.Value = -5;
                })
                .SetDescriptionValue("Halfling have a base speed of 15 feet.")
                .Configure();
        }
    }
}
