using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Magic
{
    public enum ManaCostPerLevel
    {
        Level1 = 10,
        Level2 = 20,
        Level3 = 30,
        Level4 = 40,
        Level5 = 50,
        Level6 = 60,
        Level7 = 70,
        Level8 = 80,
        Level9 = 90
    }

    public static class ManaCosts
    {
        public const int Step = 10;
        public const int MaxLevel = 9;


        public static int FromLevel(int spellLevel)
        {
            if (spellLevel <= 0) return 0;               
            if (spellLevel > MaxLevel) spellLevel = MaxLevel;
            return spellLevel * Step;
        }

        
        public static bool TryFromAbility(AbilityData ad, out int cost)
        {
            cost = 0;
            if (ad == null) return false;

            var ab = ad.Blueprint;
            if (ab == null || !ab.IsSpell) return false;

            if (ad.SpellLevel <= 0) return false;

            if (ab.GetComponent<AbilityEffectStickyTouch>() != null) return false;
            if (ab.GetComponent<AbilityDeliverTouch>() != null) return false;

            cost = FromLevel(ad.SpellLevel);
            return cost > 0;
        }
    }
}
