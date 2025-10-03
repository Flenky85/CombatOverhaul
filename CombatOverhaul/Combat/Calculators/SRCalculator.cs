/*using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;

namespace CombatOverhaul.Combat.Calculators
{
    internal static class SRCalculator
    {
        /// Devuelve la SR efectiva del objetivo (0 si no tiene).
        public static int GetTargetSR(UnitDescriptor targetDesc, MechanicsContext ctx)
        {
            if (targetDesc == null) return 0;
            var upr = targetDesc.Get<UnitPartSpellResistance>();
            if (upr == null) return 0;

            int sr = 0;
            try { sr = upr.GetValue(ctx); } catch { sr = 0; }
            if (sr <= 0)
            {
                try { sr = upr.GetValue(null); } catch { }
            }
            return sr > 0 ? sr : 0;
        }
    }
}
*/