using UnityEngine;
using TMPro;

namespace Audio
{
    /// <summary>
    /// TextMeshProを使用した字幕表示コンポーネント
    /// </summary>
    public class SubtitleDisplayTMP : MonoBehaviour, ISubtitleDisplay
    {
        [SerializeField]
        [Tooltip("字幕を表示するTextMeshProコンポーネント（3D or UGUI）")]
        private TMP_Text subtitleText;

        private void Awake()
        {
            if (subtitleText == null)
            {
                subtitleText = GetComponent<TMP_Text>();
            }

            if (subtitleText == null)
            {
                Debug.LogError("TMP_Text component is not assigned and not found on this GameObject.");
            }
        }

        /// <summary>
        /// 字幕を表示する
        /// </summary>
        public void ShowSubtitle(string text)
        {
            if (subtitleText == null)
            {
                Debug.LogWarning("TMP_Text is not assigned.");
                return;
            }

            subtitleText.text = text;
            subtitleText.enabled = true;
        }

        /// <summary>
        /// 字幕を非表示にする
        /// </summary>
        public void HideSubtitle()
        {
            if (subtitleText != null)
            {
                subtitleText.enabled = false;
            }
        }

        /// <summary>
        /// 字幕をクリアする
        /// </summary>
        public void ClearSubtitle()
        {
            if (subtitleText != null)
            {
                subtitleText.text = string.Empty;
            }
        }
    }
}
