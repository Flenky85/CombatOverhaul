using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level8
{
    [AutoRegister]
    internal static class RiftOfRuinAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.RiftOfRuin)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var spawn = (ContextActionSpawnAreaEffect)c.Actions.Actions[0];

                    spawn.DurationValue = new ContextDurationValue
                    {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.D3,
                        DiceCountValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 2
                        },
                        BonusValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 0
                        },
                        m_IsExtendable = true
                    };
                })
                .SetDuration2d3RoundsShared()
                .Configure();
        }
    }
}
