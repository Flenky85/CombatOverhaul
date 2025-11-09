using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class TidalSurgeLineAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.TidalSurgeLine)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var sv = (ContextActionSavingThrow)c.Actions.Actions[0];
                    var dmg = (ContextActionDealDamage)sv.Actions.Actions[0];

                    dmg.Value.DiceType = DiceType.D8;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank
                    };
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    cfg.m_Progression = ContextRankProgression.AsIs;
                    cfg.m_UseMax = true;
                    cfg.m_Max = 12;
                    cfg.m_AffectedByIntensifiedMetamagic = false;
                })
                .Configure();
        }
    }
}
