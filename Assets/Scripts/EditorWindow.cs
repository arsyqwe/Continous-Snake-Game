using UnityEngine;
using UnityEditor;

public class EditorWindow : UnityEditor.EditorWindow
{
    private SnakeGame snakeGame;

    [MenuItem("Tools/Editor Window")]
    public static void ShowWindow()
    {
        GetWindow<EditorWindow>("EditorWindow");
    }

    public void OnGUI()
    {
        GUILayout.Label("FPS", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("5 FPS")) SetFPS(5);
        if (GUILayout.Button("10 FPS")) SetFPS(10);
        if (GUILayout.Button("15 FPS")) SetFPS(15);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("30 FPS")) SetFPS(30);
        if (GUILayout.Button("60 FPS")) SetFPS(60);
        if (GUILayout.Button("120 FPS")) SetFPS(120);
        GUILayout.EndHorizontal();

        EditorGUILayout.Space(15);


        if (snakeGame == null)
        {
            snakeGame = FindAnyObjectByType<SnakeGame>();
        }

        if (snakeGame != null)
        {
            snakeGame.speed = EditorGUILayout.Slider("Speed", snakeGame.speed, 1f, 30f);
        }

    }

    private void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
        QualitySettings.vSyncCount = 0;
    }
}