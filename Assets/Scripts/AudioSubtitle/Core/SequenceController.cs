using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AudioSubtitle.Core
{
    /// <summary>
    /// 汎用的なシーケンス管理クラス
    /// 任意のデータ型のリストを順次実行し、イベントを発火する
    /// </summary>
    /// <typeparam name="T">シーケンスで扱うデータの型</typeparam>
    [Serializable]
    public class SequenceController<T>
    {
        [SerializeField]
        private List<T> items = new List<T>();

        [SerializeField]
        [Tooltip("各ステップ間の待機時間（秒）")]
        private float intervalBetweenSteps = 1f;

        // イベント
        public UnityEvent OnSequenceStart = new UnityEvent();
        public UnityEvent<int> OnStepStart = new UnityEvent<int>();
        public UnityEvent<int> OnStepEnd = new UnityEvent<int>();
        public UnityEvent OnSequenceComplete = new UnityEvent();

        private int currentIndex = -1;
        private bool isPlaying = false;

        /// <summary>
        /// アイテムのリスト（Inspector編集用）
        /// </summary>
        public List<T> Items
        {
            get => items;
            set => items = value;
        }

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
        /// アイテムの総数
        /// </summary>
        public int Count => items.Count;

        /// <summary>
        /// シーケンスを開始する
        /// </summary>
        /// <param name="monoBehaviour">コルーチンを実行するMonoBehaviour</param>
        /// <param name="stepAction">各ステップで実行するアクション</param>
        /// <returns>コルーチン</returns>
        public IEnumerator PlaySequence(MonoBehaviour monoBehaviour, Func<T, IEnumerator> stepAction)
        {
            if (isPlaying)
            {
                Debug.LogWarning("Sequence is already playing.");
                yield break;
            }

            if (items == null || items.Count == 0)
            {
                Debug.LogWarning("No items in sequence.");
                yield break;
            }

            isPlaying = true;
            currentIndex = -1;

            OnSequenceStart?.Invoke();

            for (int i = 0; i < items.Count; i++)
            {
                currentIndex = i;

                OnStepStart?.Invoke(i);

                // 各ステップのアクションを実行
                yield return monoBehaviour.StartCoroutine(stepAction(items[i]));

                OnStepEnd?.Invoke(i);

                // 最後のステップでない場合は間隔を待つ
                if (i < items.Count - 1)
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
        /// 特定のインデックスのアイテムを取得
        /// </summary>
        public T GetItem(int index)
        {
            if (index < 0 || index >= items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            return items[index];
        }
    }
}
