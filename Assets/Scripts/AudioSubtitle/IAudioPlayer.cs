using System.Collections;
using UnityEngine;

namespace AudioSubtitle
{
    /// <summary>
    /// 音声再生のインターフェース
    /// </summary>
    public interface IAudioPlayer
    {
        /// <summary>
        /// 音声を再生する
        /// </summary>
        /// <param name="clip">再生する音声クリップ</param>
        /// <returns>再生完了まで待機するコルーチン</returns>
        IEnumerator Play(AudioClip clip);

        /// <summary>
        /// 音声を停止する
        /// </summary>
        void Stop();

        /// <summary>
        /// 再生中かどうか
        /// </summary>
        bool IsPlaying { get; }
    }
}
