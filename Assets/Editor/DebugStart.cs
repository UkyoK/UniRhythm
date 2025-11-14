using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

public class DebugStart
{
    [MenuItem("Debug/Start")]
    private static void DebugSupporter()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/TestSelectScene.unity");
        EditorApplication.EnterPlaymode();
    }

    [MenuItem("Debug/Open TestScene")]
    private static void OpenTestScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/TestScene.unity");
    }

    [MenuItem("Debug/Open TestSelectScene")]
    private static void OpenTestSelectScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/TestSelectScene.unity");
    }
}
