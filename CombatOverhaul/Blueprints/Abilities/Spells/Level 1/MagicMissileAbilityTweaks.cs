using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class MagicMissileAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.MagicMissile)
                .EditComponent<AbilityDeliverProjectile>(p =>
                {
                    p.UseMaxProjectilesCount = true;
                    p.MaxProjectilesCountRank = AbilityRankType.Default;
                })
                .EditComponents<ContextRankConfig>(cfg =>
                {
                    cfg.m_Progression = ContextRankProgression.AsIs; 
                    cfg.m_UseMax = true;
                    cfg.m_Max = 4;                                   
                },
                cfg => cfg.m_Type == AbilityRankType.Default)
                .SetDescriptionValue(
                    "A missile of magical energy darts forth from your fingertip and strikes its target, dealing 1d4+1 points " +
                    "of force damage.\n" +
                    "The missile strikes unerringly, even if the target is in melee combat, so long as it has less than total " +
                    "cover or total concealment. Specific parts of a creature can't be singled out. Objects are not damaged by " +
                    "the spell.\n" +
                    "You fire one missile per caster level to a maximum of four missiles."
                )
                .Configure();
        }
    }
}
