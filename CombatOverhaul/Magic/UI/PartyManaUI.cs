using System;
using System.Collections.Generic;
using CombatOverhaul.Magic.UI.ManaDisplay;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.MVVM._PCView.Party;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatOverhaul.Magic.UI
{
    /// <summary>
    /// Orquestador de la UI de Maná para PartyCharacterPCView.
    /// </summary>
    internal static class PartyManaUI
    {
        public static void Ensure(PartyCharacterPCView view)
        {
            try
            {
                var unit = view?.UnitEntityData;
                if (unit == null) return;

                var container = HealthContainerLocator.Find(view, out UnitHealthPartProgressView hpProg, out UnitHealthPartTextView hpTxt);

                var alreadyBar = container.Find("ManaBar_V");
                if (alreadyBar != null)
                {
                    ManaBarUI.Refresh(alreadyBar.gameObject, unit);
                    ManaTextUI.Ensure(hpTxt, unit);
                    return;
                }

                ManaBarUI.Create(container, hpProg, unit);
                ManaTextUI.Ensure(hpTxt, unit);
            }
            catch (Exception ex)
            {
                Debug.LogError("[PartyManaUI] EX: " + ex);
            }
        }
    }

    // ----------------- Helpers -----------------

    internal static class HealthContainerLocator
    {
        public static Transform Find(PartyCharacterPCView view,
                                     out UnitHealthPartProgressView hpProg,
                                     out UnitHealthPartTextView hpTxt)
        {
            hpProg = view.GetComponentInChildren<UnitHealthPartProgressView>(true);
            hpTxt = view.GetComponentInChildren<UnitHealthPartTextView>(true);

            Transform healthContainer = null;
            if (hpProg != null) healthContainer = hpProg.transform.parent;
            if (healthContainer == null && hpTxt != null) healthContainer = hpTxt.transform.parent;

            if (healthContainer == null)
            {
                var portrait = view.GetComponentInChildren<UnitPortraitPartView>(true);
                if (portrait != null) healthContainer = portrait.transform.parent;
            }

            return healthContainer ?? view.transform;
        }
    }

    internal static class RectUtil
    {
        public static void SetupTopRightRect(RectTransform rt, Vector2 anchoredOffset)
        {
            var le = rt.GetComponent<LayoutElement>() ?? rt.gameObject.AddComponent<LayoutElement>();
            le.ignoreLayout = true;

            rt.anchorMin = new Vector2(1f, 1f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.pivot = new Vector2(1f, 1f);
            rt.sizeDelta = Vector2.zero;
            rt.anchoredPosition = anchoredOffset;
        }
    }
    
    internal static class ManaEventUtil
    {
        public static void Resubscribe(UnitEntityData unit, object owner, Action<int, int> cb)
        {
            ManaEvents.Unsubscribe(unit, owner);
            ManaEvents.Subscribe(unit, cb);
        }
    }

    internal static class UISpriteUtil
    {
        private static Sprite _white;
        public static Sprite White
        {
            get
            {
                if (_white == null)
                {
                    var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                    tex.SetPixel(0, 0, Color.white);
                    tex.Apply();
                    _white = Sprite.Create(tex, new Rect(0, 0, 1, 1),
                                           new Vector2(0.5f, 0.5f), 100f);
                }
                return _white;
            }
        }
    }

    internal static class SpriteMaterialCache
    {
        private static readonly Dictionary<Transform, Sprite> _hpSpriteCache = new Dictionary<Transform, Sprite>();
        private static readonly Dictionary<Transform, Material> _hpMaterialCache = new Dictionary<Transform, Material>();

        public static void Get(Transform key, UnitHealthPartProgressView hpProg, out Sprite sprite, out Material mat)
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

        public static void ClearFor(Transform key)
        {
            if (key == null) return;
            _hpSpriteCache.Remove(key);
            _hpMaterialCache.Remove(key);
        }
    }

    internal static class ManaTextUI
    {
        public static void Ensure(UnitHealthPartTextView hpTextView, UnitEntityData unit)
        {
            if (hpTextView == null || unit == null) return;

            var parent = hpTextView.transform;
            var exists = parent.Find("ManaText");

            var templateTMP = hpTextView.GetComponentInChildren<TextMeshProUGUI>(true);
            var templateUI = (templateTMP == null) ? hpTextView.GetComponentInChildren<Text>(true) : null;

            if (exists != null)
            {
                var rt = exists.GetComponent<RectTransform>();
                if (rt != null)
                {
                    RectUtil.SetupTopRightRect(rt, new Vector2(ManaUITextConfig.OFFSET_X, ManaUITextConfig.OFFSET_Y));
                    exists.transform.SetAsLastSibling();
                }

                var view = exists.GetComponent<ManaTextView>() ?? exists.gameObject.AddComponent<ManaTextView>();
                view.SetUnit(unit);

                var (current, max) = ManaProvider.Get(unit);
                view.UpdateValue(current, max);

                ManaEventUtil.Resubscribe(unit, view, view.UpdateValue);

                var destroyHook = parent.gameObject.GetComponent<OnDestroyHook>() ?? parent.gameObject.AddComponent<OnDestroyHook>();
                destroyHook.OnDestroyed += delegate
                {
                    ManaEvents.Unsubscribe(unit, view);
                };
                return;
            }

            var go = new GameObject("ManaText", typeof(RectTransform));
            var rtNew = go.GetComponent<RectTransform>();
            go.transform.SetParent(parent, false);

            var leNew = go.AddComponent<LayoutElement>();
            leNew.ignoreLayout = true;

            RectUtil.SetupTopRightRect(rtNew, new Vector2(ManaUITextConfig.OFFSET_X, ManaUITextConfig.OFFSET_Y));

            TextMeshProUGUI tmp = null;
            Text uiText = null;

            if (templateTMP != null)
            {
                tmp = go.AddComponent<TextMeshProUGUI>();
                tmp.font = templateTMP.font;
                tmp.fontSharedMaterial = templateTMP.fontSharedMaterial;
                tmp.fontSize = Mathf.Max(8f, templateTMP.fontSize * Mathf.Max(ManaUITextConfig.FONT_SCALE_CURRENT, ManaUITextConfig.FONT_SCALE_MAX));
                tmp.alignment = TextAlignmentOptions.TopRight;
                tmp.enableWordWrapping = false;
                tmp.raycastTarget = false;
                tmp.color = ManaUITextConfig.TEXT_COLOR;
            }
            else
            {
                uiText = go.AddComponent<Text>();
                if (templateUI != null)
                {
                    uiText.font = templateUI.font;
                    uiText.resizeTextForBestFit = templateUI.resizeTextForBestFit;
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
            viewComp.SetUnit(unit);

            var (current2, max2) = ManaProvider.Get(unit);
            viewComp.UpdateValue(current2, max2);

            ManaEventUtil.Resubscribe(unit, viewComp, viewComp.UpdateValue);

            go.transform.SetAsLastSibling();

            var destroy = parent.gameObject.GetComponent<OnDestroyHook>() ?? parent.gameObject.AddComponent<OnDestroyHook>();
            destroy.OnDestroyed += delegate
            {
                ManaEvents.Unsubscribe(unit, viewComp);
            };
        }
    }

    internal static class ManaBarUI
    {
        public static void Create(Transform healthContainer, UnitHealthPartProgressView hpProg, UnitEntityData unit)
        {
            RectTransform hpRT = hpProg != null ? hpProg.GetComponent<RectTransform>() : healthContainer as RectTransform;

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

            var bgGO = new GameObject("BG", typeof(RectTransform), typeof(Image));
            var bgRT = bgGO.GetComponent<RectTransform>();
            bgGO.transform.SetParent(manaBar.transform, false);
            bgRT.anchorMin = Vector2.zero; bgRT.anchorMax = Vector2.one;
            bgRT.offsetMin = Vector2.zero; bgRT.offsetMax = Vector2.zero;
            var bgImg = bgGO.GetComponent<Image>();
            bgImg.color = ManaUIConfig.BG_COLOR;

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

            SpriteMaterialCache.Get(healthContainer, hpProg, out Sprite hpSprite, out Material hpMat);
            if (hpSprite != null)
            {
                fillImg.sprite = hpSprite;
                fillImg.material = hpMat;
                bgImg.sprite = hpSprite; bgImg.type = Image.Type.Sliced;
            }
            else
            {
                var spr = UISpriteUtil.White;
                fillImg.sprite = spr;
                bgImg.sprite = spr; bgImg.type = Image.Type.Sliced;
            }

            bgImg.raycastTarget = false;
            fillImg.raycastTarget = false;

            var view = manaBar.AddComponent<ManaBarView>();
            view.Attach(fillImg);
            view.SetColor(ManaUIConfig.FILL_COLOR);

            var (current, max) = ManaProvider.Get(unit);
            view.UpdateValue(current, max);

            ManaEventUtil.Resubscribe(unit, view, view.UpdateValue);

            var destroyHook = healthContainer.gameObject.GetComponent<OnDestroyHook>() ?? healthContainer.gameObject.AddComponent<OnDestroyHook>();
            destroyHook.OnDestroyed += delegate
            {
                ManaEvents.Unsubscribe(unit, view);
                SpriteMaterialCache.ClearFor(healthContainer);
            };
        }

        public static void Refresh(GameObject manaBar, UnitEntityData unit)
        {
            var view = manaBar.GetComponent<ManaBarView>();
            if (view == null) return;

            var (current, max) = ManaProvider.Get(unit);
            view.UpdateValue(current, max);

            ManaEventUtil.Resubscribe(unit, view, view.UpdateValue);
        }
    }
}
