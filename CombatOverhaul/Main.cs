using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace CombatOverhaul
{   
    //test
    public static class Main
    {
        public static bool Enabled;

        public static UnityModManager.ModEntry ModEntry;
        public static Harmony Harmony;
        public static bool FatalError;
        public static string FatalMessage;

        public static void Load(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = OnGUIWrapper;

            try
            {
                Harmony = new Harmony(modEntry.Info.Id);
                Harmony.PatchAll();
                Bootstrap.Init();
                modEntry.Logger.Log("CombatOverhaul loaded with Harmony");
            }
            catch (System.Exception ex)
            {
                FailMod("Failed to initialize Harmony patches.\n" + ex);
            }
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value && !FatalError;
            if (Enabled) Bootstrap.Init();
            else Bootstrap.Dispose();
            return true;
        }

        private static void OnGUIWrapper(UnityModManager.ModEntry entry)
        {
            if (!FatalError) return;

            GUILayout.BeginVertical("box");
            var boldWrap = new GUIStyle(GUI.skin.label) { wordWrap = true, fontStyle = FontStyle.Bold };
            var wrap = new GUIStyle(GUI.skin.label) { wordWrap = true };

            GUILayout.Label(
                "CombatOverhaul has been disabled due to a GUID collision or initialization error.",
                boldWrap, GUILayout.Width(600f)
            );

            if (!string.IsNullOrEmpty(FatalMessage))
            {
                GUILayout.Space(6);
                GUILayout.Label(FatalMessage, wrap, GUILayout.Width(600f));
            }

            GUILayout.EndVertical();
        }

        public static void FailMod(string message)
        {
            FatalError = true;
            FatalMessage = message ?? "Unknown error.";
            Enabled = false;

            if (ModEntry != null)
            {
                try { ModEntry.Logger?.Error(FatalMessage); } catch { }

                try { Harmony?.UnpatchAll(ModEntry.Info.Id); } catch { }

                try
                {
                    if (ModEntry.OnToggle != null)
                        ModEntry.OnToggle(ModEntry, false);

                    ModEntry.Enabled = false;
                }
                catch { /* ignore */ }
            }
        }
    }
}
