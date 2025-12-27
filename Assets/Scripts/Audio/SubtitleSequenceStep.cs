using System;
using System.Collections;
using UnityEngine;
using Core.Sequence;

namespace Audio
{
    /// <summary>
    /// 字幕表示専用のシーケンスステップ
    /// </summary>
    [Serializable]
    public class SubtitleSequenceStep : ISequenceStep
    {
        [SerializeField]
        [TextArea(2, 5)]
        [Tooltip("表示する字幕テキスト")]
        private string subtitleText;

        private ISubtitleDisplay subtitleDisplay;

        /// <summary>
        /// 字幕テキスト
        /// </summary>
        public string SubtitleText
        {
            get => subtitleText;
            set => subtitleText = value;
        }

        /// <summary>
        /// 字幕ディスプレイを設定
        /// </summary>
        public void SetSubtitleDisplay(ISubtitleDisplay display)
        {
            subtitleDisplay = display;
        }

        /// <summary>
        /// ステップを実行（字幕を表示）
        /// </summary>
        public IEnumerator Execute()
        {
            if (string.IsNullOrEmpty(subtitleText))
            {
                Debug.LogWarning("Subtitle text is empty. Skipping subtitle display.");
                yield break;
            }

            if (subtitleDisplay == null)
            {
                Debug.LogError("SubtitleDisplay is not set. Cannot show subtitle.");
                yield break;
            }

            subtitleDisplay.ShowSubtitle(subtitleText);

            // 字幕は表示したまま（次のステップまたはシーケンス終了まで）
            yield break;
        }

        /// <summary>
        /// このステップが有効かどうか
        /// </summary>
        public bool IsValid => !string.IsNullOrEmpty(subtitleText);
    }
}
