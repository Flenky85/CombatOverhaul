using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;

namespace CombatOverhaul.Blueprints.Buffs.Cleric
{
    [AutoRegister]
    internal static class DeathDomainBaseBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.DeathDomainBaseBuff)
                .SetEmulateAbilityContext(true)
                .EditComponent<AddFactContextActions>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.NewRound.Actions[0];
                    dmg.Value.DiceType = DiceType.D6;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 1
                    };
                    dmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    };
                })

                .AddComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_Type = AbilityRankType.Default;
                    cfg.m_BaseValueType = ContextRankBaseValueType.ClassLevel; 
                    cfg.m_Class = new BlueprintCharacterClassReference[]
                    {
                        BlueprintTool.GetRef<BlueprintCharacterClassReference>(ClassGuids.ClericClass)
                    };
                    cfg.m_Progression = ContextRankProgression.DivStep; 
                    cfg.m_StepLevel = 2;
                    cfg.m_UseMin = true;
                    cfg.m_Min = 0;
                    cfg.m_DisableRankBonus = false;

                    cfg.m_AdditionalArchetypes = Array.Empty<BlueprintArchetypeReference>();
                    cfg.m_FeatureList = Array.Empty<BlueprintFeatureReference>();
                })
                .Configure();
        }
    }
}
