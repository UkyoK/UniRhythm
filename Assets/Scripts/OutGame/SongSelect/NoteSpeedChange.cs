using Shine.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteSpeedChange : MonoBehaviour
{
    [SerializeField]
    float MaxSpeed;

    [SerializeField]
    float MinSpeed;

    TextMeshProUGUI tmPro;

    void Awake()
    {
        tmPro = GetComponent<TextMeshProUGUI>();
        tmPro.text = SettingManager.Instance.NoteSpeed.ToString("F1");
    }

    void Update()
    {
        // 難易度選曲状態でないのなら、処理を行わない
        if (SongSelect.Instance.SongSelectState != SelectState.LevelSelect)
        {
            return;
        }

        // [↑]キーで0.5速く
        if (Input.GetKeyDown(KeyCode.UpArrow) && SettingManager.Instance.NoteSpeed < MaxSpeed)
        {
            SettingManager.Instance.NoteSpeed += 0.5f;
            tmPro.text = SettingManager.Instance.NoteSpeed.ToString("F1");
        }
        // [↓]キーで0.5遅く
        if (Input.GetKeyDown(KeyCode.DownArrow) && SettingManager.Instance.NoteSpeed > MinSpeed)
        {
            SettingManager.Instance.NoteSpeed -= 0.5f;
            tmPro.text = SettingManager.Instance.NoteSpeed.ToString("F1");
        }
    }
}
