using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Components;


namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class ElementalAssessorAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ElementalAssessor)
                .EditComponents<ContextCalculateSharedValue>(
                    c => { c.Value.DiceType = DiceType.D8; },
                    c => c.name == "$ContextCalculateSharedValue$2eafcdfe-a990-4f59-9931-4d350f604535"
                )
                .SetDescriptionValue(
                    "Azata champions developed this spell to deal with fiends with unknown resistances. A ray of spiraling colors springs " +
                    "from your hand and streaks to its target.\n" +
                    "You must make a successful ranged touch attack to hit your target with the ray, which deals 2d8 points of acid damage, " +
                    "2d8 points of cold damage, 2d8 points of electricity damage, and 2d8 points of fire damage.\n" +
                    "The type of energy that does the most damage to the target then persists, dealing another 4d8 points of that damage " +
                    "type per round for 1d4 rounds."
                )
                .Configure();
        }
    }
}
