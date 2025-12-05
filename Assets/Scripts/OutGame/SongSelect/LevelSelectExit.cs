using Cysharp.Threading.Tasks;
using DG.Tweening;
using Shine.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectExit : MonoBehaviour
{
    EventSystem _EventSystem;

    private void Awake()
    {
        _EventSystem = FindObjectOfType<EventSystem>();
    }

    public async void OnClickExit()
    {
        // 難易度選択をフェードアウト
        await LevelSelect.Instance._CanvasGroup.DOFade(0.0f, LevelSelect.Instance._FadeTime).SetEase(Ease.InOutQuad);

        // 選曲状態を変更
        SongSelect.Instance.SongSelectState = SelectState.SongSelect;

        // 選択中ボタンをリセット
        _EventSystem.SetSelectedGameObject(_EventSystem.firstSelectedGameObject);
        transform.parent.gameObject.SetActive(false);
    }
}
