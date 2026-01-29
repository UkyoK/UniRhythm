using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using TMPro;
using UniRhythm_acf.Selector;
using UnityEngine;

public class ComboDisplayer : MonoBehaviour
{
    ReactiveProperty<int> _currentCombo = new ReactiveProperty<int>(0);
    int MaxCombo = 0;
    public Observable<int> Observer => _currentCombo;

    Vector3 DefaultScale;
    Vector3 BigScale;

    /// <summary>
    /// コンボエフェクト
    /// </summary>
    [SerializeField]
    private GameObject ComboEffect;
    private ParticleSystem ComboParticle;

    private TextMeshProUGUI ComboText;

    /// <summary>
    /// 拡大時の倍率
    /// </summary>
    [SerializeField]
    float _BigSize = 1.33f;

    /// <summary>
    /// 拡縮秒数
    /// </summary>
    [SerializeField]
    float _Duration = 0.3f;

    void Awake()
    {
        DefaultScale = transform.localScale;
        BigScale = new Vector3(DefaultScale.x * _BigSize, DefaultScale.y * _BigSize, DefaultScale.z * _BigSize);

        ComboText = gameObject.GetComponent<TextMeshProUGUI>();

        ComboParticle = ComboEffect.GetComponent<ParticleSystem>();
        ComboEffect.SetActive(true);
        ComboParticle.Stop();

        MaxCombo = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentCombo.Subscribe(
            combo =>
            {
                ComboText.text = combo.ToString();
                if (combo != 0)
                {
                    transform.DOComplete();
                    transform.localScale = BigScale;
                    transform.DOScale(DefaultScale, _Duration);
                    // 最大コンボ変更
                    if (_currentCombo.CurrentValue > MaxCombo)
                    {
                        MaxCombo = _currentCombo.CurrentValue;
                    }
                    // 現在コンボ表示
                    ComboText.text = _currentCombo.CurrentValue.ToString();
                    // フルコンボ確認
                    if (_currentCombo.CurrentValue == ChartLoader.Instance.AllNotesValue && !ScoreManager.Instance.IsAllPerfect)
                    {
                        MySoundManager.Instance.PlayClearVoice(ClearState.FullCombo);
                        return;
                    }
                    // コンボエフェクト再生
                    if (_currentCombo.CurrentValue % 50 == 0 && _currentCombo.CurrentValue != 0)
                    {
                        ComboParticle.Play();
                    }
                }
            }).AddTo(this);
    }


    public int AddCombo()
    {
        // コンボ加算を通知
        _currentCombo.OnNext(_currentCombo.CurrentValue + 1);

        return MaxCombo;
    }

    public void Miss()
    {
        _currentCombo.OnNext(0);
    }
}
