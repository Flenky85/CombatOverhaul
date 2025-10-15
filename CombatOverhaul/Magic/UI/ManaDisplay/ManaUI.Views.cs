using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatOverhaul.Magic.UI.ManaDisplay
{
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
                _tmp.richText = true;
            }
            if (_uiText != null)
            {
                _uiText.raycastTarget = false;
                _uiText.supportRichText = true;
            }
        }

        public void SetColor(Color c)
        {
            if (_tmp != null) _tmp.color = c;
            if (_uiText != null) _uiText.color = c;
        }

        public void UpdateValue(int current, int max)
        {
            float baseScale = Mathf.Max(ManaUITextConfig.FONT_SCALE_CURRENT, ManaUITextConfig.FONT_SCALE_MAX);

            if (_tmp != null)
            {
                int percentSmall = Mathf.RoundToInt(100f * (ManaUITextConfig.FONT_SCALE_MAX / baseScale));
                _tmp.text = $"{current}/<size={percentSmall}%>{max}</size>";
                return;
            }

            if (_uiText != null)
            {
                int smallPts = Mathf.Max(1, Mathf.RoundToInt(_uiText.fontSize * (ManaUITextConfig.FONT_SCALE_MAX / baseScale)));
                _uiText.text = $"{current}/<size={smallPts}>{max}</size>";
            }
        }
    }
}
