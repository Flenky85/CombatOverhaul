using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.AreaLogic.Cutscenes;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using System.Threading;
using static Kingmaker.RuleSystem.RulebookEvent;


namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanWavesSpiritBaseAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanWavesSpiritBaseAbility)
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
                    "The shaman can unleash a torrent of ice and water from her hands in a 15 - foot cone as a " +
                    "standard action.This torrent deals 1d4 points of cold damage per 2 shaman levels she possesses, " +
                    "and bull rushes every enemy, using Charisma modifier for the roll. A successful Reflex saving " +
                    "throw halves the damage and negates the bull rush.\n" +
                    "Activating this ability expends 3 charges. The shaman has a number of charges equal to " +
                    "3 plus her Charisma modifier. At the start of each of her turns, she regains 1."
                )
                .Configure();
        }
    }
}
