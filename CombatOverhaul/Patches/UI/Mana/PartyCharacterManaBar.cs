using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.MVVM._PCView.Party;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace CombatOverhaul.Patches.UI.Mana
{
    // =========================================================
    // ===============  CONFIGURACIÓN DE LA BARRA  =============
    // =========================================================
    internal static class ManaUIConfig
    {
        // ALTURA visual de la barra (px)
        public const float BAR_HEIGHT = 122f;

        // Ancho extra respecto al ancho de la barra de HP (px). 0 = igual que HP.
        public const float WIDTH_DELTA = 3f;

        // Offsets relativos al anclaje de la HP (positivo = derecha / arriba)
        public const float X_OFFSET = 5f;
        public const float Y_OFFSET = 0f;

        // Padding interno dentro del rect del holder (left, right, top, bottom)
        public const float PAD_L = 3f;
        public const float PAD_R = 2f;
        public const float PAD_T = 1f;
        public const float PAD_B = 1f;

        // Colores
        public static readonly Color BG_COLOR = new Color(0f, 0f, 0f, 0.65f);
        public static readonly Color FILL_COLOR = new Color(0.12f, 0.45f, 1f, 1f);
    }

    // =========================================================
    // ================  API PÚBLICA DE MANA UI  ===============
    // =========================================================
    public static class ManaUI
    {
        // Llama a esto cuando tengas creado el BlueprintAbilityResource de maná.
        public static void SetManaResource(Kingmaker.Blueprints.BlueprintAbilityResource res)
        {
            ManaProvider.ManaResource = res;
        }

        // Útil si cambias valores de maná desde otro sitio y quieres forzar repaint
        public static void RefreshUnit(UnitEntityData unit)
        {
            if (unit == null) return;
            var (current, max) = ManaProvider.Get(unit);
            ManaEvents.Raise(unit, current, max);
        }
    }

    // =========================================================
    // ============  INFRA: Provider + Events + View ===========
    // =========================================================
    internal static class ManaProvider
    {
        public static Kingmaker.Blueprints.BlueprintAbilityResource ManaResource;

        public static (int current, int max) Get(UnitEntityData unit)
        {
            if (unit == null || ManaResource == null) return (0, 0);
            var desc = unit.Descriptor;
            if (desc == null) return (0, 0);

            int max = ManaResource.GetMaxAmount(desc);
            int cur = desc.Resources.GetResourceAmount(ManaResource);

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
            if (!_subs.TryGetValue(unit, out List<Action<int, int>> list))
            {
                list = new List<Action<int, int>>();
                _subs[unit] = list;
            }
            if (!list.Contains(cb)) list.Add(cb);
        }

        public static void Unsubscribe(UnitEntityData unit, object owner)
        {
            if (unit == null) return;
            if (_subs.TryGetValue(unit, out List<Action<int, int>> list))
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var a = list[i];
                    if (a == null || a.Target == owner) list.RemoveAt(i);
                }
                if (list.Count == 0) _subs.Remove(unit);
            }
        }

        public static void Raise(UnitEntityData unit, int current, int max)
        {
            if (unit == null) return;
            if (_subs.TryGetValue(unit, out List<Action<int, int>> list))
            {
                var array = list.ToArray();
                for (int i = 0; i < array.Length; i++)
                {
                    array[i]?.Invoke(current, max);
                }
            }
        }
    }

    internal sealed class ManaBarView : MonoBehaviour
    {
        [SerializeField] private Image _fill;

        public void Attach(Image fillImage)
        {
            _fill = fillImage;
        }

        public void SetColor(Color c)
        {
            if (_fill != null) _fill.color = c;
        }

        public void UpdateValue(int current, int max)
        {
            if (_fill != null) _fill.fillAmount = (max > 0) ? Mathf.Clamp01((float)current / (float)max) : 0f;
        }
    }

    internal sealed class OnDestroyHook : MonoBehaviour
    {
        public Action OnDestroyed;
        private void OnDestroy() { OnDestroyed?.Invoke(); }
    }

    // =========================================================
    // =====================  EL PARCHE  =======================
    // =========================================================
    [HarmonyPatch]
    internal static class PartyCharacterManaBarPatch
    {
        // cache ligero para evitar LINQ repetido
        private static readonly Dictionary<Transform, Sprite> _hpSpriteCache = new Dictionary<Transform, Sprite>();
        private static readonly Dictionary<Transform, Material> _hpMaterialCache = new Dictionary<Transform, Material>();

        static IEnumerable<MethodBase> TargetMethods()
        {
            var open = typeof(PartyCharacterView<>);
            var asm = open.Assembly;
            foreach (var t in asm.GetTypes())
            {
                if (!t.IsClass || t.IsAbstract) continue;
                if (!IsSubclassOfRawGeneric(open, t)) continue;
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

        static void Postfix(object __instance)
        {
            try
            {
                var comp = __instance as Component;
                if (comp == null) return;

                var vmProp = __instance.GetType().GetProperty("ViewModel",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var vm = vmProp?.GetValue(__instance, null);
                if (vm == null) return;

                var unit = TryGetUnitFromVM(vm, __instance);
                if (unit == null) return;

                // anclaje: usamos el padre del HealthProgress si existe, si no salud texto, si no retrato, etc.
                Transform healthContainer = null;
                UnitHealthPartProgressView hpProg = null;

                hpProg = comp.gameObject.GetComponentInChildren<UnitHealthPartProgressView>(true);
                if (hpProg != null) healthContainer = hpProg.transform.parent;

                if (healthContainer == null)
                {
                    var hpTxt = comp.gameObject.GetComponentInChildren<UnitHealthPartTextView>(true);
                    if (hpTxt != null) healthContainer = hpTxt.transform.parent;
                }

                if (healthContainer == null)
                {
                    var portrait = comp.gameObject.GetComponentInChildren<UnitPortraitPartView>(true);
                    if (portrait != null) healthContainer = portrait.transform.parent;
                }

                if (healthContainer == null) healthContainer = comp.transform;

                // evitar duplicados
                var already = healthContainer.Find("ManaBar");
                if (already != null)
                {
                    Refresh(already.gameObject, unit);
                    return;
                }

                // crear
                CreateManaBar(healthContainer, hpProg, unit);
            }
            catch (Exception ex)
            {
                Log.Error("[ManaBarPatch] EX", ex);
            }
        }

        private static void CreateManaBar(Transform healthContainer, UnitHealthPartProgressView hpProg, UnitEntityData unit)
        {
            // localizamos el RectTransform de la barra de HP para clonar geometría base
            RectTransform hpRT = null;
            if (hpProg != null) hpRT = hpProg.GetComponent<RectTransform>();
            if (hpRT == null) hpRT = healthContainer as RectTransform;

            // Holder para aislar de LayoutGroups y controlar ancho/alto
            var holder = new GameObject("ManaBar_Holder", typeof(RectTransform));
            var holderRT = holder.GetComponent<RectTransform>();
            holder.transform.SetParent(healthContainer, false);

            bool parentHasLayout =
                (healthContainer.GetComponent<HorizontalLayoutGroup>() != null) ||
                (healthContainer.GetComponent<VerticalLayoutGroup>() != null) ||
                (healthContainer.GetComponent<GridLayoutGroup>() != null);

            var holderLE = holder.AddComponent<LayoutElement>();
            holderLE.ignoreLayout = !parentHasLayout;
            holderLE.minHeight = ManaUIConfig.BAR_HEIGHT;
            holderLE.preferredHeight = ManaUIConfig.BAR_HEIGHT;
            holderLE.flexibleWidth = 1f;

            if (hpRT != null)
            {
                holderRT.SetSiblingIndex(hpRT.GetSiblingIndex() + 1);
                holderRT.anchorMin = hpRT.anchorMin;
                holderRT.anchorMax = hpRT.anchorMax;
                holderRT.pivot = hpRT.pivot;
                // aplicamos nuestra altura, conservando ancho base
                holderRT.sizeDelta = new Vector2(hpRT.sizeDelta.x, ManaUIConfig.BAR_HEIGHT);
                holderRT.anchoredPosition = hpRT.anchoredPosition + new Vector2(ManaUIConfig.X_OFFSET, ManaUIConfig.Y_OFFSET);
            }
            else
            {
                holderRT.anchorMin = new Vector2(0f, 0f);
                holderRT.anchorMax = new Vector2(1f, 0f);
                holderRT.pivot = new Vector2(0.5f, 0f);
                holderRT.sizeDelta = new Vector2(0f, ManaUIConfig.BAR_HEIGHT);
                holderRT.anchoredPosition = new Vector2(ManaUIConfig.X_OFFSET, ManaUIConfig.Y_OFFSET);
            }

            // Control del ancho extra
            if (parentHasLayout)
            {
                holderLE.flexibleWidth = 0f;
                float baseWidth = (hpRT != null) ? hpRT.rect.width : holderLE.preferredWidth;
                holderLE.minWidth = baseWidth + ManaUIConfig.WIDTH_DELTA;
                holderLE.preferredWidth = baseWidth + ManaUIConfig.WIDTH_DELTA;
            }
            else
            {
                var offMin = holderRT.offsetMin;
                var offMax = holderRT.offsetMax;
                offMin.x -= ManaUIConfig.WIDTH_DELTA * 0.5f;
                offMax.x += ManaUIConfig.WIDTH_DELTA * 0.5f;
                holderRT.offsetMin = offMin;
                holderRT.offsetMax = offMax;
            }

            // Rect real de la barra dentro del holder
            var manaBar = new GameObject("ManaBar", typeof(RectTransform));
            var manaRT = manaBar.GetComponent<RectTransform>();
            manaBar.transform.SetParent(holderRT, false);
            manaRT.anchorMin = Vector2.zero;
            manaRT.anchorMax = Vector2.one;
            manaRT.pivot = new Vector2(0.5f, 0.5f);
            manaRT.sizeDelta = Vector2.zero;
            manaRT.anchoredPosition = Vector2.zero;

            // padding interno
            manaRT.offsetMin = new Vector2(ManaUIConfig.PAD_L, ManaUIConfig.PAD_B);
            manaRT.offsetMax = new Vector2(-ManaUIConfig.PAD_R, -ManaUIConfig.PAD_T);

            // BG
            var bgGO = new GameObject("BG", typeof(RectTransform), typeof(Image));
            var bgRT = bgGO.GetComponent<RectTransform>();
            bgGO.transform.SetParent(manaBar.transform, false);
            bgRT.anchorMin = Vector2.zero; bgRT.anchorMax = Vector2.one;
            bgRT.offsetMin = Vector2.zero; bgRT.offsetMax = Vector2.zero;
            var bgImg = bgGO.GetComponent<Image>();
            bgImg.color = ManaUIConfig.BG_COLOR;

            // FILL
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
            fillImg.color = ManaUIConfig.FILL_COLOR;

            // sprite/material de HP (cacheados)
            GetHpSpriteAndMatCached(healthContainer, hpProg, out Sprite hpSprite, out Material hpMat);

            if (hpSprite != null)
            {
                fillImg.sprite = hpSprite;
                fillImg.material = hpMat;
                bgImg.sprite = hpSprite;
                bgImg.type = Image.Type.Sliced;
            }
            else
            {
                // fallback: 1x1
                var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                tex.SetPixel(0, 0, Color.white); tex.Apply();
                var spr = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 100f);
                fillImg.sprite = spr;
                bgImg.sprite = spr; bgImg.type = Image.Type.Sliced;
            }

            bgImg.raycastTarget = false;
            fillImg.raycastTarget = false;

            // View + primer pintado + subscripción
            var view = manaBar.AddComponent<ManaBarView>();
            view.Attach(fillImg);
            view.SetColor(ManaUIConfig.FILL_COLOR);

            var (current, max) = ManaProvider.Get(unit);
            view.UpdateValue(current, max);
            ManaEvents.Subscribe(unit, delegate (int c, int m) { view.UpdateValue(c, m); });

            // Cleanup
            var destroyHook = healthContainer.gameObject.AddComponent<OnDestroyHook>();
            destroyHook.OnDestroyed += delegate { ManaEvents.Unsubscribe(unit, view); };
        }

        private static void GetHpSpriteAndMatCached(Transform key, UnitHealthPartProgressView hpProg, out Sprite sprite, out Material mat)
        {
            if (!_hpSpriteCache.TryGetValue(key, out sprite) || !_hpMaterialCache.TryGetValue(key, out mat))
            {
                sprite = null; mat = null;
                try
                {
                    if (hpProg != null)
                    {
                        var images = hpProg.GetComponentsInChildren<Image>(true);
                        for (int i = 0; i < images.Length; i++)
                        {
                            var im = images[i];
                            if (im != null && im.type == Image.Type.Filled && im.sprite != null)
                            {
                                sprite = im.sprite;
                                mat = im.material;
                                break;
                            }
                        }
                    }
                }
                catch { }

                _hpSpriteCache[key] = sprite;
                _hpMaterialCache[key] = mat;
            }
        }

        private static void Refresh(GameObject manaBar, UnitEntityData unit)
        {
            var view = manaBar.GetComponent<ManaBarView>();
            if (view == null) return;
            var (current, max) = ManaProvider.Get(unit);
            view.UpdateValue(current, max);
            ManaEvents.Subscribe(unit, delegate (int c, int m) { view.UpdateValue(c, m); });
        }

        // ------- helpers VM -------
        private static UnitEntityData TryGetUnitFromVM(object partyCharacterVM, object viewInstance)
        {
            var u = GetUnitFromAny(partyCharacterVM);
            if (u != null) return u;

            if (viewInstance != null)
            {
                var portraitField = GetFieldRecursive(viewInstance.GetType(), "m_PortraitView");
                var portraitView = (portraitField != null) ? portraitField.GetValue(viewInstance) as Component : null;
                var portraitVM = (portraitView != null)
                    ? GetPropertyValue(portraitView, "ViewModel")
                    : null;
                var u2 = GetUnitFromAny(portraitVM);
                if (u2 != null) return u2;

                var hpField = GetFieldRecursive(viewInstance.GetType(), "m_HealthProgressView");
                var hpView = (hpField != null) ? hpField.GetValue(viewInstance) as Component : null;
                var hpVM = (hpView != null)
                    ? GetPropertyValue(hpView, "ViewModel")
                    : null;
                var u3 = GetUnitFromAny(hpVM);
                if (u3 != null) return u3;
            }

            return null;
        }

        private static object GetPropertyValue(object obj, string name)
        {
            try
            {
                var p = obj.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                return p?.GetValue(obj, null);
            }
            catch { return null; }
        }

        private static UnitEntityData GetUnitFromAny(object obj, int depth = 0)
        {
            if (obj == null || depth > 2) return null;

            var t = obj.GetType();

            // propiedades
            var props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            for (int i = 0; i < props.Length; i++)
            {
                var p = props[i];
                try
                {
                    if (p.PropertyType == typeof(UnitEntityData))
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

            // campos
            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            for (int i = 0; i < fields.Length; i++)
            {
                var f = fields[i];
                try
                {
                    if (f.FieldType == typeof(UnitEntityData))
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
