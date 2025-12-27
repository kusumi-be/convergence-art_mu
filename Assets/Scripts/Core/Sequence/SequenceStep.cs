using System.Collections;
using UnityEngine;

namespace Core.Sequence
{
    /// <summary>
    /// シーケンスステップの基底クラス
    /// 任意の処理を実行できる汎用的なステップ
    /// </summary>
    public class SequenceStep : MonoBehaviour
    {
        /// <summary>
        /// ステップを実行する
        /// </summary>
        /// <param name="index">実行する要素のインデックス</param>
        /// <returns>実行完了まで待機するコルーチン</returns>
        public virtual IEnumerator Execute(int index)
        {
            yield break;
        }
    }
}
