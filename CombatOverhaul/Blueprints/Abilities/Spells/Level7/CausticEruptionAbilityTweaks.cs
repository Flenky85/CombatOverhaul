using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class CausticEruptionAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.CausticEruption)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var save = (ContextActionSavingThrow)c.Actions.Actions[0];
                    var dmg = (ContextActionDealDamage)save.Actions.Actions[0];

                    dmg.Value.DiceType = DiceType.D4;
                    dmg.Value.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,          
                        ValueRank = AbilityRankType.Default       
                    };
                    dmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                })
                .EditComponent<ContextRankConfig>(c =>
                {
                    if (c.m_Type == AbilityRankType.Default)
                    {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.AsIs;
                        c.m_UseMax = true;
                        c.m_Max = 16;                             
                        c.m_AffectedByIntensifiedMetamagic = false; 
                    }
                })
                .SetDescriptionValue(
                    "Acid erupts from your space in all directions, causing 1d4 points of damage per caster level " +
                    "(maximum 14d4) to creatures and unattended objects in the area. Each of the next 2 rounds, " +
                    "creatures and objects that failed their saves against the initial burst take an additional " +
                    "1d4 points of acid damage per 2 caster levels (maximum 8d4) unless the acid is dispelled."
                )
                .Configure();
        }
    }
}
