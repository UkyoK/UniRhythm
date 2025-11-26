using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string _TitleScene = "TestSelectScene";

    public void LoadScene(string sceneName)
    {
        if (SettingManager.Instance.isFindData && sceneName == "SongSelectScene")
        {
            Fade.Instance.FadeOut(sceneName);
        }
        else if (sceneName == _TitleScene)
        {
            Debug.LogWarning("楽曲データがセットされていません");
        }
        else
        {
            Fade.Instance.FadeOut(sceneName);
        }
    }

    public void LoadTitleScene()
    {
        SettingManager.Instance.ResetIsFindData();
        SceneManager.LoadScene(_TitleScene);
    }
}
