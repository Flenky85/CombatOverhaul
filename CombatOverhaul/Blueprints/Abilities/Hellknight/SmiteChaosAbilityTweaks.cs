using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Hellknight
{
    [AutoRegister]
    internal static class SmiteChaosAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.SmiteChaosAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDescriptionValue(
                    "A Hellknight can call out to the powers of law to aid him in his struggle against chaos. " +
                    "As a swift action, the Hellknight chooses one target within sight to smite. If this target " +
                    "is chaotic, the Hellknight adds his Charisma bonus (if any) to his attack rolls and adds twice " +
                    "his Hellknight level to all damage rolls made against the target of his smite. Smite chaos " +
                    "attacks automatically bypass any DR the creature might possess. In addition, while smite chaos " +
                    "is in effect, the Hellknight gains a deflection bonus equal to his Charisma modifier (if any) to " +
                    "his AC against attacks made by the target of the smite.\n" +
                    "If the Hellknight targets a creature that is not chaotic, the smite is wasted with no effect.\n" +
                    "The smite chaos lasts until the target dies or the Hellknight selects a new target.\n" +
                    "This ability consumes 3 charges; the Hellknight begins with 3 charges, gains 1 additional charge " +
                    "at 4th level, and gains 1 more charge every three levels thereafter. Recover 1 charge each round."
                )
                .Configure();
        }
    }
}
