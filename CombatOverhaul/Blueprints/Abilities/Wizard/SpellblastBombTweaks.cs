using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using CombatOverhaul.Guids;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace CombatOverhaul.Blueprints.Abilities.Wizard
{
    [AutoRegister]
    internal static class SpellblastBombTweaks
    {
        public static void Register()
        {
            var buffs = new[]
            {
                AbilitiesGuids.SpellblastBomb1,
                AbilitiesGuids.SpellblastBomb2,
                AbilitiesGuids.SpellblastBomb3,
                AbilitiesGuids.SpellblastBomb4,
                AbilitiesGuids.SpellblastBomb5,
                AbilitiesGuids.SpellblastBomb6,
                AbilitiesGuids.SpellblastBomb7,
                AbilitiesGuids.SpellblastBomb8,
                AbilitiesGuids.SpellblastBomb9,
            };

            foreach (var id in buffs)
            {
                AbilityConfigurator.For(id)
                    .SetType(AbilityType.Spell)
                    .Configure();
            }
        }
    }
}
