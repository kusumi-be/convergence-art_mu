using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using AudioSubtitle.Core;

namespace AudioSubtitle
{
    /// <summary>
    /// 音声と字幕の再生を管理するメインコンポーネント
    /// </summary>
    public class AudioSubtitleBehaviour : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("音声と字幕のシーケンス")]
        private SequenceController<AudioSubtitleStep> sequenceController = new SequenceController<AudioSubtitleStep>();

        [Header("Components")]
        [SerializeField]
        [Tooltip("音声再生コンポーネント")]
        private AudioPlayerComponent audioPlayer;

        [SerializeField]
        [Tooltip("字幕表示コンポーネント")]
        private SubtitleDisplayTMP subtitleDisplay;

        [Header("Events")]
        [SerializeField]
        [Tooltip("シーケンス開始時のイベント")]
        private UnityEvent onSequenceStart = new UnityEvent();

        [SerializeField]
        [Tooltip("各ステップ開始時のイベント（インデックスが渡される）")]
        private UnityEvent<int> onStepStart = new UnityEvent<int>();

        [SerializeField]
        [Tooltip("各ステップ終了時のイベント（インデックスが渡される）")]
        private UnityEvent<int> onStepEnd = new UnityEvent<int>();

        [SerializeField]
        [Tooltip("シーケンス完了時のイベント")]
        private UnityEvent onSequenceComplete = new UnityEvent();

        private Coroutine playCoroutine;

        /// <summary>
        /// シーケンスコントローラー（Inspector編集用）
        /// </summary>
        public SequenceController<AudioSubtitleStep> SequenceController => sequenceController;

        /// <summary>
        /// 再生中かどうか
        /// </summary>
        public bool IsPlaying => sequenceController.IsPlaying;

        private void Awake()
        {
            ValidateComponents();
            SetupEvents();
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
                    Debug.LogError("AudioPlayerComponent is not assigned and not found on this GameObject.");
                }
            }

            if (subtitleDisplay == null)
            {
                subtitleDisplay = GetComponent<SubtitleDisplayTMP>();
                if (subtitleDisplay == null)
                {
                    Debug.LogError("SubtitleDisplayTMP is not assigned and not found on this GameObject.");
                }
            }
        }

        /// <summary>
        /// イベントのセットアップ
        /// </summary>
        private void SetupEvents()
        {
            sequenceController.OnSequenceStart.AddListener(() => onSequenceStart?.Invoke());
            sequenceController.OnStepStart.AddListener((index) => onStepStart?.Invoke(index));
            sequenceController.OnStepEnd.AddListener((index) => onStepEnd?.Invoke(index));
            sequenceController.OnSequenceComplete.AddListener(() => onSequenceComplete?.Invoke());
        }

        /// <summary>
        /// シーケンスを再生する（Inspector/Editorから呼び出し可能）
        /// </summary>
        public void Play()
        {
            if (playCoroutine != null)
            {
                Debug.LogWarning("Sequence is already playing.");
                return;
            }

            playCoroutine = StartCoroutine(PlaySequenceCoroutine());
        }

        /// <summary>
        /// シーケンスを停止する
        /// </summary>
        public void Stop()
        {
            if (playCoroutine != null)
            {
                StopCoroutine(playCoroutine);
                playCoroutine = null;
            }

            sequenceController.Stop();
            audioPlayer?.Stop();
            subtitleDisplay?.HideSubtitle();
        }

        /// <summary>
        /// シーケンス再生のコルーチン
        /// </summary>
        private IEnumerator PlaySequenceCoroutine()
        {
            yield return sequenceController.PlaySequence(this, ProcessStep);
            playCoroutine = null;
        }

        /// <summary>
        /// 各ステップの処理
        /// </summary>
        private IEnumerator ProcessStep(AudioSubtitleStep step)
        {
            if (step == null || !step.IsValid)
            {
                Debug.LogWarning("Invalid step. Skipping.");
                yield break;
            }

            // 字幕を表示
            if (subtitleDisplay != null)
            {
                subtitleDisplay.ShowSubtitle(step.SubtitleText);
            }

            // 音声を再生（完了まで待機）
            if (audioPlayer != null && step.AudioClip != null)
            {
                yield return audioPlayer.Play(step.AudioClip);
            }

            // 字幕を非表示（オプション: コメントアウトすれば字幕は残る）
            // if (subtitleDisplay != null)
            // {
            //     subtitleDisplay.HideSubtitle();
            // }
        }

        private void OnDestroy()
        {
            // イベントリスナーのクリーンアップ
            sequenceController.OnSequenceStart.RemoveAllListeners();
            sequenceController.OnStepStart.RemoveAllListeners();
            sequenceController.OnStepEnd.RemoveAllListeners();
            sequenceController.OnSequenceComplete.RemoveAllListeners();
        }
    }
}
