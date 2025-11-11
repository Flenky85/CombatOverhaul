using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Shaman
{
    [AutoRegister]
    internal static class ShamanBattleSpiritGreaterAbilityAllTweaks
    {
        public static void Register()
        {
            var abilities = new[]
            {
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityAberrations,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityAnimals,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityConstruct,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityDragons,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityFey,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityHumanoid,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityHumanoidGiant,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityHumanoidReptilian,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityMagicalBeast,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityMonstrousHumanoid,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityOutsiderChaotic,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityOutsiderEvil,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityOutsiderGood,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityOutsiderLawful,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityOutsiderNeutral,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityPlant,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityUndead,
                AbilitiesGuids.ShamanBattleSpiritGreaterAbilityVermin
            };

            foreach (var abilityId in abilities)
            {
                AbilityConfigurator.For(abilityId)
                    .EditComponent<AbilityEffectRunAction>(c =>
                    {
                        var actions = c.Actions.Actions;

                        var applyBuff = (ContextActionApplyBuff)actions[0];
                        applyBuff.DurationValue.Rate = DurationRate.Rounds;
                        applyBuff.DurationValue.BonusValue.ValueType = ContextValueType.Simple;
                        applyBuff.DurationValue.BonusValue.Value = 3;

                        var controlBuff = new ContextActionApplyBuff
                        {
                            m_Buff = BlueprintTool.GetRef<BlueprintBuffReference>(
                                BuffsGuids.ShamanBattleSpiritGreaterBuffControl
                            ),

                            Permanent = applyBuff.Permanent,
                            UseDurationSeconds = applyBuff.UseDurationSeconds,
                            DurationValue = applyBuff.DurationValue,
                            DurationSeconds = applyBuff.DurationSeconds,
                            IsFromSpell = applyBuff.IsFromSpell,
                            IsNotDispelable = applyBuff.IsNotDispelable,
                            ToCaster = applyBuff.ToCaster,
                            AsChild = applyBuff.AsChild,
                            SameDuration = applyBuff.SameDuration,
                            NotLinkToAreaEffect = applyBuff.NotLinkToAreaEffect,
                            IgnoreParentContext = applyBuff.IgnoreParentContext
                        };

                        var newActions = new GameAction[actions.Length + 1];
                        actions.CopyTo(newActions, 0);
                        newActions[actions.Length] = controlBuff;

                        c.Actions.Actions = newActions;
                    })
                    .EditComponent<AbilityResourceLogic>(c =>
                    {
                        c.Amount = 3;
                    })
                    .Configure();
            }
        }
    }
}
