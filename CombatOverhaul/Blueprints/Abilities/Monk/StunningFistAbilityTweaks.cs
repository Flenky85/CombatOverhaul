using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Blueprints.Abilities.Monk
{
    [AutoRegister]
    internal static class StunningFistAbilityTweaks
    {
        public static void Register()
        {
            AbilityConfigurator.For(AbilitiesGuids.StunningFistAbility)
                .EditComponent<AbilityResourceLogic>(c => { c.Amount = 3; })
                .SetDescriptionValue(
                    "You know just where to strike to temporarily stun a foe.\n" +
                    "You must use this feat before you make your attack(thus, a failed attack roll ruins the attempt).Stunning " +
                    "Fist forces a foe damaged by your unarmed attack to make a Fortitude saving throw (DC 10 + half your character " +
                    "level + your Wis modifier), in addition to dealing damage normally. A defender who fails this saving throw is" +
                    " stunned for 1 round(until just before your next turn). A stunned character drops everything held, can't take actions, " +
                    "loses any Dexterity bonus to AC, and takes a –2 penalty to AC. Constructs, oozes, plants, undead, incorporeal creatures, " +
                    "and creatures immune to critical hits cannot be stunned.\n" +
                    "This ability consumes 3 charges; the ability begins with 4 charges, gains 1 additional charge " +
                    "every five levels thereafter. Recover 1 charge each round."
                )
                .Configure();
        }
    }
}
