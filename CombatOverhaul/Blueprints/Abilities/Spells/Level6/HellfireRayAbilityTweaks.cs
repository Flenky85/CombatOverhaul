using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level6
{
    [AutoRegister]
    internal static class HellfireRayAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HellfireRay)
                .EditComponents<AbilityDeliverProjectile>(
                    ap => { ap.UseMaxProjectilesCount = false; },
                    ap => ap.name == "$AbilityDeliverProjectile$5c5285a2-e7cc-4a29-b587-937e857f3e02"
                )
                .EditComponents<ContextRankConfig>(
                    rc =>
                    {
                        rc.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        rc.m_Progression = ContextRankProgression.DivStep;
                        rc.m_StepLevel = 2;
                        rc.m_UseMax = true;
                        rc.m_Max = 7;
                        rc.m_AffectedByIntensifiedMetamagic = false;
                    },
                    rc => rc.name == "$ContextRankConfig$37de05d9-15a8-4d58-bc1b-a3069b238bb5"
                )
                .SetDescriptionValue(
                    "A blast of hellfire blazes from your hands. You fire three ray. Each ray requires a ranged touch " +
                    "attack to hit and deals 1d6 points of damage per two caster level (maximum 7d6). Half the damage is " +
                    "fire damage, but the other half results directly from unholy power and is therefore not subject to " +
                    "being reduced by fire resistance."
                )
                .Configure();
        }
    }
}
