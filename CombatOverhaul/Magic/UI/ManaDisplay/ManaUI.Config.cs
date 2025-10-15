using UnityEngine;

namespace CombatOverhaul.Magic.UI.ManaDisplay
{
    internal static class ManaUIConfig
    {
        public const float BAR_THICKNESS = 6f;
        public const float BAR_HEIGHT = 0f;

        public const float X_OFFSET = 0f;
        public const float Y_OFFSET = 0f;

        public const float PAD_L = 1f;
        public const float PAD_R = 1f;
        public const float PAD_T = 1f;
        public const float PAD_B = 1f;

        public static readonly Color BG_COLOR = new Color(0f, 0f, 0f, 0.0f);
        public static readonly Color FILL_COLOR = new Color(0.12f, 0.45f, 1f, 1f);
    }

    internal static class ManaUITextConfig
    {
        public const float OFFSET_X = 0f;
        public const float OFFSET_Y = 16f;

        public const float FONT_SCALE_CURRENT = 1f;
        public const float FONT_SCALE_MAX = 0.8f;

        public static readonly Color TEXT_COLOR = ManaUIConfig.FILL_COLOR;
    }
}
