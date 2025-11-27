using Shine.Common;
using Shine.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SongInfoLoader : MonoBehaviour
{
    public static SongInfoLoader Instance;

    public List<SongInfo> SongInfoList {  get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SongInfoList = new List<SongInfo>();

        LoadMusicInfoData();
    }

    /// <summary>
    /// 曲一覧データ読み込み
    /// </summary>
    void LoadMusicInfoData()
    {
        string path = Application.dataPath + "/StreamingAssets/MusicDatas/music_datas.json";

        if (!File.Exists(path))
        {
            Debug.LogError("楽曲一覧データが見つかりませんでした");
            return;
        }

        string json = File.ReadAllText(path);
        Data data = JsonUtility.FromJson<Data>(json);

        // ゲーム内で使えるデータに変換
        foreach (MusicData musicData in data.MusicDatas)
        {
            SongInfo info = new SongInfo();
            info.Title = musicData.Title;
            info.Artist = musicData.Artist;
            info.StartBPM = float.Parse(musicData.StartBPM);
            info.Offset = float.Parse(musicData.Offset);
            info.FolderName = musicData.FolderName;
            info.EasyLevel = musicData.Levels.Easy;
            info.NormalLevel = musicData.Levels.Normal;
            info.ExpertLevel = musicData.Levels.Expert;
            info.MasterLevel = musicData.Levels.Master;

            SongInfoList.Add(info);
        }

        Debug.Log("楽曲一覧データの読み込みが完了しました");
    }

}
