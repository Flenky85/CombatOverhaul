// Blueprints/Features/Races/SlowAndSteadyOreadFeatureTweaks.cs
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
    internal static class SlowAndSteadyOreadFeatureTweaks
    {
        public static void Register()
        {
            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.SlowAndSteadyOread);
            if (feat == null) return;

            FeatureConfigurator.For(feat)
                .EditComponent<AddStatBonus>(c =>
                {
                    if (c.Stat == StatType.Speed && c.Descriptor == ModifierDescriptor.Armor)
                        c.Value = -5;
                })
                .SetDescriptionValue(
                    "Oread have a base speed of 15 feet, but their speed is never modified by armor or encumbrance.")
                .Configure();
        }
    }
}
