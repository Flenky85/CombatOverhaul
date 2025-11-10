using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanHexFireNimbusAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanHexFireNimbusAbility)
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
                .SetDescriptionValue(
                    "The shaman causes a creature within 30 feet to gain a nimbus of fire. Though this doesn't harm the creature, " +
                    "it does cause the creature to emit light like a torch, preventing it from gaining any benefit from concealment " +
                    "or invisibility. The target also takes a –2 penalty on saving throws against spells or effects with fire descriptor. " +
                    "The fire nimbus lasts for two rounds plus one aditional every five shaman's level. A successful Will saving throw negates this " +
                    "effect. Whether or not the save is successful, the creature cannot be the target of this hex again on new combat."
                )
                .Configure();
        }
    }
}
