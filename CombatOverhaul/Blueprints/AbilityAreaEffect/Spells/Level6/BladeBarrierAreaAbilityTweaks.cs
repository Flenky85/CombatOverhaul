using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using Kingmaker.UnitLogic.Mechanics.Components;


namespace CombatOverhaul.Blueprints.AbilityAreaEffect.Spells.Level6
{
    [AutoRegister]
    internal static class BladeBarrierAreaAbilityTweaks
    {
        public static void Register()
        {
            AbilityAreaEffectConfigurator.For(AbilityAreaEffectGuids.BladeBarrierArea)
                .EditComponent<ContextRankConfig>(c => { c.m_Max = 14; })
                .Configure();
        }
    }
}
