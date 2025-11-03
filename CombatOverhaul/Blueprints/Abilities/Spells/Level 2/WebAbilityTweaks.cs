using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Paladin
{
    [AutoRegister]
    internal static class WebAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.Web)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var spawn = (ContextActionSpawnAreaEffect)c.Actions.Actions[0];
                    var extendable = spawn.DurationValue.m_IsExtendable;
                    
                    spawn.DurationValue = new ContextDurationValue
                    {
                        m_IsExtendable = extendable,
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.D4,
                        DiceCountValue = ContextValues.Constant(2),
                        BonusValue = ContextValues.Constant(0)
                    };
                })
                .SetDuration2d4RoundsShared()
                .Configure();
        }
    }
}
