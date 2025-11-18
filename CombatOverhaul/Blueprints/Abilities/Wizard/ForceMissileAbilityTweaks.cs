using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class ForceMissileAbilityTweaks
    {
        public static void Register()
        {
            var wiz = BlueprintTool.GetRef<BlueprintCharacterClassReference>(ClassGuids.WizardClass);

            AbilityConfigurator.For(AbilitiesGuids.ForceMissileAbility)
                .AddComponent(new ContextRankConfig
                {
                    m_Type = AbilityRankType.DamageBonus,
                    m_BaseValueType = ContextRankBaseValueType.ClassLevel,
                    m_Progression = ContextRankProgression.Div2,
                    m_Class = new[] { wiz }
                })
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var dmg = (ContextActionDealDamage)c.Actions.Actions[0];
                    dmg.Value.BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageBonus
                    };
                })
                .EditComponent<AbilityResourceLogic>(c =>
                {
                    c.Amount = 0; 
                })
                .SetDescriptionValue(
                    "As a standard action, you can unleash a force missile that automatically strikes a foe, as magic missile. " +
                    "The force missile deals 1d4 + your Intense Spells bonus +1 per two wizard levels damage. " +
                    "This is a force effect."
                )
                .Configure();
        }
    }
}
