using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using CombatOverhaul.Guids;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace CombatOverhaul.Blueprints.Buffs.Spells.Level1
{
    [AutoRegister]
    internal static class RayOfEnfeeblementBuffTweaks
    {
        public static void Register()
        {
            BuffConfigurator.For(BuffsGuids.RayOfEnfeeblementBuff)
                .EditComponent<AddFactContextActions>(c =>
                {
                    c.Activated = ActionsBuilder.New()
                        .Add<BuffActionAddStatBonus>(a =>
                        {
                            a.Stat = StatType.Strength;
                            a.Value = ContextValues.Shared(AbilitySharedValue.Damage);
                            a.Descriptor = ModifierDescriptor.NegativeEnergyPenalty;
                        })
                        .Build();
                })
                .Configure();
        }
    }
}
