using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Hellknight
{
    [AutoRegister]
    internal static class HellknightDisciplineTrackAbility2Tweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.HellknightDisciplineTrackAbility2)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var spawn = (ContextActionSpawnMonster)c.Actions.Actions[0];
                    spawn.DurationValue.Rate = DurationRate.Rounds;
                    spawn.DurationValue.DiceType = DiceType.Zero;
                    spawn.DurationValue.DiceCountValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 0 };
                    spawn.DurationValue.BonusValue = new ContextValue { ValueType = ContextValueType.Simple, Value = 6 };
                    spawn.DurationValue.m_IsExtendable = false;
                })
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 6; })
                .SetDuration3RoundsShared()
                .SetDescriptionValue(
                    "The Hellknight can summon a creature to aid him in battle, as if using a summon monster spell, " +
                    "save that the summoned creature lingers for 6 rounds before vanishing. A Hellknight can summon a wolf. " +
                    "A 9th-level Hellknight can summon a hell hound.\n" +
                    "This ability has a cooldown of 6 rounds."
                )
                .Configure();
        }
    }
}
