using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Sequence;

namespace Audio
{
    /// <summary>
    /// シーケンスを再生するMonoBehaviourコンポーネント
    /// 音声と字幕のステップをInspectorで設定し、順次再生する
    /// </summary>
    public class SequencePlayer : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]
        [Tooltip("音声再生コンポーネント")]
        private AudioPlayerComponent audioPlayer;

        [SerializeField]
        [Tooltip("字幕表示コンポーネント")]
        private SubtitleDisplayTMP subtitleDisplay;

        [Header("Sequence Settings")]
        [SerializeField]
        [Tooltip("音声ステップのリスト")]
        private List<AudioSequenceStep> audioSteps = new List<AudioSequenceStep>();

        [SerializeField]
        [Tooltip("字幕ステップのリスト")]
        private List<SubtitleSequenceStep> subtitleSteps = new List<SubtitleSequenceStep>();

        [SerializeField]
        [Tooltip("各ステップ間の待機時間（秒）")]
        private float intervalBetweenSteps = 1f;

        private SequenceController sequenceController;
        private Coroutine playCoroutine;

        /// <summary>
        /// シーケンスコントローラー
        /// </summary>
        public SequenceController Controller => sequenceController;

        /// <summary>
        /// 再生中かどうか
        /// </summary>
        public bool IsPlaying => sequenceController != null && sequenceController.IsPlaying;

        private void Awake()
        {
            ValidateComponents();
            InitializeController();
        }

        /// <summary>
        /// コンポーネントの検証
        /// </summary>
        private void ValidateComponents()
        {
            if (audioPlayer == null)
            {
                audioPlayer = GetComponent<AudioPlayerComponent>();
                if (audioPlayer == null)
                {
                    Debug.LogWarning("AudioPlayerComponent is not assigned. Audio playback will not work.");
                }
            }

            if (subtitleDisplay == null)
            {
                subtitleDisplay = GetComponent<SubtitleDisplayTMP>();
                if (subtitleDisplay == null)
                {
                    Debug.LogWarning("SubtitleDisplayTMP is not assigned. Subtitle display will not work.");
                }
            }
        }

        /// <summary>
        /// SequenceControllerの初期化
        /// </summary>
        private void InitializeController()
        {
            sequenceController = new SequenceController();
            sequenceController.IntervalBetweenSteps = intervalBetweenSteps;
            BuildSequence();
        }

        /// <summary>
        /// シーケンスを構築
        /// 音声と字幕のステップを組み合わせてシーケンスを作成
        /// </summary>
        private void BuildSequence()
        {
            sequenceController.ClearSteps();

            // 音声ステップにプレイヤーを設定して追加
            foreach (var audioStep in audioSteps)
            {
                if (audioStep != null)
                {
                    audioStep.SetAudioPlayer(audioPlayer);
                    sequenceController.AddStep(audioStep);
                }
            }

            // 字幕ステップにディスプレイを設定して追加
            foreach (var subtitleStep in subtitleSteps)
            {
                if (subtitleStep != null)
                {
                    subtitleStep.SetSubtitleDisplay(subtitleDisplay);
                    sequenceController.AddStep(subtitleStep);
                }
            }
        }

        /// <summary>
        /// シーケンスを再生
        /// </summary>
        public void Play()
        {
            if (playCoroutine != null)
            {
                Debug.LogWarning("Sequence is already playing.");
                return;
            }

            // シーケンスを再構築（Inspector で変更された場合に対応）
            BuildSequence();

            playCoroutine = StartCoroutine(PlaySequenceCoroutine());
        }

        /// <summary>
        /// シーケンスを停止
        /// </summary>
        public void Stop()
        {
            if (playCoroutine != null)
            {
                StopCoroutine(playCoroutine);
                playCoroutine = null;
            }

            sequenceController?.Stop();
            audioPlayer?.Stop();
            subtitleDisplay?.HideSubtitle();
        }

        /// <summary>
        /// シーケンス再生のコルーチン
        /// </summary>
        private IEnumerator PlaySequenceCoroutine()
        {
            yield return sequenceController.PlaySequence();
            playCoroutine = null;
        }

        /// <summary>
        /// ステップ間の間隔を設定
        /// </summary>
        public void SetInterval(float interval)
        {
            intervalBetweenSteps = interval;
            if (sequenceController != null)
            {
                sequenceController.IntervalBetweenSteps = interval;
            }
        }
    }
}
