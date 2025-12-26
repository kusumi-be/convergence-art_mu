using System.Collections;

namespace Core.Sequence
{
    /// <summary>
    /// シーケンスステップの基底インターフェース
    /// 任意の処理を実行できる汎用的なステップ
    /// </summary>
    public interface ISequenceStep
    {
        /// <summary>
        /// ステップを実行する
        /// </summary>
        /// <returns>実行完了まで待機するコルーチン</returns>
        IEnumerator Execute();
    }
}
