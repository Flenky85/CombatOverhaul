using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class TouchOfSlimeCastAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.TouchOfSlimeCast)
                .SetDescriptionValue(
                    "You create a coating of slime on your hand. When you make a successful melee touch attack with the slime, " +
                    "it pulls free of you and sticks to the target, at which point it acts like green slime, dealing 1d3 points of " +
                    "Constitution damage per round. Freezing, burning, and remove disease destroys this slime. It cannot transfer to " +
                    "a creature other than the original target, and it dies if separated from the original target or if the target dies. " +
                    "Only a living creature can be the target of this spell.\n" +
                    "Additionally, the targets takes 1d4 points of acid damage per caster level (maximum 10d4). A successful " +
                    "Fortitude save halves this damage."
                )
                .Configure();
        }
    }
}
