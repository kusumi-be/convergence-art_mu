using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Sequence;

namespace Audio
{
    /// <summary>
    /// シーケンスを再生するMonoBehaviourコンポーネント
    /// ISequenceStepのリストを管理し、順次再生する
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
        [SerializeReference]
        [Tooltip("シーケンスステップのリスト（ISequenceStep）")]
        private List<ISequenceStep> steps = new List<ISequenceStep>();

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
            PrepareSteps();
        }

        /// <summary>
        /// ステップを準備（各ステップに必要なコンポーネントを設定）
        /// </summary>
        private void PrepareSteps()
        {
            sequenceController.ClearSteps();

            if (steps == null)
                return;

            foreach (var step in steps)
            {
                if (step == null)
                    continue;

                // 各ステップの型に応じて必要なコンポーネントを設定
                if (step is AudioSequenceStep audioStep)
                {
                    audioStep.SetAudioPlayer(audioPlayer);
                }
                else if (step is SubtitleSequenceStep subtitleStep)
                {
                    subtitleStep.SetSubtitleDisplay(subtitleDisplay);
                }

                sequenceController.AddStep(step);
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

            // ステップを再準備（Inspector で変更された場合に対応）
            PrepareSteps();

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

        /// <summary>
        /// ステップを追加（プログラムから動的に追加する場合）
        /// </summary>
        public void AddStep(ISequenceStep step)
        {
            if (step != null)
            {
                steps.Add(step);
            }
        }

        /// <summary>
        /// ステップをクリア
        /// </summary>
        public void ClearSteps()
        {
            steps.Clear();
            sequenceController?.ClearSteps();
        }
    }
}
