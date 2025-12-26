using System;
using UnityEngine;

namespace AudioSubtitle
{
    /// <summary>
    /// 音声と字幕のペアデータ
    /// </summary>
    [Serializable]
    public class AudioSubtitleStep
    {
        [SerializeField]
        [Tooltip("再生する音声クリップ")]
        private AudioClip audioClip;

        [SerializeField]
        [TextArea(2, 5)]
        [Tooltip("表示する字幕テキスト")]
        private string subtitleText;

        /// <summary>
        /// 音声クリップ
        /// </summary>
        public AudioClip AudioClip
        {
            get => audioClip;
            set => audioClip = value;
        }

        /// <summary>
        /// 字幕テキスト
        /// </summary>
        public string SubtitleText
        {
            get => subtitleText;
            set => subtitleText = value;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AudioSubtitleStep()
        {
            audioClip = null;
            subtitleText = string.Empty;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AudioSubtitleStep(AudioClip clip, string text)
        {
            audioClip = clip;
            subtitleText = text;
        }

        /// <summary>
        /// このステップが有効かどうか
        /// </summary>
        public bool IsValid => audioClip != null;
    }
}
