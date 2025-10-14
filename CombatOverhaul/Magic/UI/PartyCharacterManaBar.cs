using CombatOverhaul.Utils;
using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.MVVM._PCView.Party;
using System;
using System.Collections.Generic;
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
                // 1) Unit in the slot (public prop)
                var unit = __instance?.UnitEntityData; // public in PCView
                if (unit == null) return; // empty slot or no active VM

                // 2) Anchor container: try HP progress bar → HP text → portrait
                Transform healthContainer = null;
                UnitHealthPartProgressView hpProg =
                    __instance.GetComponentInChildren<UnitHealthPartProgressView>(true);

                if (hpProg != null)
                    healthContainer = hpProg.transform.parent;
                if (healthContainer == null)
                {
                    var hpTxt = __instance.GetComponentInChildren<UnitHealthPartTextView>(true);
                    if (hpTxt != null) healthContainer = hpTxt.transform.parent;
                }
                if (healthContainer == null)
                {
                    var portrait = __instance.GetComponentInChildren<UnitPortraitPartView>(true);
                    if (portrait != null) healthContainer = portrait.transform.parent;
                }
                if (healthContainer == null) healthContainer = __instance.transform;

                // 3) Prevent duplicates
                var already = healthContainer.Find("ManaBar_V");
                if (already != null)
                {
                    Refresh(already.gameObject, unit);
                    return;
                }

                // 4) Create vertical bar
                CreateManaBarVertical(healthContainer, hpProg, unit);
            }
            catch (Exception ex)
            {
                Log.Error("[ManaBarPatch-PC] EX", ex);
            }
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
