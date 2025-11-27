using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public TMP_InputField tmp_InputField;

    [SerializeField]
    float NoteSpeed;

    [SerializeField]
    float PerfectTime;
    [SerializeField]
    float GreatTime;
    [SerializeField]
    float MissTime;

    [SerializeField]
    bool IsMirror;

    [SerializeField]
    float LocalOffset;

    [SerializeField]
    KeyCode Lane1Key;
    [SerializeField]
    KeyCode Lane2Key;
    [SerializeField]
    KeyCode Lane3Key;
    [SerializeField]
    KeyCode Lane4Key;

    public void ApplySetting()
    {
        SettingManager.Instance.LoadChartData(tmp_InputField.text);
        SettingManager.Instance.Setting(NoteSpeed, PerfectTime, GreatTime, MissTime, IsMirror, LocalOffset);
        SettingManager.Instance.KeyCongig(Lane1Key, Lane2Key, Lane3Key, Lane4Key);

        Debug.Log("ゲーム設定をセットしました | SongTitle: " + tmp_InputField.text);
    }

}
