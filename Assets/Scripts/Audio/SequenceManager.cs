using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Sequence;

namespace Audio
{
    /// <summary>
    /// シーケンス管理クラス
    /// 複数のマネージャーを管理し、シーケンスインデックスに基づいて並行実行する
    /// </summary>
    public class SequenceManager : MonoBehaviour
    {
        [Header("Sequence Settings")]
        [SerializeField]
        [Tooltip("マネージャーのリスト（AudioManager、SubtitleManager、BGMManager など）")]
        private List<SequenceStep> managers = new List<SequenceStep>();

        [SerializeField]
        [Tooltip("各シーケンス間の待機時間（秒）")]
        private float intervalBetweenSteps = 1f;

        [Header("Manual Control")]
        [SerializeField]
        [Tooltip("現在のシーケンスインデックス（Inspector で変更すると該当インデックスを実行）")]
        private int currentSequenceIndex = 0;

        private SequenceController sequenceController;
        private Coroutine playCoroutine;
        private int previousSequenceIndex = -1;

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
            // Inspector で currentSequenceIndex が変更されたら実行
            if (Application.isPlaying && currentSequenceIndex != previousSequenceIndex)
            {
                if (currentSequenceIndex >= 0)
                {
                    StartCoroutine(ExecuteSequenceIndex(currentSequenceIndex));
                }
                previousSequenceIndex = currentSequenceIndex;
            }
        }

        /// <summary>
        /// SequenceControllerの初期化
        /// </summary>
        private void InitializeController()
        {
            sequenceController = new SequenceController();
            sequenceController.IntervalBetweenSteps = intervalBetweenSteps;
            PrepareManagers();
        }

        /// <summary>
        /// マネージャーを準備
        /// </summary>
        private void PrepareManagers()
        {
            sequenceController.ClearManagers();

            if (managers == null)
                return;

            foreach (var manager in managers)
            {
                if (manager != null)
                {
                    sequenceController.AddManager(manager);
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

            // マネージャーを再準備（Inspector で変更された場合に対応）
            PrepareManagers();

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
            // 全マネージャーの最大要素数を計算
            int maxCount = GetMaxElementCount();
            yield return sequenceController.PlaySequence(maxCount);
            playCoroutine = null;
        }

        /// <summary>
        /// 全マネージャーの中で最大の要素数を取得
        /// </summary>
        private int GetMaxElementCount()
        {
            int maxCount = 0;

            foreach (var manager in managers)
            {
                if (manager == null) continue;

                // 各マネージャーの型に応じて要素数を取得
                if (manager is AudioManager audioManager)
                {
                    maxCount = Mathf.Max(maxCount, audioManager.AudioClips?.Count ?? 0);
                }
                else if (manager is SubtitleManager subtitleManager)
                {
                    maxCount = Mathf.Max(maxCount, subtitleManager.Subtitles?.Count ?? 0);
                }
                else if (manager is BGMManager bgmManager)
                {
                    maxCount = Mathf.Max(maxCount, bgmManager.BGMClips?.Count ?? 0);
                }
            }

            return maxCount;
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
        /// マネージャーを追加（プログラムから動的に追加する場合）
        /// </summary>
        public void AddManager(SequenceStep manager)
        {
            if (manager != null)
            {
                managers.Add(manager);
            }
        }

        /// <summary>
        /// マネージャーをクリア
        /// </summary>
        public void ClearManagers()
        {
            managers.Clear();
            sequenceController?.ClearManagers();
        }

        /// <summary>
        /// 特定のシーケンスインデックスを実行（全マネージャーで該当インデックスを実行）
        /// </summary>
        private IEnumerator ExecuteSequenceIndex(int sequenceIndex)
        {
            if (sequenceIndex < 0)
            {
                Debug.LogWarning($"Sequence index {sequenceIndex} is invalid.");
                yield break;
            }

            yield return sequenceController.ExecuteSequenceIndex(sequenceIndex);
        }
    }
}
