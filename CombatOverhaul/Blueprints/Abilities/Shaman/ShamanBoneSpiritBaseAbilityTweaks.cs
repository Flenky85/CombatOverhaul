using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanBoneSpiritBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanBoneSpiritBaseAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var conditional = (Conditional)c.Actions.Actions[0];

                    var heal = (ContextActionHealTarget)conditional.IfTrue.Actions[0];
                    heal.Value.DiceType = DiceType.D4;
                    heal.Value.DiceCountValue.ValueType = ContextValueType.Rank;   
                    heal.Value.DiceCountValue.Value = 0;
                    heal.Value.BonusValue.ValueType = ContextValueType.Simple;
                    heal.Value.BonusValue.Value = 0;                              

                    var damage = (ContextActionDealDamage)conditional.IfFalse.Actions[0];
                    damage.Value.DiceType = DiceType.D8;
                    damage.Value.DiceCountValue.ValueType = ContextValueType.Rank; 
                    damage.Value.DiceCountValue.Value = 0;
                    damage.Value.BonusValue.ValueType = ContextValueType.Simple;
                    damage.Value.BonusValue.Value = 0;
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 3;
                })
                .SetDescriptionValue(
                    "As a standard action, the shaman can make a melee touch attack infused with negative energy that deals 1d8 points " +
                    "of damage for every 2 shaman levels she possesses. She can instead touch an undead creature to heal 1d4 points of " +
                    "damage for every 2 shaman levels she possesses. At 11th level, any weapon that the shaman wields is treated as an unholy weapon.\n" +
                    "Activating this ability expends 3 charges. The shaman has a number of charges equal to " +
                    "3 plus her Charisma modifier. At the start of each of her turns, she regains 1."
                )
                .Configure();
        }
    }
}
