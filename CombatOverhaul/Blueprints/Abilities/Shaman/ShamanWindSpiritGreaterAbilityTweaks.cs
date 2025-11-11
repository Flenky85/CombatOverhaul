using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanWindSpiritGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanWindSpiritGreaterAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];
                    dmg.Value.DiceType = DiceType.D6;
                })
                .EditComponent<ContextRankConfig>(cfg =>
                {
                    cfg.m_Progression = ContextRankProgression.DivStep; 
                    cfg.m_StepLevel = 2;
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 5;
                })
                .SetDescriptionValue(
                    "The shaman gains electricity resistance 10. In addition, as a standard action she can unleash a " +
                    "20-foot line of sparks from her fingertips, dealing 1d6 points of electricity damage per two shaman " +
                    "level she possesses. A successful Reflex saving throw halves this damage.\n" +
                    "After using this ability, the shaman must wait 5 rounds before she can use it again."
                )
                .Configure();
        }
    }
}
