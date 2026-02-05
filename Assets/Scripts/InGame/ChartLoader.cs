using Shine.Common;
using System;
using System.IO;
using UnityEngine;

public class ChartLoader : MonoBehaviour
{
    public static ChartLoader Instance;

    /// <summary>
    /// シーン読み込み完了後の待機時間
    /// </summary>
    [SerializeField]
    public float WaitTime = 1.0f;

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

    /// <summary>
    /// 譜面速度
    /// </summary>
    [SerializeField]
    float NoteSpeed;

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
    /// 拍数(Beat/4)
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

    /// <summary>
    /// 譜面データが見つかったかどうか
    /// </summary>
    public bool isFindData { get; private set; }

    string FileName;
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

        BPM = SettingManager.Instance.StartBPM;
        Offset = SettingManager.Instance.Offset;
        FileName = SettingManager.Instance.FolderName + "_" + SettingManager.Instance.ChartDifficulty.ToString();
        Path = Application.dataPath + "/StreamingAssets/MusicDatas/Music/" + SettingManager.Instance.FolderName + "/" + FileName + ".csv";

        StartTime = Time.time + SettingManager.Instance.LocalOffset;
        TotalTime = Offset + Fade.Instance.FadeTime + WaitTime;

        ChangedMeasure = 1;

        LoadChartFile();
    }

    /// <summary>
    /// 譜面データ読み込み
    /// </summary>
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

        // データ形式チェック
        string checkLine = sr.ReadLine();
        string[] checkSprit = checkLine.Split(',');
        for (int i = 0; i < (int)ChartType.MAX; ++i)
        {
            ChartType info = (ChartType)Enum.ToObject(typeof(ChartType), i);

            if (checkSprit[i] != info.ToString())
            {
                Debug.LogError("譜面データの形式が間違っています");
                return;
            }
        }

        // 譜面生成
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

    /// <summary>
    /// 1小節分のノーツを生成
    /// </summary>
    /// <param name="measureNum">小節数</param>
    /// <param name="laneNum">レーン番号</param>
    /// <param name="body">譜面</param>
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
                    Note note = new Note(laneNum, justTime);
                    ++AllNotesValue;
                    NoteInstantiate(laneNum, justTime);
                    break;

                // case 2:でロングノート始点、case 3:でロングノート終点をやれたらいいなあ
            }
        }
    }

    /// <summary>
    /// ノート本体を生成
    /// </summary>
    /// <param name="laneNum">レーン番号</param>
    /// <param name="justTime">判定時間</param>
    void NoteInstantiate(int laneNum, float justTime)
    {
        float posX = 0.0f;
        float posY = justTime * NoteSpeed;
        var parent = transform;

        int lane = laneNum;
        if(SettingManager.Instance.IsMirror)
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
        hitNote.Initialize(posX, justTime, SettingManager.Instance.NoteSpeed);
    }

    /// <summary>
    /// BPM変更
    /// </summary>
    /// <param name="measureNum">小節番号</param>
    /// <param name="newBPM">変更後のBPM</param>
    void ChangeBPM(int measureNum, float newBPM)
    {
        float measureSec = (60.0f / BPM) * Beat;
        TotalTime = measureSec * (measureNum - ChangedMeasure) + TotalTime;

        BPM = newBPM;
        ChangedMeasure = measureNum;
    }

    /// <summary>
    /// 拍子変更
    /// </summary>
    /// <param name="measureNum">小節番号</param>
    /// <param name="newBeat">変更後の拍子(n/4)</param>
    void ChangeMeasure(int measureNum, int newBeat)
    {
        float measureSec = (60.0f / BPM) * Beat;
        TotalTime = measureSec * (measureNum - ChangedMeasure) + TotalTime;

        Beat = newBeat;
        ChangedMeasure = measureNum;
    }

}
