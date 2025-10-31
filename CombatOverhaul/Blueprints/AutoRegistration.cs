using System;
using System.Linq;
using System.Reflection;
using CombatOverhaul.Utils;

namespace CombatOverhaul.Blueprints
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class AutoRegisterAttribute : Attribute
    {
        public AutoRegisterAttribute() { }
    }

    internal static class BlueprintsAutoRegistrar
    {
        private static bool _ran;

        public static void RunAll()
        {
            if (_ran) return;
            _ran = true;

            try
            {
                var asm = typeof(BlueprintsAutoRegistrar).Assembly;

                var types = asm.GetTypes()
                    .Where(t => t.IsClass)
                    .Where(t => t.Namespace != null && t.Namespace.StartsWith("CombatOverhaul.Blueprints"))
                    .Select(t => new
                    {
                        Type = t,
                        Attr = t.GetCustomAttribute<AutoRegisterAttribute>(),
                        Register = t.GetMethod(
                            "Register",
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
                            binder: null, types: Type.EmptyTypes, modifiers: null)
                    })
                    .Where(x => x.Attr != null && x.Register != null)
                    .OrderBy(x => x.Type.FullName, StringComparer.Ordinal) // orden estable sin phases/order
                    .ToList();

                Log.Info("[AutoReg] Found " + types.Count + " tweak(s) to register.");

                foreach (var x in types)
                {
                    try
                    {
                        x.Register.Invoke(null, null);
#if DEBUG
                        Log.DebugLog("[AutoReg] Ran " + x.Type.FullName + ".Register()");
#endif
                    }
                    catch (Exception ex)
                    {
                        Log.Error("[AutoReg] " + x.Type.FullName + ".Register() threw", ex);
                    }
                }

#if DEBUG
                // Aviso útil para clases "*Tweaks" con Register() pero sin [AutoRegister]
                var missed = asm.GetTypes()
                    .Where(t => t.IsClass
                                && t.Name.EndsWith("Tweaks")
                                && t.Namespace != null
                                && t.Namespace.StartsWith("CombatOverhaul.Blueprints")
                                && t.GetCustomAttribute<AutoRegisterAttribute>() == null
                                && t.GetMethod(
                                    "Register",
                                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
                                    binder: null, types: Type.EmptyTypes, modifiers: null) != null);
                foreach (var t in missed)
                    Log.Warning("[AutoReg] " + t.FullName + " has Register() but no [AutoRegister]; it won't run automatically.");
#endif
            }
            catch (Exception ex)
            {
                Log.Error("[AutoReg] Fatal error while discovering/registering tweaks", ex);
            }
        }
    }
}
