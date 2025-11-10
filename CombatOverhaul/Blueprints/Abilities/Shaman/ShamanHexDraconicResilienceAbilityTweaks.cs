using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexDraconicResilienceAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexDraconicResilienceAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<ContextRankConfig>(c =>
                {
                    c.m_BaseValueType = ContextRankBaseValueType.ClassLevel;
                    c.m_Progression = ContextRankProgression.StartPlusDivStep;
                    c.m_StartLevel = -5;
                    c.m_StepLevel = 5;
                    c.m_UseMax = true;
                    c.m_Max = 6;
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.DurationValue.Rate = DurationRate.Rounds;
                })
                .SetDescriptionValue(
                    "The shaman grants a creature she touches some of the magically resilient nature " +
                    "of dragons, causing the creature to be immune to magical sleep effects for a two " +
                    "rounds plus one aditional every five shaman's level. At 7th level, the creature is also immune to " +
                    "paralysis for this duration. Once a creature has benefited from the draconic resilience " +
                    "hex, it cannot benefit from this hex again on new combat."
                )
                .Configure();
        }
    }
}
