using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanFrostSpiritGreaterAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanFrostSpiritGreaterAbility)
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
                    "The shaman gains cold resistance 10. In addition, as a standard action, she can summon an icy " +
                    "blast in a 20-foot-radius burst originating from a point she can see within 30 feet. This blast " +
                    "deals cold damage equal to 1d6 per two shaman level she has to each creature caught in the burst. " +
                    "Each target can attempt a Reflex saving throw to halve this damage.\n" +
                    "After using this ability, the shaman must wait 5 rounds before she can use it again."
                )
                .Configure();
        }
    }
}
