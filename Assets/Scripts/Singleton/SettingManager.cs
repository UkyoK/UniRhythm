using Shine.Common;
using Shine.Json;
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

    public float StartBPM { get; private set; }

    public float Offset { get; private set; }

    public string FolderName { get; private set; }

    public Difficulty ChartDifficulty { get; set; }

    public int ChartLevel { get; private set; }

    public KeyCode Lane1 { get; private set; }
    public KeyCode Lane2 { get; private set; }
    public KeyCode Lane3 { get; private set; }
    public KeyCode Lane4 { get; private set; }

    /// <summary>
    /// 楽曲データが見つかったかどうか
    /// </summary>
    public bool isFindData { get; private set; }
    public void ResetIsFindData() { isFindData = false; }

    /// <summary>
    /// 選曲画面で先頭になる曲のID
    /// </summary>
    public int TopSongID { get; set; }

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

        // デフォルト設定を突っ込む
        NoteSpeed = 8;
        PerfectTime = 50;
        GreatTime = 75;
        MissTime = 100;
        IsMirror = false;
        LocalOffset = 0.0f;

        Title = "";
        ArtistName = "";
        StartBPM = 0;
        Offset = 0.0f;
        FolderName = "test_music";
        ChartDifficulty = Difficulty.Easy;
        ChartLevel = 0;

        isFindData = false;

        TopSongID = 0;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// デフォルト設定を適用
    /// </summary>
    /// <param name="isMirror">譜面をミラーにするか否か</param>
    public void SetDefaultSetting(bool isMirror = false)
    {
        NoteSpeed = 10;
        PerfectTime = 50;
        GreatTime = 75;
        MissTime = 100;
        IsMirror = isMirror;
        LocalOffset = 0.0f;

        Lane1 = KeyCode.D;
        Lane2 = KeyCode.F;
        Lane3 = KeyCode.J;
        Lane4 = KeyCode.K;
    }

    /// <summary>
    /// 楽曲データの読み込み
    /// </summary>
    /// <param name="songName">曲名</param>
    public void LoadChartData(string songName)
    {
        string path = Application.dataPath + "/StreamingAssets/MusicDatas/music_datas.json";

        if (!File.Exists(path))
        {
            isFindData = false;
            Debug.LogError("楽曲一覧データが見つかりませんでした");
            return;
        }

        string json = File.ReadAllText(path);
        Data data = JsonUtility.FromJson<Data>(json);

        // ゲーム内で使えるデータに変換
        foreach (MusicData musicData in data.MusicDatas)
        {
            // まずタイトルだけ取得
            Title = musicData.Title;

            // タイトルが設定されていた曲名と一致していれば処理
            if (Title == songName)
            {
                ArtistName = musicData.Artist;
                StartBPM = float.Parse(musicData.StartBPM);
                Offset = float.Parse(musicData.Offset);
                FolderName = musicData.FolderName;
                isFindData = true;
                break;
            }
        }

        if (ArtistName == "")
        {
            isFindData = false;
            Debug.LogError("楽曲データが見つかりませんでした");
            return;
        }

    }

    /// <summary>
    /// 設定を適用
    /// </summary>
    /// <param name="noteSpeed">譜面速度</param>
    /// <param name="perfectTime">Perfect判定時間(ms)</param>
    /// <param name="greatTime">Great判定時間(ms)</param>
    /// <param name="missTime">Miss判定時間(ms)</param>
    /// <param name="isMirror">ミラー設定</param>
    /// <param name="localOffset">ローカルオフセット</param>
    public void Setting(float noteSpeed, float perfectTime, float greatTime, float missTime, bool isMirror, float localOffset)
    {
        NoteSpeed = noteSpeed;
        PerfectTime = perfectTime;
        GreatTime = greatTime;
        MissTime = missTime;
        IsMirror = isMirror;
        LocalOffset = localOffset;
    }

    /// <summary>
    /// キーコンフィグ
    /// 左からレーン1,2,3,4
    /// </summary>
    public void KeyCongig(KeyCode key1, KeyCode key2, KeyCode key3, KeyCode key4)
    {
        Lane1 = key1;
        Lane2 = key2;
        Lane3 = key3;
        Lane4 = key4;
    }
}
