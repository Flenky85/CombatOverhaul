using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic; 
using UnityEngine;

namespace CombatOverhaul.Blueprints.Features.Monk
{
    [AutoRegister]
    internal class SenseiAdviceTweaks
    {
        // GUIDs
        private const string SenseiAdvice = "aed2367e51118bf4c846dc118d72e153"; 

        public static void Register()
        {
            var resRef = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(AbilitiesResourcesGuids.SenseiPerformanceResource);

            // Logs antes
            var featBefore = BlueprintTool.Get<BlueprintFeature>(SenseiAdvice);
            var addBefore = featBefore.GetComponent<AddAbilityResources>();
            var incBefore = featBefore.GetComponent<IncreaseResourcesByClass>();
            Debug.Log($"[CO][SenseiAdvice BEFORE] addRes={(addBefore?.m_Resource?.Guid.ToString() ?? "null")} | incRes={(incBefore?.m_Resource?.Guid.ToString() ?? "null")} | incStat={incBefore?.Stat} | base={incBefore?.BaseValue}");

            FeatureConfigurator.For(FeaturesGuids.SenseiAdvice)
                .EditComponent<IncreaseResourcesByClass>(c =>
                {
                    c.m_CharacterClass = null; 
                    c.m_Archetype = null;      
                    
                })
                .Configure();

            var featAfter = BlueprintTool.Get<BlueprintFeature>(SenseiAdvice);
            var addAfter = featAfter.GetComponent<AddAbilityResources>();
            var incAfter = featAfter.GetComponent<IncreaseResourcesByClass>();
            Debug.Log($"[CO][SenseiAdvice  AFTER] addRes={(addAfter?.m_Resource?.Guid.ToString() ?? "null")} | incRes={(incAfter?.m_Resource?.Guid.ToString() ?? "null")} | incStat={incAfter?.Stat} | base={incAfter?.BaseValue}");
        }
    }
}
