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
    internal static class ShamanFlameSpiritBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanFlameSpiritBaseAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];
                    dmg.Value.DiceType = DiceType.D8;
                    dmg.Value.DiceCountValue.ValueType = ContextValueType.Rank;
                    dmg.Value.DiceCountValue.Value = 0;
                    dmg.Value.BonusValue.ValueType = ContextValueType.Simple;
                    dmg.Value.BonusValue.Value = 0;
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 3;
                })
                .SetDescriptionValue(
                    "As a standard action, the shaman can make a melee touch attack that deals 1d8 points of fire damage " +
                    "point for every 2 shaman levels she possesses. At 11th level, any weapon she wields is treated as a flaming weapon.\n" +
                    "Activating this ability expends 3 charges. The shaman has a number of charges equal to " +
                    "3 plus her Charisma modifier. At the start of each of her turns, she regains 1."
                )
                .Configure();
        }
    }
}
