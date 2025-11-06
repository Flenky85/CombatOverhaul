using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class ChainLightningAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ChainLightning)
                .EditComponents<ContextRankConfig>(
                    cfg =>
                    {
                        cfg.m_UseMax = true;
                        cfg.m_Max = 14;
                    },
                    cfg => cfg.name == "$ContextRankConfig$3bae59db-04a8-4774-8313-5986348d3184"
                )
                .EditComponents<ContextRankConfig>(
                    cfg =>
                    {
                        cfg.m_UseMax = true;
                        cfg.m_Max = 14;
                    },
                    cfg => cfg.name == "$AbilityRankConfig$ae4fc325-1405-492d-a34b-bc626af39e24"
                )
                .SetDescriptionValue(
                    "This spell creates an electrical discharge that begins as a single stroke commencing from your fingertips. " +
                    "Unlike lightning bolt, chain lightning strikes one object or creature initially, then arcs to other targets.\n" +
                    "The bolt deals 1d6 points of electricity damage per caster level(maximum 14d6) to the primary target.After it " +
                    "strikes, lightning can arc to a number of secondary targets equal to your caster level(maximum 14).The secondary " +
                    "bolts each strike one target and deal as much damage as the primary bolt.\n" + 
                    "Each target can attempt a Reflex saving throw for half damage. Secondary targets must be within 30 feet of the " +
                    "primary target, and no target can be struck more than once."
                )
                .Configure();
        }
    }
}
