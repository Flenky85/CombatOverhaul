using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level5
{
    [AutoRegister]
    internal static class AcidicSprayAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.AcidicSpray)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_Max = 12; 
                })
                .SetDescriptionValue(
                    "A spray of acid erupts from your outstretched hand, dealing 1d6 points of acid damage per " +
                    "caster level (maximum 12d6) to each creature within its area (Reflex half). This acid " +
                    "continues to burn for 1 round. At the end of this round every creatures that failed their " +
                    "saving throw against the spell must succeed on another Reflex save or receive 1d6 points of " +
                    "acid damage per two caster levels (maximum 6d6)."
                )
                .Configure();
        }
    }
}
