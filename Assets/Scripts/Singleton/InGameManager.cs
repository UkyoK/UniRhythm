using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

    /// <summary>
    /// 楽曲終了時間
    /// </summary>
    public float EndTime { get; private set; }
    public void SetEndTime(float endTime)
    {
        EndTime = endTime;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        EndTime = Time.time + 10000;
    }

    private async void Start()
    {
        Fade.Instance.FadeIn();
        await UniTask.Delay(TimeSpan.FromSeconds(Fade.Instance.FadeTime + ChartLoader.Instance.WaitTime));
        MySoundManager.Instance.PlayMusic();

        // 譜面データが見つからなかった場合、曲を止めて選曲画面に戻る
        if (!ChartLoader.Instance.isFindData)
        {
            MySoundManager.Instance.StopMusic();
            Debug.LogError("譜面データがセットされていません\n選曲画面に戻ります");
            SceneManager.LoadScene("SongSelectScene");
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {

        // 曲が終わったらリザルトシーンに進む
        if (Time.time > EndTime)
        {
            MySoundManager.Instance.StopMusic();
            Fade.Instance.FadeOut("ResultScene");
            Destroy(gameObject);
        }
    }

}
