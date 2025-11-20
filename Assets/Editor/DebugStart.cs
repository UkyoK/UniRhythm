using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

public class DebugStart
{
    [MenuItem("Debug/Debug Start")]
    private static void DebugSupporter()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/TestSelectScene.unity");
        EditorApplication.EnterPlaymode();
    }

    [MenuItem("Debug/Scene/TestScene")]
    private static void OpenTestScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/TestScene.unity");
    }

    [MenuItem("Debug/Scene/TestSelectScene")]
    private static void OpenTestSelectScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/TestSelectScene.unity");
    }

    [MenuItem("Debug/Scene/ResultScene")]
    private static void OpenResultScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/OutGame/ResultScene.unity");
    }

    [MenuItem("Debug/Scene/SongSelectScene")]
    private static void OpenSongSelectScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/OutGame/SongSelectScene.unity");
    }

}
