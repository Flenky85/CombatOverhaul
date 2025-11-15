using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Bard
{
    [AutoRegister]
    internal class BardicPerformanceResourceFactTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.BardicPerformanceResourceFact)
                .EditComponent<AddAbilityResources>(c =>
                {
                    c.Amount = 0;
                    c.RestoreAmount = false;
                    c.RestoreOnLevelUp = false;
                })
                .EditComponent<IncreaseResourcesByClass>(c =>
                {
                    c.BaseValue = 0;                 
                    c.Stat = StatType.Charisma;      
                    c.m_CharacterClass = null;       
                    c.m_Archetype = null;            
                })
                .Configure();
        }
    }
}
