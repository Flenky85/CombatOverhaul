using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Hellknight
{
    [AutoRegister]
    internal static class HellknightDisciplineWrackAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HellknightDisciplineWrackAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[1];
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
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_Class = new[] { BlueprintTool.GetRef<BlueprintCharacterClassReference>("ed246f1680e667b47b7427d51e651059") };
                    c.m_UseMin = false;
                    c.m_UseMax = true;
                    c.m_Max = 10;  
                })
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDescriptionValue(
                    "The Hellknight can make a touch attack as a standard action to cause a creature to suffer incredible pain. " +
                    "The creature touched takes damage equal to 1d4 per Hellknight level, and must succeed at a Will save or become staggered for 1d4 rounds.\n" +
                    "This ability has a cooldown of 6 rounds."
                )
                .Configure();
        }
    }
}
