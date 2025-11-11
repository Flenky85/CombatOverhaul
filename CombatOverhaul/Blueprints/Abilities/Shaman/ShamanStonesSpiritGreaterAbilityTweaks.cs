using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanStonesSpiritGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanStonesSpiritGreaterAbility)
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
                    "The shaman gains DR 2/adamantine. This DR increases by 1 for every 4 levels beyond 8th the shaman possesses. " +
                    "In addition, as a standard action, she can cause jagged pieces of stone to explode from her body in a " +
                    "10-foot-radius burst. This deals 1d6 points of piercing damage per 2 shaman levels she possesses. " +
                    "A successful Reflex saving throw halves this damage. " +
                    "After using this ability, the shaman must wait 5 rounds before she can use it again."
                )
                .Configure();
        }
    }
}
