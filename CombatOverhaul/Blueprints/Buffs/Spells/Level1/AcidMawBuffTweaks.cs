using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level1
{
    [AutoRegister]
    internal static class AcidMawBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.AcidMawBuff)
                .EditComponent<ContextRankConfig>(rc =>
                {
                    rc.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    rc.m_Type = AbilityRankType.DamageDice;
                    rc.m_Progression = ContextRankProgression.StartPlusDivStep; 
                    rc.m_StartLevel = 1;   
                    rc.m_StepLevel = 2;    
                    rc.m_UseMax = true;
                    rc.m_Max = 3;          
                })
                .Configure();
        }
    }
}
