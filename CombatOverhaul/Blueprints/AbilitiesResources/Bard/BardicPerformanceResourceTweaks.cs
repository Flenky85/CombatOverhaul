using System;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;

namespace CombatOverhaul.Blueprints.AbilitiesResources.Bard
{
    [AutoRegister]
    internal static class BardicPerformanceResourceTweaks
    {
        public static void Register()
        {
            var res = BlueprintTool.Get<BlueprintAbilityResource>(AbilitiesResourcesGuids.BardicPerformanceResource);
            var amount = res.m_MaxAmount;

            amount.BaseValue = 2;
            amount.IncreasedByLevel = false;
            amount.IncreasedByLevelStartPlusDivStep = true;
            amount.StartingLevel = 0;
            amount.StartingIncrease = 0;
            amount.LevelStep = 4;
            amount.PerStepIncrease = 1;

            var oldClass = amount.m_Class ?? Array.Empty<BlueprintCharacterClassReference>();
            var newClass = new BlueprintCharacterClassReference[oldClass.Length + 1];
            Array.Copy(oldClass, newClass, oldClass.Length);
            newClass[oldClass.Length] = BlueprintTool.GetRef<BlueprintCharacterClassReference>(ClassGuids.MonkClass);
            amount.m_ClassDiv = newClass;

            var oldArch = amount.m_Archetypes ?? Array.Empty<BlueprintArchetypeReference>();
            var newArch = new BlueprintArchetypeReference[oldArch.Length + 1];
            Array.Copy(oldArch, newArch, oldArch.Length);
            newArch[oldArch.Length] = BlueprintTool.GetRef<BlueprintArchetypeReference>(ArchetypesGuids.SenseiArchetype);
            amount.m_ArchetypesDiv = newArch;

            AbilityResourceConfigurator.For(res)
                .SetMaxAmount(amount)
                .Configure();
        }
    }
}
