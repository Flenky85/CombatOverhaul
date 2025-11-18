using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class SummonShadowsAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SummonShadowsAbility)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var spawn = (ContextActionSpawnMonster)c.Actions.Actions[0];
                    spawn.DurationValue = new ContextDurationValue
                    {
                        Rate = DurationRate.Rounds,
                        DiceType = DiceType.Zero,
                        DiceCountValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 0
                        },
                        BonusValue = new ContextValue
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 12   
                        },
                        m_IsExtendable = false  
                    };
                })
                .SetDuration12RoundsShared()
                .SetDescriptionValue(
                    "At 7th level, once per combat, a shadowcaster can summon to his side a shadow directly from the Plane of Shadow for 12 " +
                    "rounds as a standard action. The shadow attacks the shadowcaster's opponents to the best of its ability. " +
                    "As the shadowcaster gains levels, the shadow he summons grows more powerful. At 7th level, the shadowcaster's " +
                    "shadow gains a +2 profane bonus to Dexterity, Charisma, AC, attack rolls, Reflex saves, and Will saves. This bonus " +
                    "is further increased by the power of the Shadow Plane ability."
                )
                .Configure();
        }
    }
}
