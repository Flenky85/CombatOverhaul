using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using System.Collections.Generic;

namespace CombatOverhaul.Patches.UI.Mana
{
    /// <summary>
    /// Utilidad de prueba para forzar valores de maná en UI sin tener aún el AbilityResource.
    /// </summary>
    /// Testeo: CombatOverhaul.Patches.UI.Mana.ManaUITest.Apply25of100ToParty();
    public static class ManaUITest
    {
        /// <summary>
        /// Fuerza 25/100 de maná en todas las unidades de la party y refresca las barras.
        /// Llama a este método cuando la UI de party esté pintada (p.ej. tras cargar partida/entrar en mapa).
        /// </summary>
        public static void Apply25of100ToParty()
        {
            var party = Game.Instance?.Player?.Party;
            if (party == null) return;

            // Recorremos la party y notificamos a la UI
            foreach (UnitEntityData unit in party)
            {
                ManaEvents.Raise(unit, 50, 100);
            }

            Utils.Log.Info("[ManaUITest] Maná de prueba aplicado: 25/100 a toda la party.");
        }
    }
}
