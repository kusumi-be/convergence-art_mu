using System;
using System.Collections;

namespace Core.Sequence
{
    /// <summary>
    /// シーケンスステップの基底クラス
    /// 任意の処理を実行できる汎用的なステップ
    /// </summary>
    [Serializable]
    public class SequenceStep
    {
        /// <summary>
        /// ステップを実行する
        /// </summary>
        /// <returns>実行完了まで待機するコルーチン</returns>
        public virtual IEnumerator Execute()
        {
            yield break;
        }
    }
}
