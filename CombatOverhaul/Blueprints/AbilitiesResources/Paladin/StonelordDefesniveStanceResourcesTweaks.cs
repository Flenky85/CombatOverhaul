using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Paladin
{
    [AutoRegister]
    internal static class StonelordDefesniveStanceResourcesTweaks
    {
        public static void Register()
        {
            AbilityResourceConfigurator.For(AbilitiesResourcesGuids.StonelordDefesniveStance)
                .SetUseMax(false) 
                .SetMaxAmount(new BlueprintAbilityResource.Amount
                {
                    BaseValue = 3,

                    IncreasedByLevel = false,
                    IncreasedByLevelStartPlusDivStep = false,
                    LevelIncrease = 0,
                    StartingLevel = 0,
                    StartingIncrease = 0,
                    LevelStep = 0,
                    PerStepIncrease = 0,
                    MinClassLevelIncrease = 0,

                    m_Class = System.Array.Empty<BlueprintCharacterClassReference>(),
                    m_Archetypes = System.Array.Empty<BlueprintArchetypeReference>(),
                    m_ClassDiv = System.Array.Empty<BlueprintCharacterClassReference>(),
                    m_ArchetypesDiv = System.Array.Empty<BlueprintArchetypeReference>(),
                    OtherClassesModifier = 0f,

                    IncreasedByStat = true,
                    ResourceBonusStat = StatType.Constitution
                })
                .Configure();
        }
    }
}
