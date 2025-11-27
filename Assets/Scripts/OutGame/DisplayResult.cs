using TMPro;
using UnityEngine;

public class DisplayResult : MonoBehaviour
{
    /// <summary>
    /// Perfect表示オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject PerfectObject;
    /// <summary>
    /// Perfect表示TMPro
    /// </summary>
    private TextMeshProUGUI PerfectDisp;

    /// <summary>
    /// Great表示オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject GreatObject;
    /// <summary>
    /// Great表示TMPro
    /// </summary>
    private TextMeshProUGUI GreatDisp;

    /// <summary>
    /// Miss表示オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject MissObject;
    /// <summary>
    /// Miss表示TMPro
    /// </summary>
    private TextMeshProUGUI MissDisp;

    /// <summary>
    /// コンボ表示オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject ComboObject;
    /// <summary>
    /// コンボ表示TMPro
    /// </summary>
    private TextMeshProUGUI ComboDisp;

    /// <summary>
    /// スコア表示オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject ScoreObject;
    /// <summary>
    /// スコア表示TMPro
    /// </summary>
    private TextMeshProUGUI ScoreDisp;

    /// <summary>
    /// コンボ状態表示オブジェクト
    /// </summary>
    [SerializeField]
    private GameObject ComboState;
    /// <summary>
    /// コンボ状態表示TMPro
    /// </summary>
    private TextMeshProUGUI ComboStateDisp;

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
