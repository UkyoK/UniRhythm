using Shine.Common;
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
        }
        else
        {
            Destroy(gameObject);
        }

        SongInfoList = new List<SongInfo>();

        DontDestroyOnLoad(gameObject);

        LoadMusicInfoData();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void LoadMusicInfoData()
    {
        string path = Application.dataPath + "/StreamingAssets/MusicDatas/music_datas.csv";

        if (!File.Exists(path))
        {
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

            SongInfo songInfo = new SongInfo();
            songInfo.Title = split[(int)MusicInfo.Title];
            songInfo.Artist = split[(int)MusicInfo.Artist];
            songInfo.StartBPM = float.Parse(split[(int)MusicInfo.StartBPM]);
            songInfo.Offset = float.Parse(split[(int)MusicInfo.Offset]);
            songInfo.FolderName = split[(int)MusicInfo.FolderName];

            SongInfoList.Add(songInfo);
        }

        Debug.Log("楽曲一覧データの読み込みが完了しました");
    }

}
