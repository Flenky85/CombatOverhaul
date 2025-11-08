/*using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level9
{
    [AutoRegister]
    internal static class ImplosionSelfAddActionBuffAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ImplosionSelfAddActionBuff)
                .EditComponent<ContextRankConfig>(rc =>
                {
                    rc.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                    rc.m_Progression = ContextRankProgression.AsIs;
                    rc.m_UseMax = true;
                    rc.m_Max = 20;
                })
                .EditComponent<AddFactContextActions>(c =>
                {
                    var dmg0 = (ContextActionDealDamage)c.Activated.Actions[0];
                    dmg0.Value.DiceType = DiceType.D6;
                    dmg0.Value.DiceCountValue = new ContextValue { ValueType = ContextValueType.Rank };
                    dmg0.Value.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "Each round, including immediately upon casting the spell, you can cause one creature to collapse in " +
                    "on itself, inflicting 1d6 points of damage per caster level (20d6 maximun) (Fortitude saving throw negates). " +
                    "Choosing a new target for this spell is a move action. You can target a particular creature only once with each " +
                    "casting of the spell. Implosion has no effect on creatures with no material body or on incorporeal creatures."
                )
                .Configure();
        }
    }
}
*/