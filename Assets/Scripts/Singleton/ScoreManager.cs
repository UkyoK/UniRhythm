using Shine.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRhythm_acf.Selector;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int Combo;
    private int MaxCombo;
    private int PerfectCount;
    private int FastGreatCount;
    private int LateGreatCount;
    private int GreatCount;
    private int FastMissCount;
    private int LateMissCount;
    private int MissCount;

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

        Combo = 0;
        MaxCombo = 0;
        PerfectCount = 0;
        FastGreatCount = 0;
        LateGreatCount = 0;
        GreatCount = 0;
        FastMissCount = 0;
        LateMissCount = 0;
        MissCount = 0;

        Parent = GameObject.Find("JudgeCanvas").transform;
        ObjectPos = new Vector3(1280.0f / 2.0f, 720.0f / 2.0f - 30.0f, 0.0f);

        ComboDisp = ComboObject.GetComponent<TextMeshProUGUI>();
        ComboDisp.text = "0";

        ScoreDisp = ScoreObject.GetComponent<TextMeshProUGUI>();
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

        if(margin < 0)
        {
            ++LateGreatCount;
        }
        else
        {
            ++FastGreatCount;
        }
    }
    public void Perfect()
    {
        ++PerfectCount;
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
