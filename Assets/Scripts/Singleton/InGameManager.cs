using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

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
        await UniTask.Delay(TimeSpan.FromSeconds(Fade.Instance.FadeTime));
        MySoundManager.Instance.PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        // 譜面データが見つからなかった場合、曲を止めて選曲画面に戻る
        if (!ChartLoader.Instance.isFindData)
        {
            MySoundManager.Instance.StopMusic();
            Debug.LogError("譜面データがセットされていません\n選曲画面に戻ります");
            SceneManager.LoadScene("SongSelectScene");
            Destroy(gameObject);
            return;
        }

        // 曲が終わったらリザルトシーンに進む
        if (Time.time > EndTime)
        {
            MySoundManager.Instance.MusicPlayStop();
            Fade.Instance.FadeOut("ResultScene");
            Destroy(gameObject);
        }
    }

}
