using System.Collections;
using UnityEngine;

namespace Audio
{
    /// <summary>
    /// AudioSourceを使用した音声再生コンポーネント
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        /// <summary>
        /// 音声を再生し、完了まで待機する
        /// </summary>
        public IEnumerator Play(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("AudioClip is null. Skipping playback.");
                yield break;
            }

            audioSource.clip = clip;
            audioSource.Play();

            // 音声の長さ分待機
            yield return new WaitForSeconds(clip.length);
        }

        /// <summary>
        /// 音声を停止する
        /// </summary>
        public void Stop()
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        /// <summary>
        /// 再生中かどうか
        /// </summary>
        public bool IsPlaying => audioSource != null && audioSource.isPlaying;
    }
}
