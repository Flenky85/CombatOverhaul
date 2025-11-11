using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanBattleSpiritTrueAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ShamanBattleSpiritTrueAbility)
                .SetActionType(UnitCommand.CommandType.Swift)
                .SetIsFullRoundAction(false)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var actions = c.Actions.Actions;

                    var buff0 = (ContextActionApplyBuff)actions[0];
                    buff0.DurationValue.Rate = DurationRate.Rounds;
                    buff0.DurationValue.BonusValue.ValueType = ContextValueType.Simple;
                    buff0.DurationValue.BonusValue.Value = 3;

                    var buff1 = (ContextActionApplyBuff)actions[1];
                    buff1.DurationValue.Rate = DurationRate.Rounds;
                    buff1.DurationValue.BonusValue.ValueType = ContextValueType.Simple;
                    buff1.DurationValue.BonusValue.Value = 3;

                    var controlBuff = new ContextActionApplyBuff
                    {
                        m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(
                                BuffsGuids.ParagonBattleBuff
                            ),

                        Permanent = buff0.Permanent,
                        UseDurationSeconds = buff0.UseDurationSeconds,
                        DurationValue = buff0.DurationValue,
                        DurationSeconds = buff0.DurationSeconds,
                        IsFromSpell = buff0.IsFromSpell,
                        IsNotDispelable = buff0.IsNotDispelable,
                        ToCaster = buff0.ToCaster,
                        AsChild = buff0.AsChild,
                        SameDuration = buff0.SameDuration,
                        NotLinkToAreaEffect = buff0.NotLinkToAreaEffect,
                        IgnoreParentContext = buff0.IgnoreParentContext
                    };
                    controlBuff.DurationValue.Rate = DurationRate.Rounds;
                    controlBuff.DurationValue.BonusValue.ValueType = ContextValueType.Simple;
                    controlBuff.DurationValue.BonusValue.Value = 3;

                    c.Actions.Actions = new GameAction[]
                    {
                        buff0,
                        buff1,
                        controlBuff
                    };
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 3;
                })
                .SetDescriptionValue(
                    "As a swift action, the shaman assumes a form that combines the effects of enlarge person and divine " +
                    "power for 3 rounds. This ability uses a pool of charges. Activating this form expends 3 charges. " +
                    "The shaman has a number of charges equal to 3 plus her Charisma modifier. At the start of each of " +
                    "her turns, she regains 1 expended charge, but only while this form is not active."
                )
                .Configure();
        }
    }
}
