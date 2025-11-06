using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level4
{
    [AutoRegister]
    internal static class DragonsBreathBrassAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.DragonsBreathBrass)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var deal = (ContextActionDealDamage)c.Actions.Actions[0];
                    deal.Value.DiceType = DiceType.D8;
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_Max = 10;                 
                })
                .SetDescriptionValue(
                    "You breathe out a stream of fire in the form of a 60-foot line. Creatures in the affected " +
                    "area take 1d8 points of energy damage per caster level (maximum of 10d8). A successful Reflex " +
                    "save halves the damage. The spell's effect and energy type depend on the type of dragon."
                )
                .Configure();
        }
    }
}
