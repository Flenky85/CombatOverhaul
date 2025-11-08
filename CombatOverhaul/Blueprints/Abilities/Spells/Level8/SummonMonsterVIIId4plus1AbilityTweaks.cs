using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level8
{
    [AutoRegister]
    internal static class SummonMonsterVIIId4plus1AbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SummonMonsterVIIId4plus1)
                .SetActionType(UnitCommand.CommandType.Standard)  
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var cond = (Conditional)c.Actions.Actions[0];
                    var spawnIfTrue = (ContextActionSpawnMonster)cond.IfTrue.Actions[0];
                    var spawnIfFalse = (ContextActionSpawnMonster)cond.IfFalse.Actions[0];
                    var fixed6 = new ContextDurationValue
                    {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.Zero,
                        DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 },
                        BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 },
                        m_IsExtendable = false
                    };

                    spawnIfTrue.DurationValue = fixed6;
                    spawnIfFalse.DurationValue = fixed6;
                })
                .SetDuration6RoundsShared()
                .Configure();
        }
    }
}
