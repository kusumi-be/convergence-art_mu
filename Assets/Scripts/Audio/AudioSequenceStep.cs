using System;
using System.Collections;
using UnityEngine;
using Core.Sequence;

namespace Audio
{
    /// <summary>
    /// 音声再生専用のシーケンスステップ
    /// </summary>
    [Serializable]
    public class AudioSequenceStep : ISequenceStep
    {
        [SerializeField]
        [Tooltip("再生する音声クリップ")]
        private AudioClip audioClip;

        private IAudioPlayer audioPlayer;

        /// <summary>
        /// 音声クリップ
        /// </summary>
        public AudioClip AudioClip
        {
            get => audioClip;
            set => audioClip = value;
        }

        /// <summary>
        /// 音声プレイヤーを設定
        /// </summary>
        public void SetAudioPlayer(IAudioPlayer player)
        {
            audioPlayer = player;
        }

        /// <summary>
        /// ステップを実行（音声を再生）
        /// </summary>
        public IEnumerator Execute()
        {
            if (audioClip == null)
            {
                Debug.LogWarning("AudioClip is null. Skipping audio playback.");
                yield break;
            }

            if (audioPlayer == null)
            {
                Debug.LogError("AudioPlayer is not set. Cannot play audio.");
                yield break;
            }

            yield return audioPlayer.Play(audioClip);
        }

        /// <summary>
        /// このステップが有効かどうか
        /// </summary>
        public bool IsValid => audioClip != null;
    }
}
