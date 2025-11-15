using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class SenseiAdviceDiamondsAbilityTweaks
    {
        public static void Register()
        {
            var abilites = new[]
            {
                AbilitiesGuids.SenseiAdviceDiamondBodyMass,
                AbilitiesGuids.SenseiAdviceDiamondSoulMass,
            };
            foreach (var id in abilites)
            {
                AbilityConfigurator.For(id)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var apply = (ContextActionApplyBuff)c.Actions.Actions[0];
                    apply.DurationValue.Rate = DurationRate.Rounds;
                    apply.DurationValue.DiceType = DiceType.Zero;
                    apply.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    apply.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                    apply.DurationValue.m_IsExtendable = false;
                })
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDuration6RoundsShared()
                .Configure();
            }
        }
    }
}
