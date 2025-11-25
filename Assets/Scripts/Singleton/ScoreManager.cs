using Shine.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRhythm_acf.Selector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private const string _InGameScene = "TestScene";
    private const string _ResultScene = "ResultScene";

    private const float _ScreenWidth = 1280.0f;
    private const float _ScreenHeight = 720.0f;

    public int Combo { get; private set; }
    public int MaxCombo { get; private set; }
    public int PerfectCount { get; private set; }
    public int FastGreatCount { get; private set; }
    public int LateGreatCount { get; private set; }
    public int GreatCount { get; private set; }
    public int FastMissCount { get; private set; }
    public int LateMissCount { get; private set; }
    public int MissCount { get; private set; }

    [SerializeField]
    private GameObject JudgeCanvas;
    private Transform Parent;
    [SerializeField]
    private GameObject PerfectObject;
    [SerializeField]
    private GameObject GreatObject;
    [SerializeField]
    private GameObject MissObject;

    [SerializeField]
    private float DestroyTime;

    private Vector3 ObjectPos;

    [SerializeField]
    private GameObject ComboObject;
    private TextMeshProUGUI ComboDisp;

    [SerializeField]
    private GameObject ScoreObject;
    private TextMeshProUGUI ScoreDisp;

    private float PerfectScore;
    private float GreatScore;
    private float NowScore;
    public int DispScore { get; private set; }

    [SerializeField]
    private float MaxScore;

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

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ResetScore();
    }

    public void ResetScore()
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

        PerfectScore = MaxScore / ChartLoader.Instance.AllNotesValue;
        GreatScore = PerfectScore / 2;
        NowScore = 0.0f;
        DispScore = 0;

        if (SceneManager.GetActiveScene().name == _InGameScene)
        {
            InGameDisplay();
        }
        else if (SceneManager.GetActiveScene().name == _ResultScene)
        {
            ResultDisplay();
        }
    }

    public void InGameDisplay()
    {
        Parent = JudgeCanvas.transform;
        ObjectPos = new Vector3(_ScreenWidth / 2.0f, _ScreenHeight / 2.0f - 30.0f, 0.0f);

        ComboDisp = ComboObject.GetComponent<TextMeshProUGUI>();
        ComboDisp.text = "0";

        ScoreDisp = ScoreObject.GetComponent<TextMeshProUGUI>();
        ScoreDisp.text = "0000000";
    }

    private void UpdateScoreDisplay()
    {
        ScoreDisp.text = DispScore.ToString("0000000");
    }

    public void ResultDisplay()
    {
        /* ここにリザルトシーンの初期化を記載 */
    }

    public void AddCombo()
    {
        ++Combo;
        ComboDisp.text = Combo.ToString();
        if(Combo >= MaxCombo)
        {
            MaxCombo = Combo;
        }
    }

    /// <summary>
    /// ミス(入力時)
    /// </summary>
    /// <param name="margin"></param>
    public void Miss(float margin)
    {
        Combo = 0;
        ComboDisp.text = Combo.ToString();
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
        ComboDisp.text = Combo.ToString();
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

        UpdateScoreDisplay();
    }
    public void Perfect()
    {
        ++PerfectCount;
        NowScore += PerfectScore;
        DispScore = (int)NowScore;
        UpdateScoreDisplay();
    }

    public void JudgementDisplay(Judgement judgement)
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

        GameObject Instant = Instantiate(go, ObjectPos, Quaternion.identity, Parent);
        Destroy(Instant, DestroyTime);
    }

}
