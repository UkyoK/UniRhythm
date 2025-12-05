using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shine.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public static LevelSelect Instance;

    public CanvasGroup _CanvasGroup;

    [SerializeField]
    public float _FadeTime = 0.5f;

    public EventSystem _EventSystem;

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

        _CanvasGroup = GetComponent<CanvasGroup>();
        _CanvasGroup.alpha = 0.0f;
        _EventSystem = FindObjectOfType<EventSystem>();
        _EventSystem.enabled = false;
        gameObject.SetActive(false);
    }

    void Start()
    {

    }

    public async UniTask LevelSelectStep()
    {
        gameObject.SetActive(true);
        _EventSystem.SetSelectedGameObject(_EventSystem.firstSelectedGameObject);
        await _CanvasGroup.DOFade(1.0f, _FadeTime).SetEase(Ease.InOutQuad);
        SongSelect.Instance.SongSelectState = SelectState.LevelSelect;
    }

    async void Update()
    {
        // 難易度選択状態でないなら、処理を行わない
        if (SongSelect.Instance.SongSelectState != SelectState.LevelSelect)
        {
            // イベントシステムを無効にする
            _EventSystem.SetSelectedGameObject(_EventSystem.firstSelectedGameObject);
            _EventSystem.enabled = false;
            return;
        }
        else if (_EventSystem.enabled == false)
        {
            // イベントシステムを有効にする
            _EventSystem.enabled = true;
        }

        // Escapeキーで難易度選択から楽曲選択に戻る
        if (Input.GetKey(KeyCode.Escape))
        {
            await _CanvasGroup.DOFade(0.0f, _FadeTime).SetEase(Ease.InOutQuad);
            SongSelect.Instance.SongSelectState = SelectState.SongSelect;
            gameObject.SetActive(false);
            return;
        }

    }
}
