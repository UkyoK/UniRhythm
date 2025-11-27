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

    [SerializeField]
    private GameObject ComboState;
    private TextMeshProUGUI ComboStateDisp;

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

        ComboStateDisp = ComboState.GetComponent<TextMeshProUGUI>();
        if (ScoreManager.Instance.PerfectCount == ScoreManager.Instance.AllCombo)
        {
            ComboStateDisp.fontStyle = FontStyles.Bold | FontStyles.Italic;
            ComboStateDisp.color = new Color(1.0f, 0.0f, 1.0f, 1.0f);
            ComboStateDisp.text = "All Perfect!!";
        }
        else if (ScoreManager.Instance.MaxCombo == ScoreManager.Instance.AllCombo)
        {
            ComboStateDisp.fontStyle = FontStyles.Bold;
            ComboStateDisp.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
            ComboStateDisp.text = "Full Combo!";
        }
        else
        {
            ComboStateDisp.fontStyle = FontStyles.Normal;
            ComboStateDisp.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ComboStateDisp.text = "Max Combo";
        }

        Fade.Instance.FadeIn();
    }

}
