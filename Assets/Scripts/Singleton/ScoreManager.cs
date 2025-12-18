using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UniRhythm_acf.Selector;
using UnityEngine;
using UnityEngine.SceneManagement;
using R3;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private const string _InGameScene = "InGameScene";

    public int Combo { get; private set; }
    public int MaxCombo { get; private set; }
    public int PerfectCount { get; private set; }
    public int FastGreatCount { get; private set; }
    public int LateGreatCount { get; private set; }
    public int GreatCount { get; private set; }
    public int FastMissCount { get; private set; }
    public int LateMissCount { get; private set; }
    public int MissCount { get; private set; }
    public int AllCombo { get; private set; }

    /// <summary>
    /// 判定表示キャンバス
    /// </summary>
    [SerializeField]
    private GameObject JudgeCanvas;
    private Transform Parent;

    /// <summary>
    /// PerfectのPrefab
    /// </summary>
    [SerializeField]
    private GameObject PerfectObject;
    /// <summary>
    /// GreatのPrefab
    /// </summary>
    [SerializeField]
    private GameObject GreatObject;
    /// <summary>
    /// MissのPrefab
    /// </summary>
    [SerializeField]
    private GameObject MissObject;

    /// <summary>
    /// 判定表示が消えるまでの時間
    /// </summary>
    [SerializeField]
    private float DestroyTime;

    /// <summary>
    /// コンボ表示
    /// </summary>
    [SerializeField]
    private GameObject ComboObject;
    private ComboDisplayer ComboDisp;

    /// <summary>
    /// スコア表示
    /// </summary>
    [SerializeField]
    private GameObject ScoreObject;
    private TextMeshProUGUI ScoreDisp;

    private float PerfectScore;
    private float GreatScore;
    private float NowScore;
    public int DispScore { get; private set; }

    public bool IsAllPerfect = false;

    /// <summary>
    /// 理論値
    /// </summary>
    [SerializeField]
    private float MaxScore;

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

        IsAllPerfect = false;
    }

    private void Start()
    {
        Combo = 0;
        MaxCombo = 0;
        PerfectCount = 0;
        FastGreatCount = 0;
        LateGreatCount = 0;
        GreatCount = 0;
        FastMissCount = 0;
        LateMissCount = 0;
        MissCount = 0;

        AllCombo = ChartLoader.Instance.AllNotesValue;

        PerfectScore = MaxScore / ChartLoader.Instance.AllNotesValue;
        GreatScore = PerfectScore / 2;
        NowScore = 0.0f;
        DispScore = 0;

        float totalscore = PerfectScore * ChartLoader.Instance.AllNotesValue;

        ComboDisp = ComboObject.gameObject.GetComponent<ComboDisplayer>();

        if (SceneManager.GetActiveScene().name == _InGameScene)
        {
            Parent = JudgeCanvas.transform;

            ScoreDisp = ScoreObject.GetComponent<TextMeshProUGUI>();
            ScoreDisp.text = "0000000";
        }
    }

    private void UpdateScoreDisplay()
    {
        ScoreDisp.text = DispScore.ToString("0000000");
    }

    /// <summary>
    /// ミス(入力時)
    /// </summary>
    /// <param name="margin"></param>
    public void Miss(float margin)
    {
        ComboDisp.Miss();

        ++MissCount;

        if (margin < 0)
        {
            ++LateMissCount;
        }
        else
        {
            ++FastMissCount;
        }
    }

    /// <summary>
    /// ミス(放置時)
    /// </summary>
    public void Miss()
    {
        Combo = 0;
        ComboDisp.Miss();
        ++MissCount;
    }

    public void Great(float margin)
    {
        ++GreatCount;
        NowScore += GreatScore;
        DispScore = (int)NowScore;

        if (margin < 0)
        {
            ++LateGreatCount;
        }
        else
        {
            ++FastGreatCount;
        }

        ComboDisp.AddCombo();
        UpdateScoreDisplay();
    }
    public void Perfect()
    {
        ++PerfectCount;
        NowScore += PerfectScore;
        DispScore = (int)NowScore;

        // スコアの誤差が発生したら穴埋めする
        if ((PerfectScore * (PerfectCount + GreatCount + MissCount)) - DispScore > 0)
        {
            NowScore += 1;
            DispScore = (int)NowScore;
        }
        if (DispScore > MaxScore)
        {
            DispScore = (int)MaxScore;
        }

        if (PerfectCount == ChartLoader.Instance.AllNotesValue)
        {
            IsAllPerfect = true;
            MySoundManager.Instance.PlayClearVoice(ClearState.AllPerfect);
        }

        ComboDisp.AddCombo();
        UpdateScoreDisplay();
    }

    /// <summary>
    /// 判定表示生成
    /// </summary>
    /// <param name="judgement"></param>
    public void JudgementDisplay(Judgement judgement, Vector3 lanePos)
    {
        GameObject go = MissObject;
        switch(judgement)
        {
            case Judgement.Perfect:
                go = PerfectObject;
                break;

            case Judgement.Great:
                go = GreatObject;
                break;

            case Judgement.Miss:
                go = MissObject;
                break;
        }

        GameObject Instant = Instantiate(go, lanePos, Quaternion.identity, Parent);
        Destroy(Instant, DestroyTime);
    }

}
