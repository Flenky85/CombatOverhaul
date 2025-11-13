using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class DrunkenTechniqueFirewaterBreathAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DrunkenTechniqueFirewaterBreathAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (ContextActionConditionalSaved)c.Actions.Actions[0];
                    var dmg = (ContextActionDealDamage)cond.Failed.Actions[0];
                    dmg.Value.DiceType = DiceType.D4; 
                })
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 4; })
                .SetDescriptionValue(
                    "At 4th level, a drunken master can take a drink and expel a gout of alcohol-fueled fire in a " +
                    "15-foot cone. Creatures within the cone take 1d4 points of fire damage per monk level. A successful " +
                    "Reflex saving throw (DC 10 + 1/2 the monk's level + the monk's Wisdom modifier) halves the damage. " +
                    "Using this ability is a standard action that consumes 4 points from the monk's drunken ki pool.\n" +
                    "This ability replaces still mind."
                )
                .Configure();
        }
    }
}
