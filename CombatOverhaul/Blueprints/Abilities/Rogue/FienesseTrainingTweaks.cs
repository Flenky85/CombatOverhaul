using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Abilities.Rogue
{
    [AutoRegister]
    internal static class FienesseTrainingTweaks
    {
        public static void Register()
        {
            var buffs = new[]
            {
                AbilitiesGuids.FinesseTrainingBite,
                AbilitiesGuids.FinesseTrainingClaw,
                AbilitiesGuids.FinesseTrainingDagger,
                AbilitiesGuids.FinesseTrainingDuelingSword,
                AbilitiesGuids.FinesseTrainingElvenCurvedBlade,
                AbilitiesGuids.FinesseTrainingEstoc,
                AbilitiesGuids.FinesseTrainingGore,
                AbilitiesGuids.FinesseTrainingHandaxe,
                AbilitiesGuids.FinesseTrainingHoof,
                AbilitiesGuids.FinesseTrainingKama,
                AbilitiesGuids.FinesseTrainingKukri,
                AbilitiesGuids.FinesseTrainingLightHammer,
                AbilitiesGuids.FinesseTrainingLightMace,
                AbilitiesGuids.FinesseTrainingLightPick,
                AbilitiesGuids.FinesseTrainingLightShield,
                AbilitiesGuids.FinesseTrainingNunchaku,
                AbilitiesGuids.FinesseTrainingPunchingDagger,
                AbilitiesGuids.FinesseTrainingRapier,
                AbilitiesGuids.FinesseTrainingSai,
                AbilitiesGuids.FinesseTrainingSawtoothSabre,
                AbilitiesGuids.FinesseTrainingShortsword,
                AbilitiesGuids.FinesseTrainingSickle,
                AbilitiesGuids.FinesseTrainingSlam,
                AbilitiesGuids.FinesseTrainingSpike,
                AbilitiesGuids.FinesseTrainingSpikedLightShield,
                AbilitiesGuids.FinesseTrainingStarknife,
                AbilitiesGuids.FinesseTrainingTail,
                AbilitiesGuids.FinesseTrainingTalon,
                AbilitiesGuids.FinesseTrainingTentacle,
                AbilitiesGuids.FinesseTrainingUnarmed,
                AbilitiesGuids.FinesseTrainingWing,
            };

            foreach (var id in buffs)
            {
                AbilityConfigurator.For(id)
                    .SetDescriptionValue(
                        "Whenever the rogue makes a successful melee attack with the selected weapon, " +
                        "she adds her Dexterity modifier to the damage roll. If any effect would prevent " +
                        "adding an ability modifier to the damage roll, she does not add her Dexterity modifier."
                    )
                    .Configure();
            }
        }
    }
}
