using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level2
{
    [AutoRegister]
    internal static class AidBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.AidBuff)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_Type = AbilityRankType.StatBonus;
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    r.m_Progression = ContextRankProgression.AsIs;
                    r.m_UseMax = true;
                    r.m_Max = 6; 
                })
                .AddContextCalculateSharedValue(
                    value: new ContextDiceValue
                    {
                        DiceType = DiceType.D2,
                        DiceCountValue = ContextValues.Rank(AbilityRankType.StatBonus),
                        BonusValue = ContextValues.Constant(0)
                    },
                    valueType: AbilitySharedValue.Damage
                )
                .EditComponent<TemporaryHitPointsRandom>(thp =>
                {
                    thp.Descriptor = ModifierDescriptor.UntypedStackable; 
                    thp.Dice = new DiceFormula(0, DiceType.D2);          
                    thp.Bonus = ContextValues.Shared(AbilitySharedValue.Damage); 
                    thp.ScaleBonusByCasterLevel = false;                 
                })
                .Configure();
        }
    }
}
