using Shine.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    /// <summary>
    /// 譜面速度
    /// </summary>
    public float NoteSpeed {  get; private set; }

    /// <summary>
    /// Perfect判定時間(ms)
    /// </summary>
    public float PerfectTime { get; private set; }
    /// <summary>
    /// Great判定時間(ms)
    /// </summary>
    public float GreatTime { get; private set; }
    /// <summary>
    /// Miss判定時間(ms)
    /// </summary>
    public float MissTime { get; private set; }

    /// <summary>
    /// ミラー設定
    /// </summary>
    public bool IsMirror { get; private set; }

    /// <summary>
    /// 環境オフセット
    /// </summary>
    public float LocalOffset { get; private set; }


    public string Title {  get; private set; }

    public string ArtistName { get; private set; }

    public int StartBPM { get; private set; }

    public float Offset { get; private set; }

    public string FolderName { get; private set; }

    public Difficulty ChartDifficulty { get; private set; }

    public int ChartLevel { get; private set; }

    public KeyCode Lane1 { get; private set; }
    public KeyCode Lane2 { get; private set; }
    public KeyCode Lane3 { get; private set; }
    public KeyCode Lane4 { get; private set; }

    public bool isFindData { get; private set; }
    public void ResetIsFindData() { isFindData = false; }

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

        NoteSpeed = 2;
        PerfectTime = 50;
        GreatTime = 75;
        MissTime = 100;
        IsMirror = false;
        LocalOffset = 0.0f;

        Title = "None";
        ArtistName = "None";
        StartBPM = 0;
        Offset = 0.0f;
        FolderName = "test_music";
        ChartDifficulty = Difficulty.Easy;
        ChartLevel = 0;

        isFindData = false;

        DontDestroyOnLoad(gameObject);
    }

    public void LoadChartData(string songName)
    {
        string path = Application.dataPath + "/StreamingAssets/MusicDatas/music_datas.csv";

        if (!File.Exists(path))
        {
            isFindData = false;
            Debug.LogError("楽曲一覧データが見つかりませんでした");
            return;
        }

        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);

        sr.ReadLine();  // 1行目はスキップ(あとでファイル形式チェック処理にする)

        while (sr.Peek() != -1)
        {
            string line = sr.ReadLine();
            string[] split = line.Split(',');

            // まずタイトルだけ取得
            Title = split[(int)MusicInfo.Title];

            // タイトルが設定されていた曲名と一致していれば処理
            if(Title == songName)
            {
                ArtistName = split[(int)MusicInfo.Artist];
                StartBPM = int.Parse(split[(int)MusicInfo.StartBPM]);
                Offset = float.Parse(split[(int)MusicInfo.Offset]);
                FolderName = split[(int)MusicInfo.FolderName];
                isFindData = true;
                break;
            }
        }

        if (ArtistName == "None")
        {
            isFindData = false;
            Debug.LogError("楽曲データが見つかりませんでした");
            return;
        }

    }

    public void Setting(float noteSpeed, float perfectTime, float greatTime, float missTime, bool isMirror, float localOffset)
    {
        NoteSpeed = noteSpeed;
        PerfectTime = perfectTime;
        GreatTime = greatTime;
        MissTime = missTime;
        IsMirror = isMirror;
        LocalOffset = localOffset;
    }

    public void KeyCongig(KeyCode key1, KeyCode key2, KeyCode key3, KeyCode key4)
    {
        Lane1 = key1;
        Lane2 = key2;
        Lane3 = key3;
        Lane4 = key4;
    }
}
