using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System.Linq;


namespace CombatOverhaul.Blueprints.Abilities.Spells.Level3
{
    [AutoRegister]
    internal static class BatteringBlastAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.BatteringBlast)
                .EditComponent<AbilityEffectRunAction>(c =>
                {
                    var deal = c.Actions.Actions.OfType<ContextActionDealDamage>().First();
                    deal.Value.DiceType = DiceType.D6;
                    deal.Value.DiceCountValue = ContextValues.Constant(2);
                })
                .EditComponent<AbilityDeliverProjectile>(c =>
                {
                    c.UseMaxProjectilesCount = true;
                    c.MaxProjectilesCountRank = AbilityRankType.ProjectilesCount;
                })
                .RemoveComponents(comp =>
                {
                    return comp is ContextRankConfig r && (
                        r.m_Type == AbilityRankType.Default
                    || r.m_Type == AbilityRankType.ProjectilesCount     
                    );
                })
                .AddContextRankConfig(new ContextRankConfig
                {
                    m_Type = AbilityRankType.ProjectilesCount,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.DivStep,
                    m_StartLevel = 2,   
                    m_StepLevel = 2,    
                    m_UseMax = true,
                    m_Max = 4
                })
                .SetDescriptionValue(
                    "You hurl a fist-sized ball of force resembling a sphere of spikes to ram a designated creature. " +
                    "You must succeed on a ranged touch attack to strike your target. On a successful hit, each " +
                    "projectile deals 2d6 points of force damage. You fire one projectile for every two caster levels " +
                    "you have, to a maximum of 4 projectiles (for example, 4 projectiles at caster level 8)\n." +
                    "A creature struck by any of these is subject to a bull rush attempt. The force has a Strength modifier equal to " +
                    "your Intelligence, Wisdom, or Charisma modifier (whichever is highest). The CMB for the force's bull " +
                    "rush uses your caster level as its base attack bonus, adding the force's Strength modifier and a +10 " +
                    "bonus for each additional blast that hits. If the bull rush succeeds, the force pushes the creature away " +
                    "from you in a straight line, and the creature must make a Reflex save or fall prone."
                )
                .Configure();
        }
    }
}
