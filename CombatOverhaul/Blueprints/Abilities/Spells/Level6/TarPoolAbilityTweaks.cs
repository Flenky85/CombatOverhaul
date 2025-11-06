using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class TarPoolAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.TarPool)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];
                    dmg.Value.DiceType = DiceType.D3;
                })
                .EditComponent<ContextRankConfig>(r =>
                {
                    if (r.m_Type == AbilityRankType.DamageDice)
                    {
                        r.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        r.m_Progression = ContextRankProgression.AsIs; 
                        r.m_UseMax = true;
                        r.m_Max = 14;                         
                    }
                })
                .EditComponent<AdditionalAbilityEffectRunActionOnClickedTarget>(comp =>
                {
                    var spawn = (ContextActionSpawnAreaEffect)comp.Action.Actions[0];
                    spawn.DurationValue.Rate = DurationRate.Rounds;
                    spawn.DurationValue.DiceType = DiceType.D3;
                    spawn.DurationValue.DiceCountValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 2 
                    };
                    spawn.DurationValue.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0
                    };
                    spawn.DurationValue.m_IsExtendable = false;
                })
                .SetDuration2d3RoundsShared()
                .SetDescriptionValue(
                    "You convert a layer of the ground into hot tar. Creatures in the area when the tar appears take 1d3 " +
                    "points of fire damage per caster levels (maximum of 14d3) and must succeed at a Reflex save or become " +
                    "entangled until they make a successful Reflex saving throw, Strength check, or Mobility check (DC = " +
                    "spell saving throw DC). They can make attempts each round after receiving the effect. The area is " +
                    "difficult terrain and all creatures inside the area take a –5 penalty on Mobility skill checks. All " +
                    "creatures inside the tar area take 2d6 points of fire damage each round and must reattempt a Reflex " +
                    "save or become entangled. A creature that falls prone in the area takes a –4 penalty on its Reflex " +
                    "save against the tar and on Strength and Mobility checks to escape the tar. After leaving the tar " +
                    "area, a creature can take no actions for 1 round as it cleans itself from the tar."
                )
                .Configure();
        }
    }
}
