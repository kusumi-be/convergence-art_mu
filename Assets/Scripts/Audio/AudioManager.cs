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
        /// ステップを実行（指定されたインデックスの音声を再生）
        /// </summary>
        /// <param name="index">再生する音声クリップのインデックス</param>
        public override IEnumerator Execute(int index)
        {
            if (audioClips == null || index < 0 || index >= audioClips.Count)
            {
                Debug.LogWarning($"AudioManager: Index {index} is out of range (Count: {audioClips?.Count ?? 0}).");
                yield break;
            }

            if (audioPlayer == null)
            {
                Debug.LogError("AudioPlayer is not assigned in AudioManager.");
                yield break;
            }

            AudioClip clip = audioClips[index];
            if (clip != null)
            {
                yield return audioPlayer.Play(clip);
            }
        }
    }
}
