using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using CombatOverhaul.Guids;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints.Features.Paladin
{
    [AutoRegister]
    internal class DivineGuardianTrothFeatureTweaks
    {
        public static void Register()
        {
            FeatureConfigurator.For(FeaturesGuids.DivineGuardianTroth)
                .SetDescriptionValue(
                    "The divine guardian pledges her protection to a willing " +
                    "creature for the day. The creature gains benefits while remaining adjacent " +
                    "to the divine guardian.\n" +
                    "When a target of the divine guardian's divine troth ability is attacked, if that " +
                    "target is within her melee reach, she may use an attack of opportunity to provide " +
                    "her ally with a +4 bonus to AC.\n" +
                    "The divine guardian can intercept a successful attack against the target of her " +
                    "divine troth ability once per round, taking full damage from that attack and any " +
                    "associated effects.\n" +
                    "Divine guardian uses charges; activating this ability expends 3 charges. The paladin begins with 3 " +
                    "charges. At the start of each round, the paladin regains 1 charge, up to her maximum number of charges."
                )
                .Configure();
        }
    }
}
