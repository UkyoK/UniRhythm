using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (SettingManager.Instance.isFindData)
        {
            SceneManager.LoadScene(sceneName);
            SettingManager.Instance.ResetIsFindData();
        }
        else
        {
            Debug.LogWarning("楽曲データがセットされていません");
        }
    }
}
