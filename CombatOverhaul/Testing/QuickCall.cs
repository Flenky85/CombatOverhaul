using System;
using UnityEngine;

namespace CombatOverhaul.Testing
{
    public static class QuickCall
    {
        /// <summary>
        /// Llamar asi en unity explorer: 
        /// CombatOverhaul.Testing.QuickCall.Run();
        /// </summary>
        public static void Run()
        {
            try
            {
                Debug.Log("[CombatOverhaul][QuickCall] Inicio");
                // ⬇️ Cambia esta línea por la función real que quieras invocar:
                QuickCallTest.Test();
                Debug.Log("[CombatOverhaul][QuickCall] Fin OK");
            }
            catch (Exception ex)
            {
                Debug.LogError("[CombatOverhaul][QuickCall] ERROR: " + ex);
            }
        }
        public static class QuickCallTest
        {
            public static void Test()
            {
                Debug.Log("prueba de quickcall");
            }
        }
    }
}
