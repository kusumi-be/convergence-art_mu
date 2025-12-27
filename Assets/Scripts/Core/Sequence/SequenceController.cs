using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Sequence
{
    /// <summary>
    /// 汎用的なシーケンス管理クラス
    /// 複数のマネージャーを管理し、シーケンスインデックスに基づいて並行実行する
    /// 具体的な処理内容は知らず、シーケンスの進行のみを管理する
    /// </summary>
    public class SequenceController
    {
        [SerializeField]
        [Tooltip("各ステップ間の待機時間（秒）")]
        private float intervalBetweenSteps = 0f;

        // イベント
        public UnityEvent OnSequenceStart = new UnityEvent();
        public UnityEvent<int> OnStepStart = new UnityEvent<int>();
        public UnityEvent<int> OnStepEnd = new UnityEvent<int>();
        public UnityEvent OnSequenceComplete = new UnityEvent();

        private int currentSequenceIndex = -1;
        private bool isPlaying = false;
        private List<SequenceStep> managers = new List<SequenceStep>();

        /// <summary>
        /// ステップ間の間隔
        /// </summary>
        public float IntervalBetweenSteps
        {
            get => intervalBetweenSteps;
            set => intervalBetweenSteps = Mathf.Max(0f, value);
        }

        /// <summary>
        /// 現在のシーケンスインデックス
        /// </summary>
        public int CurrentSequenceIndex => currentSequenceIndex;

        /// <summary>
        /// 再生中かどうか
        /// </summary>
        public bool IsPlaying => isPlaying;

        /// <summary>
        /// マネージャーの総数
        /// </summary>
        public int ManagerCount => managers.Count;

        /// <summary>
        /// マネージャーのリスト
        /// </summary>
        public List<SequenceStep> Managers
        {
            get => managers;
            set => managers = value ?? new List<SequenceStep>();
        }

        /// <summary>
        /// マネージャーを追加
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
        }

        /// <summary>
        /// シーケンスを開始する（全マネージャーの最大要素数まで実行）
        /// </summary>
        public IEnumerator PlaySequence(int maxSequenceCount)
        {
            if (isPlaying)
            {
                Debug.LogWarning("Sequence is already playing.");
                yield break;
            }

            if (managers == null || managers.Count == 0)
            {
                Debug.LogWarning("No managers in sequence.");
                yield break;
            }

            isPlaying = true;
            currentSequenceIndex = -1;

            OnSequenceStart?.Invoke();

            // シーケンスインデックス 0 から maxSequenceCount - 1 まで実行
            for (int sequenceIndex = 0; sequenceIndex < maxSequenceCount; sequenceIndex++)
            {
                currentSequenceIndex = sequenceIndex;

                OnStepStart?.Invoke(sequenceIndex);

                // 全てのマネージャーを並行実行
                foreach (var manager in managers)
                {
                    if (manager != null)
                    {
                        yield return manager.Execute(sequenceIndex);
                    }
                }

                OnStepEnd?.Invoke(sequenceIndex);

                // 最後のシーケンスでない場合は間隔を待つ
                if (sequenceIndex < maxSequenceCount - 1)
                {
                    yield return new WaitForSeconds(intervalBetweenSteps);
                }
            }

            isPlaying = false;
            currentSequenceIndex = -1;

            OnSequenceComplete?.Invoke();
        }

        /// <summary>
        /// 特定のシーケンスインデックスを実行
        /// </summary>
        public IEnumerator ExecuteSequenceIndex(int sequenceIndex)
        {
            if (managers == null || managers.Count == 0)
            {
                Debug.LogWarning("No managers in sequence.");
                yield break;
            }

            foreach (var manager in managers)
            {
                if (manager != null)
                {
                    yield return manager.Execute(sequenceIndex);
                }
            }
        }

        /// <summary>
        /// シーケンスを停止する
        /// </summary>
        public void Stop()
        {
            isPlaying = false;
            currentSequenceIndex = -1;
        }
    }
}
