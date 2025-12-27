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
        [SerializeReference]
        [Tooltip("シーケンスステップのリスト（各管理クラス）")]
        private List<SequenceStep> steps = new List<SequenceStep>();

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
            InitializeController();
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
        /// </summary>
        private void PrepareSteps()
        {
            sequenceController.ClearSteps();

            if (steps == null)
                return;

            foreach (var step in steps)
            {
                if (step != null)
                {
                    sequenceController.AddStep(step);
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
        /// ステップを追加（プログラムから動的に追加する場合）
        /// </summary>
        public void AddStep(SequenceStep step)
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
