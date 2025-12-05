using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shine.Common;
using TMPro;
using Cysharp.Threading.Tasks;

public class LevelDecide : MonoBehaviour
{
    [SerializeField]
    Difficulty difficulty;

    public void OnClick()
    {
        SettingManager.Instance.SetDefaultSetting();
        SettingManager.Instance.ChartDifficulty = difficulty;
        SettingManager.Instance.LoadChartData(SongSelect.Instance.CenterSongInfo.Title);
        SettingManager.Instance.TopSongID = SongSelect.Instance.TopSong + 1;

        MySoundManager.Instance.PlaySongSelectVoice(SelectState.MusicStart);

        Fade.Instance.FadeOut("InGameScene");
    }

    private void OnEnable()
    {
        TextMeshProUGUI level = transform.Find("Level").gameObject.GetComponent<TextMeshProUGUI>();

        switch(difficulty)
        {
            case Difficulty.Easy:
                level.text = SongSelect.Instance.CenterSongInfo.EasyLevel.ToString();
                break;

            case Difficulty.Normal:
                level.text = SongSelect.Instance.CenterSongInfo.NormalLevel.ToString();
                break;

            case Difficulty.Expert:
                level.text = SongSelect.Instance.CenterSongInfo.ExpertLevel.ToString();
                break;

            case Difficulty.Master:
                level.text = SongSelect.Instance.CenterSongInfo.MasterLevel.ToString();
                break;
        }

    }
}
