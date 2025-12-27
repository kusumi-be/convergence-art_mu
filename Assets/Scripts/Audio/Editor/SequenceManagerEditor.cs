using UnityEngine;
using UnityEditor;

namespace Audio.Editor
{
    /// <summary>
    /// SequenceManagerのカスタムエディター
    /// Inspector上に再生/停止ボタンを表示する
    /// </summary>
    [CustomEditor(typeof(SequenceManager))]
    public class SequenceManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SequenceManager manager = (SequenceManager)target;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Playback Controls", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            EditorGUILayout.BeginHorizontal();

            // 再生ボタン
            if (GUILayout.Button("Play", GUILayout.Height(30)))
            {
                manager.Play();
            }

            // 停止ボタン
            if (GUILayout.Button("Stop", GUILayout.Height(30)))
            {
                manager.Stop();
            }

            EditorGUILayout.EndHorizontal();

            // 再生状態の表示
            if (Application.isPlaying)
            {
                EditorGUILayout.Space(5);
                string status = manager.IsPlaying ? "Playing..." : "Stopped";
                EditorGUILayout.LabelField("Status:", status);

                if (manager.Controller != null)
                {
                    EditorGUILayout.LabelField("Current Step:", $"{manager.Controller.CurrentIndex + 1} / {manager.Controller.Count}");
                }
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
