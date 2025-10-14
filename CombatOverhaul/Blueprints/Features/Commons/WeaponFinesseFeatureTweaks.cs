using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Commons
{
    internal static class WeaponFinesseFeatureTweaks
    {
        public static void Register()
        {
            var feat = ResourcesLibrary.TryGetBlueprint<BlueprintFeature>(FeaturesGuids.WeaponFinesse);
            if (feat == null) return;

            FeatureConfigurator.For(FeaturesGuids.WeaponFinesse)
                .RemoveComponents(c =>
                    c is AttackStatReplacement
                 || c is WeaponParametersAttackBonus)
                .SetDescriptionValue(
                    "With a light weapon, elven curve blade, estoc, or rapier made for a creature of your category, " +
                    "your main-hand attacks convert 5% extra damage per point of Strength bonus into 5% extra damage per point of Dexterity bonus.")
                .Configure();
        }
    }
}
