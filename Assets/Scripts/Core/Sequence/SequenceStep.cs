using System;
using System.Collections;

namespace Core.Sequence
{
    /// <summary>
    /// シーケンスステップの基底抽象クラス
    /// 任意の処理を実行できる汎用的なステップ
    /// </summary>
    [Serializable]
    public abstract class SequenceStep
    {
        /// <summary>
        /// ステップを実行する
        /// </summary>
        /// <returns>実行完了まで待機するコルーチン</returns>
        public abstract IEnumerator Execute();
    }
}
