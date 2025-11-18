using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Wizard
{
    [AutoRegister]
    internal static class ConjurationDimensionalStepsResourceTweaks
    {
        public static void Register()
        {
            var guid = AbilitiesResourcesGuids.ConjurationDimensionalStepsResource;
            var resource = BlueprintTool.Get<BlueprintAbilityResource>(guid);
            var amount = resource.m_MaxAmount;

            amount.BaseValue = 3;
            amount.IncreasedByLevel = false;
            amount.LevelIncrease = 0;
            amount.IncreasedByLevelStartPlusDivStep = false;
            amount.StartingLevel = 0;
            amount.StartingIncrease = 0;
            amount.LevelStep = 0;
            amount.PerStepIncrease = 0;
            amount.MinClassLevelIncrease = 0;
            amount.m_Class = System.Array.Empty<BlueprintCharacterClassReference>();
            amount.m_Archetypes = System.Array.Empty<BlueprintArchetypeReference>();
            amount.m_ClassDiv = System.Array.Empty<BlueprintCharacterClassReference>();
            amount.m_ArchetypesDiv = System.Array.Empty<BlueprintArchetypeReference>();
            amount.OtherClassesModifier = 0f;

            amount.IncreasedByStat = false;

            AbilityResourceConfigurator.For(guid)
                .SetMaxAmount(amount)
                .Configure();
        }
    }
}
