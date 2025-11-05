using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class ProtectionFromAcidBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.ProtectionFromAcid)
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.MultiplyByModifier;

                    cfg.m_StepLevel = 10; 
                    cfg.m_UseMax = true;
                    cfg.m_Max = 80;       
                })
                .Configure();
        }
    }
}
