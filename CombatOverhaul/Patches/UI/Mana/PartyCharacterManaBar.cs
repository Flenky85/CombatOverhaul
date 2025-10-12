using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.MVVM._PCView.Party;
using Owlcat.Runtime.UI.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;   // Image, LayoutElement, LayoutGroups

namespace CombatOverhaul.Patches.UI.Mana
{
    // ---------- Infra mini: Provider + Events + View ----------
    internal static class ManaProvider
    {
        // TODO: al crear el BlueprintAbilityResource de maná, asígnalo aquí.
        public static Kingmaker.Blueprints.BlueprintAbilityResource ManaResource;

        public static (int current, int max) Get(UnitEntityData unit)
        {
            if (unit == null || ManaResource == null) return (0, 0);

            var desc = unit.Descriptor;
            if (desc == null) return (0, 0);

            // Máximo desde el blueprint del recurso
            int max = ManaResource.GetMaxAmount(desc);

            // Actual desde la colección de recursos del descriptor
            int cur = desc.Resources.GetResourceAmount(ManaResource);

            // Seguridad
            if (cur > max) cur = max;
            if (cur < 0) cur = 0;

            return (cur, max);
        }
    }

    internal static class ManaEvents
    {
        private static readonly Dictionary<UnitEntityData, List<Action<int, int>>> _subs =
            new Dictionary<UnitEntityData, List<Action<int, int>>>();

        public static void Subscribe(UnitEntityData unit, Action<int, int> cb)
        {
            if (unit == null || cb == null) return;
            List<Action<int, int>> list;
            if (!_subs.TryGetValue(unit, out list))
            {
                list = new List<Action<int, int>>();
                _subs[unit] = list;
            }
            if (!list.Contains(cb)) list.Add(cb);
        }

        public static void Unsubscribe(UnitEntityData unit, object owner)
        {
            if (unit == null) return;
            List<Action<int, int>> list;
            if (_subs.TryGetValue(unit, out list))
            {
                list.RemoveAll(a => a.Target == owner);
                if (list.Count == 0) _subs.Remove(unit);
            }
        }

        public static void Raise(UnitEntityData unit, int current, int max)
        {
            if (unit == null) return;
            List<Action<int, int>> list;
            if (_subs.TryGetValue(unit, out list))
            {
                var array = list.ToArray();
                for (int i = 0; i < array.Length; i++)
                    if (array[i] != null) array[i](current, max);
            }
        }
    }

    internal sealed class ManaBarView : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private TextMeshProUGUI _label;

        public void Attach(Image fillImage, TextMeshProUGUI label)
        {
            _fill = fillImage;
            _label = label;
        }

        public void SetBlue()
        {
            if (_fill != null) _fill.color = new Color(0.2f, 0.5f, 1f, 1f);
        }

