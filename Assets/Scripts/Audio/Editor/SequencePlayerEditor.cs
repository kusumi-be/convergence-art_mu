using UnityEngine;
using UnityEditor;

namespace Audio.Editor
{
    /// <summary>
    /// SequencePlayerのカスタムエディター
    /// Inspector上に再生/停止ボタンを表示する
    /// </summary>
    [CustomEditor(typeof(SequencePlayer))]
    public class SequencePlayerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SequencePlayer player = (SequencePlayer)target;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Playback Controls", EditorStyles.boldLabel);

            EditorGUI.BeginDisabledGroup(!Application.isPlaying);

            EditorGUILayout.BeginHorizontal();

            // 再生ボタン
            if (GUILayout.Button("Play", GUILayout.Height(30)))
            {
                player.Play();
            }

            // 停止ボタン
            if (GUILayout.Button("Stop", GUILayout.Height(30)))
            {
                player.Stop();
            }

            EditorGUILayout.EndHorizontal();

            // 再生状態の表示
            if (Application.isPlaying)
            {
                EditorGUILayout.Space(5);
                string status = player.IsPlaying ? "Playing..." : "Stopped";
                EditorGUILayout.LabelField("Status:", status);

                if (player.Controller != null)
                {
                    EditorGUILayout.LabelField("Current Step:", $"{player.Controller.CurrentIndex + 1} / {player.Controller.Count}");
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
