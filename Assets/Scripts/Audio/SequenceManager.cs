using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Sequence;

namespace Audio
{
    /// <summary>
    /// シーケンス管理クラス
    /// SequenceStepのリストを管理し、順次実行する
    /// </summary>
    public class SequenceManager : MonoBehaviour
    {
        [Header("Sequence Settings")]
        [SerializeField]
        [Tooltip("オーディオ管理クラス")]
        private AudioManager audioManager;

        [SerializeField]
        [Tooltip("字幕管理クラス")]
        private SubtitleManager subtitleManager;

        [SerializeField]
        [Tooltip("各ステップ間の待機時間（秒）")]
        private float intervalBetweenSteps = 1f;

        [Header("Manual Control")]
        [SerializeField]
        [Tooltip("現在のステップインデックス（Inspector で変更すると該当ステップを実行）")]
        private int currentStepIndex = 0;

        private SequenceController sequenceController;
        private Coroutine playCoroutine;
        private int previousStepIndex = -1;

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
            InitializeController();
        }

        private void OnValidate()
        {
            // Inspector で currentStepIndex が変更されたら実行
            if (Application.isPlaying && currentStepIndex != previousStepIndex)
            {
                if (sequenceController != null && currentStepIndex >= 0 && currentStepIndex < sequenceController.Count)
                {
                    StartCoroutine(ExecuteStep(currentStepIndex));
                }
                previousStepIndex = currentStepIndex;
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
        /// ステップを準備
        /// AudioManager と SubtitleManager を交互に追加してシーケンスを構築
        /// </summary>
        private void PrepareSteps()
        {
            sequenceController.ClearSteps();

            if (audioManager == null || subtitleManager == null)
            {
                Debug.LogWarning("AudioManager or SubtitleManager is not assigned.");
                return;
            }

            // オーディオと字幕のペア数を決定
            int audioCount = audioManager.AudioClips?.Count ?? 0;
            int subtitleCount = subtitleManager.Subtitles?.Count ?? 0;
            int pairCount = Mathf.Max(audioCount, subtitleCount);

            // 交互に追加（オーディオ → 字幕 → オーディオ → 字幕...）
            for (int i = 0; i < pairCount; i++)
            {
                if (i < audioCount)
                {
                    sequenceController.AddStep(audioManager);
                }
                if (i < subtitleCount)
                {
                    sequenceController.AddStep(subtitleManager);
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
        /// AudioManager を取得
        /// </summary>
        public AudioManager GetAudioManager()
        {
            return audioManager;
        }

        /// <summary>
        /// SubtitleManager を取得
        /// </summary>
        public SubtitleManager GetSubtitleManager()
        {
            return subtitleManager;
        }

        /// <summary>
        /// AudioManager を設定
        /// </summary>
        public void SetAudioManager(AudioManager manager)
        {
            audioManager = manager;
        }

        /// <summary>
        /// SubtitleManager を設定
        /// </summary>
        public void SetSubtitleManager(SubtitleManager manager)
        {
            subtitleManager = manager;
        }

        /// <summary>
        /// 特定のステップを実行
        /// </summary>
        private IEnumerator ExecuteStep(int index)
        {
            if (sequenceController == null || index < 0 || index >= sequenceController.Count)
            {
                Debug.LogWarning($"Step index {index} is out of range.");
                yield break;
            }

            SequenceStep step = sequenceController.GetStep(index);
            if (step != null)
            {
                yield return step.Execute();
            }
        }
    }
}
