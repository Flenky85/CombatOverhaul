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
    internal static class StinkingCloudAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.StinkingCloud)
                .EditComponent<AdditionalAbilityEffectRunActionOnClickedTarget>(c =>
                {
                    var spawn = (ContextActionSpawnAreaEffect)c.Action.Actions[0];
                    var extendable = spawn.DurationValue.m_IsExtendable;

                    spawn.DurationValue = new ContextDurationValue
                    {
                        m_IsExtendable = extendable,
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.D3,
                        DiceCountValue = ContextValues.Constant(2),
                        BonusValue = ContextValues.Constant(0)
                    };
                })
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
