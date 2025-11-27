using UnityEditor.SceneManagement;
using UnityEditor;

public class DebugStart
{
    [MenuItem("UniRhythm/Debug Start", priority = 1)]
    private static void DebugSupporter()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/TestSelectScene.unity");
        EditorApplication.EnterPlaymode();
    }

    [MenuItem("UniRhythm/Scene/InGameScene", priority = 103)]
    private static void OpenTestScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/InGame/InGameScene.unity");
    }

    [MenuItem("UniRhythm/Scene/TestSelectScene", priority = 101)]
    private static void OpenTestSelectScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/TestSelectScene.unity");
    }

    [MenuItem("UniRhythm/Scene/ResultScene", priority = 104)]
    private static void OpenResultScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/OutGame/ResultScene.unity");
    }

    [MenuItem("UniRhythm/Scene/SongSelectScene", priority = 102)]
    private static void OpenSongSelectScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/OutGame/SongSelectScene.unity");
    }

}
