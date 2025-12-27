using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Sequence;

namespace Audio
{
    /// <summary>
    /// 音声管理クラス
    /// 複数の音声を管理し、シーケンスに従って順次再生する
    /// </summary>
    [Serializable]
    public class AudioManager : SequenceStep
    {
        [SerializeField]
        [Tooltip("再生する音声クリップのリスト")]
        private List<AudioClip> audioClips = new List<AudioClip>();

        [SerializeField]
        [Tooltip("音声再生コンポーネント")]
        private AudioPlayer audioPlayer;

        private int currentIndex = 0;

        /// <summary>
        /// 音声クリップのリスト
        /// </summary>
        public List<AudioClip> AudioClips
        {
            get => audioClips;
            set => audioClips = value;
        }

        /// <summary>
        /// ステップを実行（現在のインデックスの音声を再生）
        /// </summary>
        public override IEnumerator Execute()
        {
            if (audioClips == null || audioClips.Count == 0)
            {
                Debug.LogWarning("No audio clips in AudioManager.");
                yield break;
            }

            if (currentIndex >= audioClips.Count)
            {
                Debug.LogWarning("AudioManager: Current index exceeds audio clips count.");
                yield break;
            }

            if (audioPlayer == null)
            {
                Debug.LogError("AudioPlayer is not assigned in AudioManager.");
                yield break;
            }

            AudioClip clip = audioClips[currentIndex];
            if (clip != null)
            {
                yield return audioPlayer.Play(clip);
            }

            currentIndex++;
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
