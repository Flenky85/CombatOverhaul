using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanWavesSpiritGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanWavesSpiritGreaterAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];
                    dmg.Value.DiceType = DiceType.D6; 
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 5;
                })
                .SetDescriptionValue(
                    "The shaman can unleash a torrent of ice and water from her hands in a 15 - foot cone as a standard " +
                    "action.This torrent deals 1d4 points of cold damage per 2 shaman levels she possesses, " +
                    "and bull rushes every enemy, using Charisma modifier for the roll. A successful Reflex " +
                    "saving throw halves the damage and negates the bull rush.\n" +
                    "After using this ability, the shaman must wait 5 rounds before she can use it again."
                )
                .Configure();
        }
    }
}
