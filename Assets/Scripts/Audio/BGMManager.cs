using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Sequence;

namespace Audio
{
    /// <summary>
    /// BGM管理クラス
    /// 複数のBGMを管理し、シーケンスに従って順次再生する
    /// フェードイン/アウト機能を使用してスムーズに切り替える
    /// </summary>
    public class BGMManager : SequenceStep
    {
        [SerializeField]
        [Tooltip("再生するBGMクリップのリスト")]
        private List<AudioClip> bgmClips = new List<AudioClip>();

        [SerializeField]
        [Tooltip("BGM再生コンポーネント")]
        private AudioPlayer bgmPlayer;

        /// <summary>
        /// BGMクリップのリスト
        /// </summary>
        public List<AudioClip> BGMClips
        {
            get => bgmClips;
            set => bgmClips = value;
        }

        /// <summary>
        /// ステップを実行（指定されたインデックスのBGMをフェード付きで再生）
        /// </summary>
        /// <param name="index">再生するBGMクリップのインデックス</param>
        public override IEnumerator Execute(int index)
        {
            if (bgmClips == null || index < 0 || index >= bgmClips.Count)
            {
                Debug.LogWarning($"BGMManager: Index {index} is out of range (Count: {bgmClips?.Count ?? 0}).");
                yield break;
            }

            if (bgmPlayer == null)
            {
                Debug.LogError("BGMPlayer is not assigned in BGMManager.");
                yield break;
            }

            AudioClip clip = bgmClips[index];
            if (clip != null)
            {
                // フェード付きで再生（ループあり）
                yield return bgmPlayer.PlayWithFade(clip, loop: true);
            }
        }
    }
}
