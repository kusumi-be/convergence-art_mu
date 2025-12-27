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

        [SerializeField]
        [Tooltip("フェードイン/アウトの時間（秒）")]
        private float fadeDuration = 1f;

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
        /// フェード付きで音声を再生する（BGM用）
        /// 前の音声をフェードアウトしながら、新しい音声をフェードインする
        /// </summary>
        public IEnumerator PlayWithFade(AudioClip clip, bool loop = true)
        {
            if (clip == null)
            {
                Debug.LogWarning("AudioClip is null. Skipping playback.");
                yield break;
            }

            // フェードアウト
            if (audioSource.isPlaying)
            {
                yield return FadeOut(fadeDuration);
            }

            // 新しいクリップをセット
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.volume = 0f;
            audioSource.Play();

            // フェードイン
            yield return FadeIn(fadeDuration);
        }

        /// <summary>
        /// フェードイン
        /// </summary>
        private IEnumerator FadeIn(float duration)
        {
            float currentTime = 0f;
            audioSource.volume = 0f;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0f, 1f, currentTime / duration);
                yield return null;
            }

            audioSource.volume = 1f;
        }

        /// <summary>
        /// フェードアウト
        /// </summary>
        private IEnumerator FadeOut(float duration)
        {
            float startVolume = audioSource.volume;
            float currentTime = 0f;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
                yield return null;
            }

            audioSource.volume = 0f;
            audioSource.Stop();
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
