using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class BurningArcAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.BurningArc)
                .EditComponent<ContextRankConfig>(c =>
                {
                    if (c.m_Type == AbilityRankType.DamageDice)
                    {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.AsIs;
                        c.m_UseMax = true;
                    }

                    if (c.m_Type == AbilityRankType.DamageDiceAlternative)
                    {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.Div2;
                        c.m_UseMax = true;
                        c.m_Max = 3;   
                    }

                    if (c.m_Type == AbilityRankType.ProjectilesCount)
                    {
                        c.m_BaseValueType = ContextRankBaseValueType.CasterLevel;
                        c.m_Progression = ContextRankProgression.OnePlusDivStep;
                        c.m_StartLevel = 1;
                        c.m_StepLevel = 2;   
                        c.m_UseMax = true;
                        c.m_Max = 4;   
                    }
                })
                .SetDescriptionValue(
                    "This spell causes an arc of flame to leap from your fingers, burning one enemy nearby " +
                    "plus one additional enemy per 2 caster levels (maximum 3 additional enemies at 6th " +
                    "level). Each additional target must be within 15 feet of the primary target. It deals " +
                    "1d6 points of fire damage per caster level (maximum 6d6) to the primary target. Each " +
                    "additional target takes half as many damage dice as the primary target (rounded down). " +
                    "Each target can attempt a Reflex saving throw for half damage."
                )
                .Configure();
        }
    }
}
