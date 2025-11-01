using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.FactLogic;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class MartyrPerformanceResourceFactFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.MartyrPerformanceResourceFact)
                .EditComponent<IncreaseResourcesByClass>(c =>
                {
                    c.m_CharacterClass = null; 
                    c.BaseValue = 3;           
                })
                .Configure();
        }
    }
}
