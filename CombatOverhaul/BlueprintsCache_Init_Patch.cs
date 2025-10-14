using CombatOverhaul.Blueprints.Abilities.Commons;
using CombatOverhaul.Blueprints.Features.Commons;
using CombatOverhaul.Blueprints.Features.Races;
using CombatOverhaul.Features;
using CombatOverhaul.Magic.UI;
using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;

namespace CombatOverhaul
{
    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    [HarmonyPriority(Priority.Last)]
    internal static class Patch_BlueprintsCache_Init
    {
        private static bool _initialized; 

        static void Postfix()
        {
            if (_initialized) return;
            _initialized = true;

            MonsterArmorMarkers.Register();
            ManaResource.Register();
            ManaUI.SetManaResource(ManaResource.Mana);

            
            /////////////
            //Abilities//
            /////////////
            
            //Commons
            CrushingBlowAbilityTweaks.Register();
            SunderArmorAbilityTweaks.Register();
            

            ////////////
            //Features//
            ////////////
            
            //Commons
            CrushingBlowFeatureTweaks.Register();
            DoubleSliceFeatureTweaks.Register();
            DragonStyleFeatureTweaks.Register();
            GreaterTwoWeaponFightingFeatureTweaks.Register();
            ImprovedTwoWeaponFightingFeatureTweaks.Register();
            ShiftersEdgeFeatureTweaks.Register();
            SunderArmorFeatureTweaks.Register();
            WeaponFinesseFeatureTweaks.Register();

            //Race
            SlowAndSteadyDwarfFeatureTweaks.Register();
            SlowAndSteadyOreadFeatureTweaks.Register();
            SlowGnomeFeatureTweaks.Register();
            SlowHalflingFeatureTweaks.Register();
        }
    }
}
