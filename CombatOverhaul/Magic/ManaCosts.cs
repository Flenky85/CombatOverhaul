using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;

namespace CombatOverhaul.Magic
{
    public static class ManaCosts
    {
        private const int MaxLevel = 9;
        private const int BaseCost = 5;      
        private const int CostPerLevel = 5;  

        public static int FromLevel(int spellLevel)
        {
            if (spellLevel <= 0) return 0;                 
            if (spellLevel > MaxLevel) spellLevel = MaxLevel;
            return BaseCost + spellLevel * CostPerLevel;   
        }

        public static bool TryFromAbility(AbilityData ad, out int cost)
        {
            cost = 0;
            if (ad == null) return false;

            var ab = ad.Blueprint;
            if (ab == null || !ab.IsSpell) return false;
            if (ab.GetComponent<AbilityDeliverTouch>() != null) return false; 
            if (ad.SpellLevel <= 0) return false;

            cost = FromLevel(ad.SpellLevel);
            return cost > 0;
        }
    }
}
