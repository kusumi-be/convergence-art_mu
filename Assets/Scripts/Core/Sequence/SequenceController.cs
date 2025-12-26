using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Sequence
{
    /// <summary>
    /// 汎用的なシーケンス管理クラス
    /// ISequenceStepのリストを順次実行し、イベントを発火する
    /// 具体的な処理内容は知らず、ステップの進行のみを管理する
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

        private int currentIndex = -1;
        private bool isPlaying = false;
        private List<ISequenceStep> steps = new List<ISequenceStep>();

        /// <summary>
        /// ステップ間の間隔
        /// </summary>
        public float IntervalBetweenSteps
        {
            get => intervalBetweenSteps;
            set => intervalBetweenSteps = Mathf.Max(0f, value);
        }

        /// <summary>
        /// 現在のインデックス
        /// </summary>
        public int CurrentIndex => currentIndex;

        /// <summary>
        /// 再生中かどうか
        /// </summary>
        public bool IsPlaying => isPlaying;

        /// <summary>
        /// ステップの総数
        /// </summary>
        public int Count => steps.Count;

        /// <summary>
        /// ステップのリスト
        /// </summary>
        public List<ISequenceStep> Steps
        {
            get => steps;
            set => steps = value ?? new List<ISequenceStep>();
        }

        /// <summary>
        /// ステップを追加
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
        }

        /// <summary>
        /// シーケンスを開始する
        /// </summary>
        public IEnumerator PlaySequence()
        {
            if (isPlaying)
            {
                Debug.LogWarning("Sequence is already playing.");
                yield break;
            }

            if (steps == null || steps.Count == 0)
            {
                Debug.LogWarning("No steps in sequence.");
                yield break;
            }

            isPlaying = true;
            currentIndex = -1;

            OnSequenceStart?.Invoke();

            for (int i = 0; i < steps.Count; i++)
            {
                currentIndex = i;

                OnStepStart?.Invoke(i);

                // 各ステップを実行（具体的な処理は知らない）
                yield return steps[i].Execute();

                OnStepEnd?.Invoke(i);

                // 最後のステップでない場合は間隔を待つ
                if (i < steps.Count - 1)
                {
                    yield return new WaitForSeconds(intervalBetweenSteps);
                }
            }

            isPlaying = false;
            currentIndex = -1;

            OnSequenceComplete?.Invoke();
        }

        /// <summary>
        /// シーケンスを停止する
        /// </summary>
        public void Stop()
        {
            isPlaying = false;
            currentIndex = -1;
        }

        /// <summary>
        /// 特定のインデックスのステップを取得
        /// </summary>
        public ISequenceStep GetStep(int index)
        {
            if (index < 0 || index >= steps.Count)
            {
                return null;
            }
            return steps[index];
        }
    }
}
