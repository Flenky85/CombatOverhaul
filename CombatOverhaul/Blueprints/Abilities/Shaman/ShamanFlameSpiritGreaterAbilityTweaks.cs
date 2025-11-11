using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanFlameSpiritGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanFlameSpiritGreaterAbility)
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
                    "The shaman gains fire resistance 10. In addition, as a standard action she can unleash a 15-foot cone of flame from her mouth," +
                    " dealing 1d6 points of fire damage per shaman level she possesses. A successful Reflex saving throw halves this damage. " +
                    "After using this ability, the shaman must wait 5 rounds before she can use it again."
                )
                .Configure();
        }
    }
}
