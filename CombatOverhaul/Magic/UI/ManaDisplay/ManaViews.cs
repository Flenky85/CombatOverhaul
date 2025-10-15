using Kingmaker.EntitySystem.Entities;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatOverhaul.Magic.UI.ManaDisplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    internal sealed class ManaBarView : MonoBehaviour
    {
        [SerializeField] private Image _fill;

        private const float EPS = 0.0001f;
        private float _lastFill = -1f;

        public void Attach(Image fillImage) => _fill = fillImage;

        public void SetColor(Color c)
        {
            if (_fill != null) _fill.color = c;
        }

        public void UpdateValue(int current, int max)
        {
            if (_fill == null) return;

            float target = (max > 0) ? (current / (float)max) : 0f;
            target = Mathf.Clamp01(target);

            if (Mathf.Abs(target - _lastFill) < EPS) return;

            _fill.fillAmount = target;
            _lastFill = target;
        }
    }

    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    internal sealed class ManaTextView : MonoBehaviour
    {
        private TextMeshProUGUI _tmp;
        private Text _uiText;

        private UnitEntityData _unit;           
        private int _lastCur = int.MinValue;
        private int _lastMax = int.MinValue;
        private int _lastRegen = int.MinValue;  

        private StringBuilder _sb;

        public void Attach(TextMeshProUGUI tmp, Text uiText)
        {
            _tmp = tmp; _uiText = uiText;
            _sb ??= new StringBuilder(32);

            if (_tmp != null) { _tmp.raycastTarget = false; _tmp.richText = true; }
            if (_uiText != null) { _uiText.raycastTarget = false; _uiText.supportRichText = true; }
        }

        public void SetUnit(UnitEntityData unit) => _unit = unit;

        public void SetColor(Color c)
        {
            if (_tmp != null) _tmp.color = c;
            if (_uiText != null) _uiText.color = c;
        }

        public void UpdateValue(int current, int max)
        {
            int regen = ManaProvider.GetRegen(_unit);

            if (current == _lastCur && max == _lastMax && regen == _lastRegen) return;
            _lastCur = current; _lastMax = max; _lastRegen = regen;

            float smallScale = ManaUITextConfig.FONT_SCALE_MAX; 

            if (_tmp != null)
            {
                int percentSmall = Mathf.RoundToInt(100f * smallScale);

                _sb.Clear();

                if (regen > 0)
                {
                    _sb.Append("<size=");
                    _sb.Append(percentSmall);
                    _sb.Append("%>+");
                    _sb.Append(regen);
                    _sb.Append("</size> ");
                }

                _sb.Append(current);
                _sb.Append("/<size=");
                _sb.Append(percentSmall);
                _sb.Append("%>");
                _sb.Append(max);
                _sb.Append("</size>");

                _tmp.text = _sb.ToString();
                return;
            }

            if (_uiText != null)
            {
                int smallPts = Mathf.Max(1, Mathf.RoundToInt(_uiText.fontSize * smallScale));

                _sb.Clear();

                if (regen > 0)
                {
                    _sb.Append("<size=");
                    _sb.Append(smallPts);
                    _sb.Append(">+");
                    _sb.Append(regen);
                    _sb.Append("</size> ");
                }

                _sb.Append(current);
                _sb.Append("/<size=");
                _sb.Append(smallPts);
                _sb.Append(">");
                _sb.Append(max);
                _sb.Append("</size>");

                _uiText.text = _sb.ToString();
            }
        }

    }
}
