using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level7
{
    [AutoRegister]
    internal static class CausticEruptionBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.CausticEruptionBuff)
                .EditComponent<AddFactContextActions>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.NewRound.Actions[0];
                    dmg.Value.DiceType = DiceType.D4; 
                })
                .EditComponents<ContextRankConfig>(
                    rc =>
                    {
                        rc.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        rc.m_Progression = ContextRankProgression.Div2;
                        rc.m_UseMax = true;
                        rc.m_Max = 8;          
                        rc.m_AffectedByIntensifiedMetamagic = false;
                    },
                    rc => rc.name == "$ContextRankConfig$1d09ea44-c17e-42cc-818a-577cdf295649"
                )
                .Configure();
        }
    }
}
