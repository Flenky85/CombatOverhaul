using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace CombatOverhaul.Blueprints.Abilities.Spells.Level9
{
    [AutoRegister]
    internal static class ElementalSwarmAbilityTweak
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.ElementalSwarmAir)
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(false)
                .SetDuration6RoundsShared()
                .SetDescriptionValue(
                    "This spell opens a portal to an Elemental Plane and summons elementals from it. When the spell is complete, " +
                    "2d4 large elementals appear. One rounds later, 1d4 huge elementals appear. One rounds after that, " +
                    "one greater elemental appears. The summoned elementals appear where you designate and act according to " +
                    "their initiative check results. They attack your opponents to the best of their ability. After 6 rounds all the elementals disappear."
                )
                .Configure();
        }
    }
}
