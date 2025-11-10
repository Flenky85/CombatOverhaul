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
    internal static class ShamanHexAmelioratingSickenedAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexAmelioratingSickenedAbility)
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
                    "The shaman can touch a creature to protect it from negative conditions and suppress their effects. " +
                    "The shaman chooses one of the following conditions each time she uses this hex: dazzled, fatigued, " +
                    "shaken, or sickened. If the target is afflicted with the chosen condition, that condition is suppressed " +
                    "for two rounds and an aditional round per 5th shaman's level. Additionally, the shaman grants her target a +4 " +
                    "circumstance bonus on saving throws against effects that cause the chosen conditions for one combat. " +
                    "A creature can benefit from the hex twice per combat, once for each of two different conditions."
                )
                .Configure();
        }
    }
}
