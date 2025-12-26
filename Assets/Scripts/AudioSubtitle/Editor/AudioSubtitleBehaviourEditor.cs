using UnityEngine;
using UnityEditor;

namespace AudioSubtitle.Editor
{
    /// <summary>
    /// AudioSubtitleBehaviourのカスタムエディター
    /// Inspector上に再生/停止ボタンを表示する
    /// </summary>
    [CustomEditor(typeof(AudioSubtitleBehaviour))]
    public class AudioSubtitleBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AudioSubtitleBehaviour behaviour = (AudioSubtitleBehaviour)target;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Playback Controls", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            EditorGUILayout.BeginHorizontal();

            // 再生ボタン
            if (GUILayout.Button("Play", GUILayout.Height(30)))
            {
                behaviour.Play();
            }

            // 停止ボタン
            if (GUILayout.Button("Stop", GUILayout.Height(30)))
            {
                behaviour.Stop();
            }

            EditorGUILayout.EndHorizontal();

            // 再生状態の表示
            if (Application.isPlaying)
            {
                EditorGUILayout.Space(5);
                string status = behaviour.IsPlaying ? "Playing..." : "Stopped";
                EditorGUILayout.LabelField("Status:", status);
            }
            else
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.HelpBox("Playback controls are only available in Play Mode.", MessageType.Info);
            }

            EditorGUI.EndDisabledGroup();
        }
    }
}
