using BlueprintCore.Blueprints.CustomConfigurators;      
using Kingmaker.Blueprints;

namespace CombatOverhaul.Resources
{
    internal static class ManaResourceBP
    {
        public static BlueprintAbilityResource Mana; 

        public const string ManaGuid = "2f6b2b9a-1b6b-4a9c-8c0c-7f4b1a2e9c10";

        private static bool _registered;
        private const string ManaName = "CO_ManaResource";

        public static void Register()
        {
            if (_registered) return;
            _registered = true;

            var amount = ResourceAmountBuilder.New(baseValue: 0);
            Mana = AbilityResourceConfigurator
                .New(ManaName, ManaGuid)
                .SetMaxAmount(amount)
                .Configure();
        }
    }
}
