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
        /// ステップを実行（指定されたインデックスの字幕を表示）
        /// </summary>
        /// <param name="index">表示する字幕のインデックス</param>
        public override IEnumerator Execute(int index)
        {
            if (subtitles == null || index < 0 || index >= subtitles.Count)
            {
                Debug.LogWarning($"SubtitleManager: Index {index} is out of range (Count: {subtitles?.Count ?? 0}).");
                yield break;
            }

            if (subtitleDisplay == null)
            {
                Debug.LogError("SubtitleDisplay is not assigned in SubtitleManager.");
                yield break;
            }

            string subtitle = subtitles[index];
            if (!string.IsNullOrEmpty(subtitle))
            {
                subtitleDisplay.ShowSubtitle(subtitle);
            }

            // 字幕は表示したまま（次のステップまたはシーケンス終了まで）
            yield break;
        }
    }
}
