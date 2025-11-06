using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level5
{
    [AutoRegister]
    internal static class StoneskinCommunalBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.StoneskinCommunalBuff)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_Progression = ContextRankProgression.MultiplyByModifier;
                    r.m_StepLevel = 5;  
                    r.m_UseMax = true;
                    r.m_Max = 50;      
                })
                .Configure();
        }
    }
}
