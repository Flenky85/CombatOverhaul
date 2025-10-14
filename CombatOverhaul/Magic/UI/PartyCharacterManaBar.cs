using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.MVVM._PCView.Party;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatOverhaul.Magic.UI
{
    // =========================================================
    // =====================  BAR CONFIG  ======================
    // =========================================================
    internal static class ManaUIConfig
    {
        // Vertical bar thickness (WIDTH) in px
        public const float BAR_THICKNESS = 6f;

        // Desired height. If 0 => use HP bar height
        public const float BAR_HEIGHT = 0f;

        // Offsets relative to HP (px). +X = right; +Y = up
        public const float X_OFFSET = 0f;
        public const float Y_OFFSET = 0f;

        // Inner padding of the rect (L, R, T, B)
        public const float PAD_L = 1f;
        public const float PAD_R = 1f;
        public const float PAD_T = 1f;
        public const float PAD_B = 1f;

        // Colors
        public static readonly Color BG_COLOR = new Color(0f, 0f, 0f, 0.0f);
        public static readonly Color FILL_COLOR = new Color(0.12f, 0.45f, 1f, 1f);
    }

    internal static class ManaUITextConfig
    {
        // Posición respecto a la esquina sup. derecha del contenedor de vida
        public const float OFFSET_X = 0f;   // izquierda (negativo) / derecha (positivo)
        public const float OFFSET_Y = 16f;  // abajo (negativo)   / arriba (positivo)

        // Tamaños independientes
        public const float FONT_SCALE_CURRENT = 1f; // tamaño para el "actual" y la barra "/"
        public const float FONT_SCALE_MAX = 0.8f; // tamaño para el "máximo" (ej. 20% más pequeño que current)

        public static readonly Color TEXT_COLOR = ManaUIConfig.FILL_COLOR; // mismo azul que la barra
    }


    // =========================================================
    // =================  PUBLIC MANA UI API  ==================
    // =========================================================
    public static class ManaUI
    {
        public static void SetManaResource(Kingmaker.Blueprints.BlueprintAbilityResource res)
            => ManaProvider.ManaResource = res;

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

            int max = ManaCalc.CalcMaxMana(unit); 
            var coll = desc.Resources;
            int cur = coll.ContainsResource(ManaResource) ? coll.GetResourceAmount(ManaResource) : 0;

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
            if (!_subs.TryGetValue(unit, out var list))
            {
                list = new List<Action<int, int>>();
                _subs[unit] = list;
            }
            if (!list.Contains(cb)) list.Add(cb);
        }

        public static void Unsubscribe(UnitEntityData unit, object owner)
        {
            if (unit == null) return;
            if (_subs.TryGetValue(unit, out var list))
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
            if (_subs.TryGetValue(unit, out var list))
            {
                var copy = list.ToArray();
                for (int i = 0; i < copy.Length; i++)
                    copy[i]?.Invoke(current, max);
            }
        }
    }

    internal sealed class ManaBarView : MonoBehaviour
    {
        [SerializeField] private Image _fill;

        public void Attach(Image fillImage) => _fill = fillImage;

        public void SetColor(Color c)
        {
            if (_fill != null) _fill.color = c;
        }

        public void UpdateValue(int current, int max)
        {
            if (_fill != null)
                _fill.fillAmount = max > 0 ? Mathf.Clamp01(current / (float)max) : 0f;
        }
    }

    internal sealed class OnDestroyHook : MonoBehaviour
    {
        public Action OnDestroyed;
        private void OnDestroy() => OnDestroyed?.Invoke();
    }

    internal sealed class ManaTextView : MonoBehaviour
    {
        private TextMeshProUGUI _tmp;
        private Text _uiText;

        public void Attach(TextMeshProUGUI tmp, Text uiText)
        {
            _tmp = tmp; _uiText = uiText;

            if (_tmp != null)
            {
                _tmp.raycastTarget = false;
                _tmp.richText = true; // necesario para <size=...>
            }
            if (_uiText != null)
            {
                _uiText.raycastTarget = false;
                _uiText.supportRichText = true; // necesario para <size=...>
            }
        }

        public void SetColor(Color c)
        {
            if (_tmp != null) _tmp.color = c;
            if (_uiText != null) _uiText.color = c;
        }

        public void UpdateValue(int current, int max)
        {
            // Base = el mayor de los dos escalados (para que el "grande" sea el tamaño del componente)
            float baseScale = Mathf.Max(ManaUITextConfig.FONT_SCALE_CURRENT, ManaUITextConfig.FONT_SCALE_MAX);

            if (_tmp != null)
            {
                // En TMP podemos usar porcentaje relativo
                int percentSmall = Mathf.RoundToInt(100f * (ManaUITextConfig.FONT_SCALE_MAX / baseScale));
                string s = $"{current}/<size={percentSmall}%>{max}</size>";
                _tmp.text = s;
                return;
            }

            if (_uiText != null)
            {
                // En UI.Text el <size=> es absoluto en puntos
                int smallPts = Mathf.Max(1, Mathf.RoundToInt(_uiText.fontSize * (ManaUITextConfig.FONT_SCALE_MAX / baseScale)));
                string s = $"{current}/<size={smallPts}>{max}</size>";
                _uiText.text = s;
            }
        }
    }

    // =========================================================
    // ======================  PATCH  ==========================
    // =========================================================
    [HarmonyPatch(typeof(PartyCharacterPCView), "BindViewImplementation")]
    internal static class PartyCharacterManaBarPCPatch
    {
        private static readonly Dictionary<Transform, Sprite> _hpSpriteCache = new Dictionary<Transform, Sprite>();
        private static readonly Dictionary<Transform, Material> _hpMaterialCache = new Dictionary<Transform, Material>();

        // Postfix tipado: acceso directo a API pública de la vista PC
        static void Postfix(PartyCharacterPCView __instance)
        {
            try
            {
                var unit = __instance?.UnitEntityData;
                if (unit == null) return;

                Transform healthContainer = null;
                UnitHealthPartProgressView hpProg = __instance.GetComponentInChildren<UnitHealthPartProgressView>(true);

                if (hpProg != null)
                    healthContainer = hpProg.transform.parent;

                var hpTxt = __instance.GetComponentInChildren<UnitHealthPartTextView>(true); // <-- necesitaremos esta ref
                if (healthContainer == null && hpTxt != null)
                    healthContainer = hpTxt.transform.parent;

                if (healthContainer == null)
                {
                    var portrait = __instance.GetComponentInChildren<UnitPortraitPartView>(true);
                    if (portrait != null) healthContainer = portrait.transform.parent;
                }
                if (healthContainer == null) healthContainer = __instance.transform;

                // Evitar duplicados (barra)
                var alreadyBar = healthContainer.Find("ManaBar_V");
                if (alreadyBar != null)
                {
                    Refresh(alreadyBar.gameObject, unit);
                    CreateOrRefreshManaText(hpTxt, unit); // <-- asegurar el texto también
                    return;
                }

                // Crear barra y texto
                CreateManaBarVertical(healthContainer, hpProg, unit);
                CreateOrRefreshManaText(hpTxt, unit); // <-- crear el número si hay contenedor de HP text
            }
            catch (Exception ex)
            {
                Log.Error("[ManaBarPatch-PC] EX", ex);
            }
        }

        // ----- NUEVO: creación/refresh del texto -----
        // ----- NUEVO: creación/refresh del texto (con ignoreLayout) -----
        private static void CreateOrRefreshManaText(UnitHealthPartTextView hpTextView, UnitEntityData unit)
        {
            if (hpTextView == null || unit == null) return;

            var parent = hpTextView.transform; // hereda el hover de los números de vida
            var exists = parent.Find("ManaText");

            // Buscar plantilla de texto (TMP preferido)
            var templateTMP = hpTextView.GetComponentInChildren<TextMeshProUGUI>(true);
            var templateUI = (templateTMP == null) ? hpTextView.GetComponentInChildren<Text>(true) : null;

            if (exists != null)
            {
                // --- ya existe: asegurar que sale del layout y aplicar offset ---
                var rt = exists.GetComponent<RectTransform>();
                if (rt != null)
                {
                    var le = exists.GetComponent<LayoutElement>() ?? exists.gameObject.AddComponent<LayoutElement>();
                    le.ignoreLayout = true; // <- CLAVE para que funcionen los offsets

                    // Anclamos arriba-derecha del contenedor de vida
                    rt.anchorMin = new Vector2(1f, 1f);
                    rt.anchorMax = new Vector2(1f, 1f);
                    rt.pivot = new Vector2(1f, 1f);
                    rt.sizeDelta = Vector2.zero;

                    // Ahora sí, estos offsets MUEVEN el texto
                    rt.anchoredPosition = new Vector2(ManaUITextConfig.OFFSET_X, ManaUITextConfig.OFFSET_Y);

                    exists.transform.SetAsLastSibling(); // por si hay solapes, que dibuje encima
                }

                var view = exists.GetComponent<ManaTextView>() ?? exists.gameObject.AddComponent<ManaTextView>();
                var (c, m) = ManaProvider.Get(unit);
                view.UpdateValue(c, m);
                ManaEvents.Subscribe(unit, (cc, mm) => view.UpdateValue(cc, mm));

                // Limpieza
                var destroyHook = parent.gameObject.GetComponent<OnDestroyHook>() ?? parent.gameObject.AddComponent<OnDestroyHook>();
                destroyHook.OnDestroyed += () => ManaEvents.Unsubscribe(unit, view);
                return;
            }

            // ---- Crear nuevo ManaText ----
            var go = new GameObject("ManaText", typeof(RectTransform));
            var rtNew = go.GetComponent<RectTransform>();
            go.transform.SetParent(parent, false);

            // ¡Fuera del layout! (igual que hacemos con la barra según su lógica)
            var leNew = go.AddComponent<LayoutElement>();
            leNew.ignoreLayout = true; // <- CLAVE

            // Ancla y posición (top-right del contenedor de vida)
            rtNew.anchorMin = new Vector2(1f, 1f);
            rtNew.anchorMax = new Vector2(1f, 1f);
            rtNew.pivot = new Vector2(1f, 1f);
            rtNew.sizeDelta = Vector2.zero;
            rtNew.anchoredPosition = new Vector2(ManaUITextConfig.OFFSET_X, ManaUITextConfig.OFFSET_Y);

            TextMeshProUGUI tmp = null;
            Text uiText = null;

            if (templateTMP != null)
            {
                // ---- donde creas TMP ----
                tmp = go.AddComponent<TextMeshProUGUI>();
                tmp.font = templateTMP.font;
                tmp.fontSharedMaterial = templateTMP.fontSharedMaterial;
                // ANTES: tmp.fontSize = Mathf.Max(8f, templateTMP.fontSize * ManaUITextConfig.FONT_SCALE);
                // AHORA:
                tmp.fontSize = Mathf.Max(8f, templateTMP.fontSize * Mathf.Max(ManaUITextConfig.FONT_SCALE_CURRENT, ManaUITextConfig.FONT_SCALE_MAX));
                tmp.alignment = TextAlignmentOptions.TopRight;
                tmp.enableWordWrapping = false;
                tmp.raycastTarget = false;
                tmp.color = ManaUITextConfig.TEXT_COLOR;
            }
            else
            {
                // ---- donde creas UI.Text ----
                uiText = go.AddComponent<Text>();
                if (templateUI != null)
                {
                    uiText.font = templateUI.font;
                    uiText.resizeTextForBestFit = templateUI.resizeTextForBestFit;
                    // ANTES: uiText.fontSize = Mathf.Max(8, Mathf.RoundToInt(templateUI.fontSize * ManaUITextConfig.FONT_SCALE));
                    // AHORA:
                    uiText.fontSize = Mathf.Max(8, Mathf.RoundToInt(templateUI.fontSize * Mathf.Max(ManaUITextConfig.FONT_SCALE_CURRENT, ManaUITextConfig.FONT_SCALE_MAX)));
                }
                uiText.alignment = TextAnchor.UpperRight;
                uiText.horizontalOverflow = HorizontalWrapMode.Overflow;
                uiText.verticalOverflow = VerticalWrapMode.Overflow;
                uiText.raycastTarget = false;
                uiText.color = ManaUITextConfig.TEXT_COLOR;

            }

            var viewComp = go.AddComponent<ManaTextView>();
            viewComp.Attach(tmp, uiText);
            viewComp.SetColor(ManaUITextConfig.TEXT_COLOR);

            var (current, max) = ManaProvider.Get(unit);
            viewComp.UpdateValue(current, max);
            ManaEvents.Subscribe(unit, (c, m) => viewComp.UpdateValue(c, m));

            // Que dibuje por encima del de vida
            go.transform.SetAsLastSibling();

            // Limpieza al destruir el retrato
            var destroy = parent.gameObject.GetComponent<OnDestroyHook>() ?? parent.gameObject.AddComponent<OnDestroyHook>();
            destroy.OnDestroyed += () => ManaEvents.Unsubscribe(unit, viewComp);
        }


        private static void CreateManaBarVertical(Transform healthContainer, UnitHealthPartProgressView hpProg, UnitEntityData unit)
        {
            RectTransform hpRT = hpProg != null ? hpProg.GetComponent<RectTransform>() : healthContainer as RectTransform;

            // Holder: vertical bar to the RIGHT of the HP bar
            var holder = new GameObject("ManaBar_V", typeof(RectTransform));
            var holderRT = holder.GetComponent<RectTransform>();
            holder.transform.SetParent(healthContainer, false);

            bool parentHasLayout =
                healthContainer.GetComponent<HorizontalLayoutGroup>() != null ||
                healthContainer.GetComponent<VerticalLayoutGroup>() != null ||
                healthContainer.GetComponent<GridLayoutGroup>() != null;

            var holderLE = holder.AddComponent<LayoutElement>();
            holderLE.ignoreLayout = !parentHasLayout;

            float targetHeight = ManaUIConfig.BAR_HEIGHT > 0f
                ? ManaUIConfig.BAR_HEIGHT
                : hpRT != null ? Mathf.Max(1f, hpRT.rect.height) : 12f;

            if (parentHasLayout)
            {
                holderLE.minWidth = ManaUIConfig.BAR_THICKNESS;
                holderLE.preferredWidth = ManaUIConfig.BAR_THICKNESS;
                holderLE.flexibleWidth = 0f;
            }

            if (hpRT != null)
            {
                holderRT.SetSiblingIndex(hpRT.GetSiblingIndex() + 1);
                holderRT.anchorMin = hpRT.anchorMin;
                holderRT.anchorMax = hpRT.anchorMax;
                holderRT.pivot = hpRT.pivot;
                holderRT.sizeDelta = new Vector2(ManaUIConfig.BAR_THICKNESS, targetHeight);


                // Position to the right of HP + offset
                float x = hpRT.anchoredPosition.x + hpRT.rect.width * 0.5f + ManaUIConfig.BAR_THICKNESS * 0.5f + ManaUIConfig.X_OFFSET;
                float y = hpRT.anchoredPosition.y + ManaUIConfig.Y_OFFSET;
                holderRT.anchoredPosition = new Vector2(x, y);
            }
            else
            {
                holderRT.anchorMin = new Vector2(1f, 0.5f);
                holderRT.anchorMax = new Vector2(1f, 0.5f);
                holderRT.pivot = new Vector2(0.5f, 0.5f);
                holderRT.sizeDelta = new Vector2(ManaUIConfig.BAR_THICKNESS, targetHeight);
                holderRT.anchoredPosition = new Vector2(ManaUIConfig.X_OFFSET, ManaUIConfig.Y_OFFSET);
            }

            // Inner rect of the bar
            var manaBar = new GameObject("Bar", typeof(RectTransform));
            var manaRT = manaBar.GetComponent<RectTransform>();
            manaBar.transform.SetParent(holderRT, false);
            manaRT.anchorMin = Vector2.zero;
            manaRT.anchorMax = Vector2.one;
            manaRT.pivot = new Vector2(0.5f, 0.5f);
            manaRT.sizeDelta = Vector2.zero;
            manaRT.anchoredPosition = Vector2.zero;
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

            // FILL vertical bottom->top
            var fillGO = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            var fillRT = fillGO.GetComponent<RectTransform>();
            fillGO.transform.SetParent(manaBar.transform, false);
            fillRT.anchorMin = new Vector2(0f, 0f);
            fillRT.anchorMax = new Vector2(1f, 1f);
            fillRT.offsetMin = Vector2.zero; fillRT.offsetMax = Vector2.zero;

            var fillImg = fillGO.GetComponent<Image>();
            fillImg.type = Image.Type.Filled;
            fillImg.fillMethod = Image.FillMethod.Vertical;
            fillImg.fillOrigin = (int)Image.OriginVertical.Bottom; 
            fillImg.color = ManaUIConfig.FILL_COLOR;

            // Reuse HP sprite/material (cached)
            GetHpSpriteAndMatCached(healthContainer, hpProg, out var hpSprite, out var hpMat);
            if (hpSprite != null)
            {
                fillImg.sprite = hpSprite;
                fillImg.material = hpMat;
                bgImg.sprite = hpSprite; bgImg.type = Image.Type.Sliced;
            }
            else
            {
                var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                tex.SetPixel(0, 0, Color.white); tex.Apply();
                var spr = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 100f);
                fillImg.sprite = spr;
                bgImg.sprite = spr; bgImg.type = Image.Type.Sliced;
            }

            bgImg.raycastTarget = false;
            fillImg.raycastTarget = false;

            // View + first paint + subscription
            var view = manaBar.AddComponent<ManaBarView>();
            view.Attach(fillImg);
            view.SetColor(ManaUIConfig.FILL_COLOR);

            var (current, max) = ManaProvider.Get(unit);
            view.UpdateValue(current, max);
            ManaEvents.Subscribe(unit, (c, m) => view.UpdateValue(c, m));

            // Cleanup
            var destroyHook = healthContainer.gameObject.AddComponent<OnDestroyHook>();
            destroyHook.OnDestroyed += () => ManaEvents.Unsubscribe(unit, view);
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
                catch { /* ignore */ }

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
            ManaEvents.Subscribe(unit, (c, m) => view.UpdateValue(c, m));
        }

    }
}
