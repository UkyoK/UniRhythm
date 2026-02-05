using Shine.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MirrorChange : MonoBehaviour
{
    Toggle toggle;
    void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    void Update()
    {
        // 難易度選曲状態でないのなら、処理を行わない
        if (SongSelect.Instance.SongSelectState != SelectState.LevelSelect)
        {
            return;
        }

        // [M]キーで切り替え
        if (Input.GetKeyDown(KeyCode.M))
        {
            toggle.isOn = !toggle.isOn;
        }
    }

    public void OnToggleChanged(bool isMirror)
    {
        SettingManager.Instance.IsMirror = isMirror;
    }
}
