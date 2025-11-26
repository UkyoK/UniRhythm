using Shine.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChartLoader : MonoBehaviour
{
    public static ChartLoader Instance;

    const float LANE1_POS_X = -1.5f;
    const float LANE2_POS_X = -0.5f;
    const float LANE3_POS_X = 0.5f;
    const float LANE4_POS_X = 1.5f;

    [SerializeField]
    GameObject NoteObject;

    [SerializeField]
    GameObject Lane1;
    [SerializeField]
    GameObject Lane2;
    [SerializeField]
    GameObject Lane3;
    [SerializeField]
    GameObject Lane4;

    [SerializeField]
    float NoteSpeed;

    List<ChartType> ChartTypeList = new List<ChartType>();
    List<NoteType> NoteTypeList = new List<NoteType>();

    string ChartDataPath;

    /// <summary>
    /// 曲名
    /// </summary>
    string Title;

    /// <summary>
    /// 楽曲製作者
    /// </summary>
    string Artist;

    /// <summary>
    /// 曲のテンポ
    /// </summary>
    float BPM;

    /// <summary>
    /// 開始時間調整
    /// </summary>
    [SerializeField]
    float Offset;

    /// <summary>
    /// 開始時間
    /// </summary>
    float StartTime;

    /// <summary>
    /// 合計時間
    /// </summary>
    float TotalTime;

    /// <summary>
    /// 情報が更新された小節
    /// </summary>
    int ChangedMeasure;

    /// <summary>
    /// 拍数
    /// </summary>
    int Beat;

    /// <summary>
    /// ノーツ数の合計
    /// </summary>
    public int AllNotesValue {  get; private set; }

    /// <summary>
    /// 譜面反転設定
    /// </summary>
    [SerializeField]
    bool IsMirror;

    public bool isFindData { get; private set; }

    string Path;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        isFindData = false;

        AllNotesValue = 0;
        Beat = 4;

        Title = SettingManager.Instance.Title;
        Artist = SettingManager.Instance.ArtistName;
        BPM = SettingManager.Instance.StartBPM;
        Offset = SettingManager.Instance.Offset;
        Path = Application.dataPath + "/StreamingAssets/MusicDatas/Music/" + SettingManager.Instance.FolderName + ".csv";

        StartTime = Time.time + SettingManager.Instance.LocalOffset;
        TotalTime = Offset;

        ChangedMeasure = 1;

        LoadChartFile();
    }

    void LoadChartData()
    {

    }

    void LoadChartFile()
    {
        isFindData = false;

        if (!File.Exists(Path))
        {
            Debug.LogError("譜面データが見つかりませんでした");
            return;
        }

        FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);

        sr.ReadLine();  // 1行目はスキップ(あとでファイル形式チェック処理にする)

        while (sr.Peek() != -1)
        {
            string line = sr.ReadLine();
            string[] split = line.Split(',');

            ChartInfoType infoType = (ChartInfoType)Enum.Parse(typeof(ChartInfoType), split[(int)ChartType.Info]);

            int measureNum = int.Parse(split[(int)ChartType.Measure]);
            switch (infoType)
            {
                case ChartInfoType.Note:
                    int laneNum = int.Parse(split[(int)ChartType.Lane]);
                    string body = split[(int)ChartType.Body];
                    MakeChart(measureNum, laneNum, body);
                    break;
                case ChartInfoType.MeasureChange:
                    int newBeat = int.Parse(split[(int)ChartType.Body]);
                    ChangeMeasure(measureNum, newBeat);
                    break;
                case ChartInfoType.BPMChange:
                    float newBPM = float.Parse(split[(int)ChartType.Body]);
                    ChangeBPM(measureNum, newBPM);
                    break;
                case ChartInfoType.SceneChange:
                    float measureSec = (60.0f / BPM) * Beat;
                    float endTime = measureSec * (measureNum - ChangedMeasure) + TotalTime + StartTime;
                    InGameManager.Instance.SetEndTime(endTime);
                    break;
                default:
                    break;
            }
        }

        isFindData = true;
    }

    void MakeChart(int measureNum, int laneNum, string body)
    {
        // 文字列を分割して配列にする
        char[] noteArray = body.ToCharArray();
        // 現在のBPMから1小節あたりの時間を計算
        float measureSec = (60.0f / BPM) * Beat;
        // 小節頭の時間を取得
        float nowMeasureTime = measureSec * (measureNum - ChangedMeasure) + TotalTime;
        // 1音符あたりの時間を計算
        float divisionSec = measureSec / noteArray.Length;

        for(int i  = 0; i < noteArray.Length; ++i)
        {
            switch (noteArray[i])
            {
                default:
                case '0':
                    break;

                case '1':
                    float justTime = nowMeasureTime + (divisionSec * i) + StartTime;
                    Note note = new Note(laneNum, justTime, false, NowJudgement.None);
                    ++AllNotesValue;
                    NoteInstantiate(laneNum, justTime);
                    break;

                // case 2:でロングノート始点、case 3:でロングノート終点をやれたらいいなあ
            }
        }
    }

    void NoteInstantiate(int laneNum, float justTime)
    {
        float posX = 0.0f;
        float posY = justTime * NoteSpeed;
        var parent = transform;

        int lane = laneNum;
        if(IsMirror)
        {
            lane = 4 - laneNum + 1;
        }

        switch(lane)
        {
            default:
                Debug.LogWarning("譜面データのレーン指定が誤っています");
                return;
            case 1:
                posX = LANE1_POS_X;
                parent = Lane1.transform;
                break;
            case 2:
                posX = LANE2_POS_X;
                parent = Lane2.transform;
                break;
            case 3:
                posX = LANE3_POS_X;
                parent = Lane3.transform;
                break;
            case 4:
                posX = LANE4_POS_X;
                parent = Lane4.transform;
                break;
        }

        Debug.Log("NoteJustTime: " + justTime);

        GameObject nowNote = Instantiate(NoteObject, Vector3.zero, Quaternion.identity, parent);
        HitNote hitNote = nowNote.GetComponent<HitNote>();
        Settings setting = GetComponent<Settings>();
        hitNote.Initialize(posX, justTime, SettingManager.Instance.NoteSpeed);
    }

    void ChangeBPM(int measureNum, float newBPM)
    {
        float measureSec = (60.0f / BPM) * Beat;
        TotalTime = measureSec * (measureNum - ChangedMeasure) + TotalTime;

        BPM = newBPM;
        ChangedMeasure = measureNum;
    }

    void ChangeMeasure(int measureNum, int newBeat)
    {
        float measureSec = (60.0f / BPM) * Beat;
        TotalTime = measureSec * (measureNum - ChangedMeasure) + TotalTime;

        Beat = newBeat;
        ChangedMeasure = measureNum;
    }

}
