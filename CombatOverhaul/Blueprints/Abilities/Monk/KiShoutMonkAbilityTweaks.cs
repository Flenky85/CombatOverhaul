using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class KiShoutMonkAbilityTweaks
    {
        public static void Register()
        {
            var abilites = new[]
            {
                AbilitiesGuids.KiShoutMonk,
                AbilitiesGuids.DrunkenKiShout,
                AbilitiesGuids.ScaledFistShout,
            };
            foreach (var id in abilites)
            {
                AbilityConfigurator.For(id)
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_UseMax = true;
                    c.m_Max = 16;
                    c.m_AffectedByIntensifiedMetamagic = false;
                })
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 6; })
                .SetDescriptionValue(
                    "A monk with this ki power can spend 6 points from his ki pool as a standard action to unleash a " +
                    "sudden blast of sonic energy that strikes his opponent.The target takes 1d6 points of sonic damage " +
                    "per level(maximum 16d6) and is stunned for 1 round; a successful Fortitude save reduces " +
                    "the damage by half and negates the stun."
                )
                .Configure();
            }
        }
    }
}
