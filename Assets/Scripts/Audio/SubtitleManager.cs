using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Sequence;

namespace Audio
{
    /// <summary>
    /// 字幕管理クラス
    /// 複数の字幕を管理し、シーケンスに従って順次表示する
    /// </summary>
    public class SubtitleManager : SequenceStep
    {
        [SerializeField]
        [Tooltip("表示する字幕テキストのリスト")]
        private List<string> subtitles = new List<string>();

        [SerializeField]
        [Tooltip("字幕表示コンポーネント")]
        private SubtitleDisplayTMP subtitleDisplay;

        private int currentIndex = 0;

        /// <summary>
        /// 字幕のリスト
        /// </summary>
        public List<string> Subtitles
        {
            get => subtitles;
            set => subtitles = value;
        }

        /// <summary>
        /// ステップを実行（現在のインデックスの字幕を表示）
        /// </summary>
        public override IEnumerator Execute()
        {
            if (subtitles == null || subtitles.Count == 0)
            {
                Debug.LogWarning("No subtitles in SubtitleManager.");
                yield break;
            }

            if (currentIndex >= subtitles.Count)
            {
                Debug.LogWarning("SubtitleManager: Current index exceeds subtitles count.");
                yield break;
            }

            if (subtitleDisplay == null)
            {
                Debug.LogError("SubtitleDisplay is not assigned in SubtitleManager.");
                yield break;
            }

            string subtitle = subtitles[currentIndex];
            if (!string.IsNullOrEmpty(subtitle))
            {
                subtitleDisplay.ShowSubtitle(subtitle);
            }

            currentIndex++;

            // 字幕は表示したまま（次のステップまたはシーケンス終了まで）
            yield break;
        }

        /// <summary>
        /// インデックスをリセット
        /// </summary>
        public void Reset()
        {
            currentIndex = 0;
        }
    }
}
