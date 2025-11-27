using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayResult : MonoBehaviour
{
    [SerializeField]
    private GameObject PerfectObject;
    private TextMeshProUGUI PerfectDisp;
    [SerializeField]
    private GameObject GreatObject;
    private TextMeshProUGUI GreatDisp;
    [SerializeField]
    private GameObject MissObject;
    private TextMeshProUGUI MissDisp;

    [SerializeField]
    private GameObject ComboObject;
    private TextMeshProUGUI ComboDisp;

    [SerializeField]
    private GameObject ScoreObject;
    private TextMeshProUGUI ScoreDisp;

    // Start is called before the first frame update
    void Start()
    {
        PerfectDisp = PerfectObject.GetComponent<TextMeshProUGUI>();
        PerfectDisp.text = ScoreManager.Instance.PerfectCount.ToString();

        GreatDisp = GreatObject.GetComponent<TextMeshProUGUI>();
        GreatDisp.text = ScoreManager.Instance.GreatCount.ToString();

        MissDisp = MissObject.GetComponent<TextMeshProUGUI>();
        MissDisp.text = ScoreManager.Instance.MissCount.ToString();

        ComboDisp = ComboObject.GetComponent<TextMeshProUGUI>();
        ComboDisp.text = ScoreManager.Instance.MaxCombo.ToString();

        ScoreDisp = ScoreObject.GetComponent<TextMeshProUGUI>();
        ScoreDisp.text = ScoreManager.Instance.DispScore.ToString();

        Fade.Instance.FadeIn();

    }

}
