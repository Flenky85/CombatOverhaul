using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanWeaponSwitchAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanWeaponSwitchAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var root = (Conditional)c.Actions.Actions[0];

                    var trueEnchant = (ContextActionWeaponEnchantPool)root.IfTrue.Actions[0];
                    trueEnchant.DurationValue.Rate = DurationRate.Rounds;
                    trueEnchant.DurationValue.DiceType = DiceType.Zero;
                    trueEnchant.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    trueEnchant.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 };
                    trueEnchant.DurationValue.m_IsExtendable = true;

                    var trueBuff = (ContextActionApplyBuff)root.IfTrue.Actions[1];
                    trueBuff.DurationValue.Rate = DurationRate.Rounds;
                    trueBuff.DurationValue.DiceType = DiceType.Zero;
                    trueBuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    trueBuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 };
                    trueBuff.DurationValue.m_IsExtendable = true;

                    var falseEnchant = (ContextActionWeaponEnchantPool)root.IfFalse.Actions[0];
                    falseEnchant.DurationValue.Rate = DurationRate.Rounds;
                    falseEnchant.DurationValue.DiceType = DiceType.Zero;
                    falseEnchant.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    falseEnchant.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 };
                    falseEnchant.DurationValue.m_IsExtendable = true;

                    var falseBuff = (ContextActionApplyBuff)root.IfFalse.Actions[1];
                    falseBuff.DurationValue.Rate = DurationRate.Rounds;
                    falseBuff.DurationValue.DiceType = DiceType.Zero;
                    falseBuff.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    falseBuff.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 3 };
                    falseBuff.DurationValue.m_IsExtendable = true;
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 3; 
                })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "At 1st level a spirit hunter can, as a swift action, grant a +1 enhancement bonus for 1 minute to any weapon she " +
                    "is holding. For every four levels beyond 1st, the weapon gains another +1 enhancement bonus, to a maximum of +5 " +
                    "at 17th level. These bonuses can be added to the weapon, stacking with existing weapon enhancement to a maximum " +
                    "of +5. Effects of multiple uses of this ability don't stack.\n" +
                    "At 1st level, the bonus can be used to add ghost touch special ability to the weapon.\n" +
                    "At 5th level, these bonuses can be used to add any of the following weapon special abilities: flaming, frost, keen, shock, or speed.\n" +
                    "Adding these properties consumes an amount of bonus equal to the property's base price modifier. " +
                    "These special abilities are added to any the weapon already has, but duplicates don't stack.If the " +
                    "weapon is not magical, at least a + 1 enhancement bonus must be added before any other special " +
                    "abilities can be added.These bonuses and special abilities are decided when the ability is used " +
                    "and cannot be changed until the next time the spirit hunter uses this ability.These bonuses do " +
                    "not function if the weapon is wielded by anyone other than the spirit hunter.\n" +
                    "A spirit hunter can only enhance one weapon at a time with this ability.If she uses this ability again, the first use immediately ends.\n" +
                    "This ability uses 3 charges and grants the " +
                    "a buff that lasts for 3 rounds.The class begins with 3 charges; at 10th level and " +
                    "20th level she gains 3 additional charges each time (for a total of 6 and 9 charges, " +
                    "respectively). At the start of each of her turns, she regains 1 expended charge, but only " +
                    "while the buff is not active."
                )
                .Configure();
        }
    }
}
