using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level7
{
    [AutoRegister]
    internal static class DestructionAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Destruction)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    r.m_Progression = ContextRankProgression.AsIs;
                    r.m_UseMax = true;
                    r.m_Max = 16;
                    r.m_AffectedByIntensifiedMetamagic = false;
                })
                .EditComponent<AbilityEffectRunAction>(e =>
                {
                    var save = (ContextActionConditionalSaved)e.Actions.Actions[0];
                    var onSave = (ContextActionDealDamage)save.Succeed.Actions[0];
                    onSave.Value.DiceType = DiceType.D3;
                    onSave.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank };
                    onSave.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    onSave.Half = false;
                    onSave.HalfIfSaved = false;

                    var onFail = (ContextActionDealDamage)save.Failed.Actions[0];
                    onFail.Value.DiceType = DiceType.D10;
                    onFail.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank };
                    onFail.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    onFail.Half = false;
                    onFail.HalfIfSaved = false;
                })
                .SetDescriptionValue(
                    "This spell instantly delivers 1d10 points of damage per caster level (16d10 maximun). If the target's Fortitude saving throw " +
                    "succeeds, it instead takes 1d3 points of damage per caster lvl (16d3 maximun)."
                )
                .Configure();
        }
    }
}