        public void UpdateValue(int current, int max)
        {
            if (_fill != null) _fill.fillAmount = (max > 0) ? Mathf.Clamp01((float)current / (float)max) : 0f;
            if (_label != null) _label.text = string.Format("{0}/{1}", current, max);
        }
    }

    internal sealed class OnDestroyHook : MonoBehaviour
    {
        public Action OnDestroyed;
        private void OnDestroy() { var a = OnDestroyed; if (a != null) a(); }
    }

    // ---------- Patch: añadimos la barra de maná en BindViewImplementation() ----------
    [HarmonyPatch]
    internal static class PartyCharacterManaBarPatch
    {
        // Parchar TODAS las clases concretas que heredan de PartyCharacterView<> y tengan BindViewImplementation
        static IEnumerable<MethodBase> TargetMethods()
        {
            var partyViewOpenGeneric = typeof(PartyCharacterView<>);
            var asm = partyViewOpenGeneric.Assembly;
            foreach (var t in asm.GetTypes())
            {
                if (!t.IsClass || t.IsAbstract) continue;
                if (!IsSubclassOfRawGeneric(partyViewOpenGeneric, t)) continue;
                var m = t.GetMethod("BindViewImplementation",
                                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (m != null && m.GetParameters().Length == 0)
                    yield return m;
            }
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        // Postfix común
        static void Postfix(object __instance)
        {
            try
            {
                Log.Info("[ManaBarPatch] Postfix enter: " + __instance.GetType().FullName);

                var comp = __instance as Component;
                if (comp == null) { Log.Warning("[ManaBarPatch] comp == null"); return; }

                // ViewModel
                var vmProp = __instance.GetType().GetProperty("ViewModel",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var vm = (vmProp != null) ? vmProp.GetValue(__instance, null) : null;
                if (vm == null) { Log.Warning("[ManaBarPatch] vm == null"); return; }

                UnitEntityData unit = TryGetUnitFromVM(vm, __instance);
                if (unit == null) { Log.Warning("[ManaBarPatch] unit == null"); return; }

                Log.Info("[ManaBarPatch] unit OK: " + (unit.CharacterName ?? unit.ToString()));

                // Intentar localizar el bloque de salud por tipo de vista (robusto)
                Transform healthContainer = null;

                // 1) Progress bar
                var hpProg = comp.gameObject.GetComponentsInChildren<UnitHealthPartProgressView>(true).FirstOrDefault();
                if (hpProg != null)
                {
                    healthContainer = hpProg.transform.parent;
                    Log.Info("[ManaBarPatch] healthContainer from UnitHealthPartProgressView: " + healthContainer.name);
                }

                // 2) Fallback: texto de salud
                if (healthContainer == null)
                {
                    var hpTxt = comp.gameObject.GetComponentsInChildren<UnitHealthPartTextView>(true).FirstOrDefault();
                    if (hpTxt != null)
                    {
                        healthContainer = hpTxt.transform.parent;
                        Log.Info("[ManaBarPatch] healthContainer from UnitHealthPartTextView: " + healthContainer.name);
                    }
                }

                // 3) Último recurso: contenedor del retrato
                if (healthContainer == null)
                {
                    var portrait = comp.gameObject.GetComponentsInChildren<UnitPortraitPartView>(true).FirstOrDefault();
                    if (portrait != null)
                    {
                        healthContainer = portrait.transform.parent;
                        Log.Info("[ManaBarPatch] healthContainer from UnitPortraitPartView: " + healthContainer.name);
                    }
                }

                if (healthContainer == null)
                {
                    Log.Warning("[ManaBarPatch] healthContainer == null (types not found). Anchor to root as last resort.");
                    healthContainer = comp.transform; // último recurso visual
                }

                // Evitar duplicados si ya existe
                var existing = healthContainer.Find("ManaBar");
                if (existing != null)
                {
                    Log.Info("[ManaBarPatch] ManaBar already exists → refresh");
                    Refresh(existing.gameObject, unit);
                    return;
                }

                // ---------- Crear ManaBar (anclada al RT de la barra de HP) ----------
                Log.Info("[ManaBarPatch] Creating ManaBar");

                // Localizar el RectTransform de la barra de vida para clonar geometría
                RectTransform hpRT = null;
                if (hpProg != null)
                    hpRT = hpProg.GetComponent<RectTransform>();
                if (hpRT == null)
                    hpRT = healthContainer as RectTransform;

                // Tweaks de colocación/tamaño
                const float WIDTH_DELTA = 2.3f;  // píxeles extra de anchura total
                const float BAR_HEIGHT = 20f;   // Altura
                const float X_OFFSET_R = 3f;   // un poco a la derecha
                const float Y_OFFSET_UP = 0f;  // menos negativo = más arriba (respecto a la HP)

                // 1) CONTENEDOR para evitar que el Layout del padre nos aplaste
                var holder = new GameObject("ManaBar_Holder", typeof(RectTransform));
                var holderRT = holder.GetComponent<RectTransform>();
                holder.transform.SetParent(healthContainer, false);

                bool parentHasLayout =
                    (healthContainer.GetComponent<HorizontalLayoutGroup>() != null) ||
                    (healthContainer.GetComponent<VerticalLayoutGroup>() != null) ||
                    (healthContainer.GetComponent<GridLayoutGroup>() != null);

                var holderLE = holder.AddComponent<LayoutElement>();
                holderLE.ignoreLayout = !parentHasLayout;
                holderLE.minHeight = BAR_HEIGHT;
                holderLE.preferredHeight = BAR_HEIGHT;
                holderLE.flexibleWidth = 1f;

                // Colocar justo después de la HP y con ligeros offsets
                if (hpRT != null)
                {
                    holderRT.SetSiblingIndex(hpRT.GetSiblingIndex() + 1);
                    holderRT.anchorMin = hpRT.anchorMin;
                    holderRT.anchorMax = hpRT.anchorMax;
                    holderRT.pivot = hpRT.pivot;
                    holderRT.sizeDelta = hpRT.sizeDelta;
                    holderRT.anchoredPosition = hpRT.anchoredPosition + new Vector2(X_OFFSET_R, Y_OFFSET_UP);
                }
                else
                {
                    holderRT.anchorMin = new Vector2(0f, 0f);
                    holderRT.anchorMax = new Vector2(1f, 0f);
                    holderRT.pivot = new Vector2(0.5f, 0f);
                    holderRT.sizeDelta = new Vector2(0f, BAR_HEIGHT);
                    holderRT.anchoredPosition = new Vector2(X_OFFSET_R, Y_OFFSET_UP);
                }

                if (parentHasLayout)
                {
                    // Si el padre usa LayoutGroup, el ancho lo manda el LayoutElement
                    // Tomamos como base el ancho actual (si lo conoces puedes usar hpRT.rect.width)
                    holderLE.flexibleWidth = 0f; // que no estire libremente
                                                 // Si hpRT existe, usamos su ancho real; si no, sumamos al preferred
                    float baseWidth = (hpRT != null) ? hpRT.rect.width : holderLE.preferredWidth;
                    holderLE.minWidth = baseWidth + WIDTH_DELTA;
                    holderLE.preferredWidth = baseWidth + WIDTH_DELTA;
                }
                else
                {
                    // Si NO hay LayoutGroup y los anchors están en stretch (0..1),
                    // sizeDelta.x no manda; hay que abrir offsets.
                    // Abrimos por ambos lados para ganar WIDTH_DELTA en total.
                    var offMin = holderRT.offsetMin;
                    var offMax = holderRT.offsetMax;
                    offMin.x -= WIDTH_DELTA * 0.5f;
                    offMax.x += WIDTH_DELTA * 0.5f;
                    holderRT.offsetMin = offMin;
                    holderRT.offsetMax = offMax;

                    // Alternativa (si prefieres ancho absoluto en vez de stretch):
                    // holderRT.anchorMin = new Vector2(0.5f, holderRT.anchorMin.y);
                    // holderRT.anchorMax = new Vector2(0.5f, holderRT.anchorMax.y);
                    // holderRT.sizeDelta = new Vector2((hpRT != null ? hpRT.rect.width : holderRT.sizeDelta.x) + WIDTH_DELTA, BAR_HEIGHT);
                }

                // 2) Barra real dentro del holder
                var manaBar = new GameObject("ManaBar", typeof(RectTransform));
                var manaRT = manaBar.GetComponent<RectTransform>();
                manaBar.transform.SetParent(holderRT, false);
                manaRT.anchorMin = Vector2.zero;
                manaRT.anchorMax = Vector2.one;
                manaRT.pivot = new Vector2(0.5f, 0.5f);
                manaRT.sizeDelta = Vector2.zero;
                manaRT.anchoredPosition = Vector2.zero;

                // Pequeño “padding” para que no toque bordes
                const float PAD_XL = 4f, PAD_XR = 2f, PAD_YT = 1f, PAD_YB = 1f;
                manaRT.offsetMin = new Vector2(PAD_XL, PAD_YB);
                manaRT.offsetMax = new Vector2(-PAD_XR, -PAD_YT);

                // 3) BG visible (ajusta luego colores si quieres)
                var bgGO = new GameObject("BG", typeof(RectTransform), typeof(Image));
                var bgRT = bgGO.GetComponent<RectTransform>();
                bgGO.transform.SetParent(manaBar.transform, false);
                bgRT.anchorMin = Vector2.zero; bgRT.anchorMax = Vector2.one;
                bgRT.offsetMin = Vector2.zero; bgRT.offsetMax = Vector2.zero;
                var bgImg = bgGO.GetComponent<Image>();
                bgImg.color = new Color(0f, 0f, 0f, 0.65f); // negro suave

                // 4) FILL (azul) con sprite asegurado
                var fillGO = new GameObject("Fill", typeof(RectTransform), typeof(Image));
                var fillRT = fillGO.GetComponent<RectTransform>();
                fillGO.transform.SetParent(manaBar.transform, false);
                fillRT.anchorMin = new Vector2(0f, 0f);
                fillRT.anchorMax = new Vector2(1f, 1f);
                fillRT.offsetMin = Vector2.zero; fillRT.offsetMax = Vector2.zero;
                var fillImg = fillGO.GetComponent<Image>();
                fillImg.type = Image.Type.Filled;
                fillImg.fillMethod = Image.FillMethod.Horizontal;
                fillImg.fillOrigin = (int)Image.OriginHorizontal.Left;
                fillImg.color = new Color(0.1f, 0.4f, 1f, 1f); // azul HUD

                // Reusar sprite/material de la HP si existe
                Sprite hpSprite = null; Material hpMat = null;
                try
                {
                    if (hpProg != null)
                    {
                        var hpImg = hpProg.GetComponentsInChildren<Image>(true)
                                          .FirstOrDefault(i => i.type == Image.Type.Filled && i.sprite != null);
                        if (hpImg != null) { hpSprite = hpImg.sprite; hpMat = hpImg.material; }
                    }
                }
                catch { /* ignore */ }

                if (hpSprite != null)
                {
                    fillImg.sprite = hpSprite;
                    fillImg.material = hpMat;
                    bgImg.sprite = hpSprite;
                    bgImg.type = Image.Type.Sliced;
                }
                else
                {
                    var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    tex.SetPixel(0, 0, Color.white);
                    tex.Apply();
                    var spr = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 100f);
                    fillImg.sprite = spr;
                    bgImg.sprite = spr;
                    bgImg.type = Image.Type.Sliced;
                }

                bgImg.raycastTarget = false;
                fillImg.raycastTarget = false;

                // (label opcional) lo seguimos omitiendo de momento
                TextMeshProUGUI label = null;


            }
            catch (Exception ex)
            {
                Log.Error("[ManaBarPatch] EX", ex);
            }
        }

        private static void Refresh(GameObject manaBar, UnitEntityData unit)
        {
            var view = manaBar.GetComponent<ManaBarView>();
            if (view == null) return;
            var cm = ManaProvider.Get(unit);
            view.UpdateValue(cm.current, cm.max);
            ManaEvents.Subscribe(unit, delegate (int c, int m) { view.UpdateValue(c, m); });
        }

        private static UnitEntityData TryGetUnitFromVM(object partyCharacterVM, object viewInstance)
        {
            // A) Escaneo genérico del VM principal
            var u = GetUnitFromAny(partyCharacterVM);
            if (u != null) { Log.Info("[ManaBarPatch] Unit from VM (generic) OK"); return u; }

            // B) Si tenemos la view concreta, usamos sus sub-views ya bindeados
            if (viewInstance != null)
            {
                // m_PortraitView → .ViewModel → escaneo
                var portraitField = GetFieldRecursive(viewInstance.GetType(), "m_PortraitView");
                var portraitView = (portraitField != null) ? portraitField.GetValue(viewInstance) as Component : null;
                var portraitVMProp = (portraitView != null) ? portraitView.GetType().GetProperty("ViewModel",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : null;
                var portraitVM = (portraitVMProp != null) ? portraitVMProp.GetValue(portraitView, null) : null;
                var u2 = GetUnitFromAny(portraitVM);
                if (u2 != null) { Log.Info("[ManaBarPatch] Unit from m_PortraitView.ViewModel OK"); return u2; }

                // m_HealthProgressView → .ViewModel → escaneo
                var hpField = GetFieldRecursive(viewInstance.GetType(), "m_HealthProgressView");
                var hpView = (hpField != null) ? hpField.GetValue(viewInstance) as Component : null;
                var hpVMProp = (hpView != null) ? hpView.GetType().GetProperty("ViewModel",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : null;
                var hpVM = (hpVMProp != null) ? hpVMProp.GetValue(hpView, null) : null;
                var u3 = GetUnitFromAny(hpVM);
                if (u3 != null) { Log.Info("[ManaBarPatch] Unit from m_HealthProgressView.ViewModel OK"); return u3; }

                // m_HealthTextView → .ViewModel → escaneo
                var htField = GetFieldRecursive(viewInstance.GetType(), "m_HealthTextView");
                var htView = (htField != null) ? htField.GetValue(viewInstance) as Component : null;
                var htVMProp = (htView != null) ? htView.GetType().GetProperty("ViewModel",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : null;
                var htVM = (htVMProp != null) ? htVMProp.GetValue(htView, null) : null;
                var u4 = GetUnitFromAny(htVM);
                if (u4 != null) { Log.Info("[ManaBarPatch] Unit from m_HealthTextView.ViewModel OK"); return u4; }
            }

            Log.Warning("[ManaBarPatch] No Unit in VM (all strategies failed)");
            return null;
        }

        private static UnitEntityData GetUnitFromAny(object obj, int depth = 0)
        {
            if (obj == null || depth > 2) return null;

            var t = obj.GetType();

            // 1) Propiedades que devuelvan directamente UnitEntityData
            var props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            for (int i = 0; i < props.Length; i++)
            {
                var p = props[i];
                try
                {
                    var pt = p.PropertyType;
                    if (pt == typeof(UnitEntityData))
                        return p.GetValue(obj, null) as UnitEntityData;

                    if (p.CanRead && (p.Name.IndexOf("Unit", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                      p.Name.IndexOf("EntityData", StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        var v = p.GetValue(obj, null);
                        var u = GetUnitFromAny(v, depth + 1);
                        if (u != null) return u;
                    }
                }
                catch { }
            }

            // 2) Campos que devuelvan directamente UnitEntityData
            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            for (int i = 0; i < fields.Length; i++)
            {
                var f = fields[i];
                try
                {
                    var ft = f.FieldType;
                    if (ft == typeof(UnitEntityData))
                        return f.GetValue(obj) as UnitEntityData;

                    if (f.Name.IndexOf("Unit", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        f.Name.IndexOf("EntityData", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        var v = f.GetValue(obj);
                        var u = GetUnitFromAny(v, depth + 1);
                        if (u != null) return u;
                    }
                }
                catch { }
            }

            return null;
        }

        private static FieldInfo GetFieldRecursive(Type t, string name)
        {
            while (t != null)
            {
                var f = t.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (f != null) return f;
                t = t.BaseType;
            }
            return null;
        }
    }
}
