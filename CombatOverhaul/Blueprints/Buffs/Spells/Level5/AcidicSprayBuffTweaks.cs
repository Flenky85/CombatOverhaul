using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level5
{
    [AutoRegister]
    internal static class AcidicSprayBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.AcidicSprayBuff)
                .EditComponent<ContextRankConfig>(r =>
                {
                    r.m_UseMax = true;
                    r.m_Max = 6; 
                })
                .Configure();
        }
    }
}
