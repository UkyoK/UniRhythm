using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class TextScroller : MonoBehaviour
{
    /// <summary>
    /// スクロール速度
    /// </summary>
    [SerializeField]
    private float ScrollSpeed = 100.0f;

    /// <summary>
    /// スクロール終了判定までのマージン
    /// </summary>
    [SerializeField]
    private float ScrollFinishLineAddValue = 50;

    /// <summary>
    /// スクロール開始前の待機時間
    /// </summary>
    [SerializeField]
    private float WaitTime = 2.0f;


    /// <summary>
    /// テキストのフェードインアウト時間
    /// </summary>
    [SerializeField]
    private float FadeDuration = 0.5f;

    /// <summary>
    /// フェードアウトしてからフェードインするまでの待機時間
    /// </summary>
    [SerializeField]
    private float WaitTimeFade = 0.2f;

    private CanvasGroup CanvasGroup;
    private RectTransform TextRectTransform;

    /// <summary>
    /// スタート位置
    /// </summary>
    private Vector3 StartPosition;

    /// <summary>
    /// スクロールが停止する位置
    /// </summary>
    private float FinishLineValue;

    private void Start()
    {
        if (!TryGetComponent<TMP_Text>(out var textComponent))
        {
            Debug.LogWarning("TMP_Textがアタッチされていません");
            return;
        }

        // ContentSizeFitterを更新
        ContentSizeFitter contentSizeFitter = GetComponent<ContentSizeFitter>();
        contentSizeFitter.SetLayoutHorizontal();
        contentSizeFitter.SetLayoutVertical();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentSizeFitter.GetComponent<RectTransform>());

        CanvasGroup = GetComponent<CanvasGroup>();
        TextRectTransform = textComponent.GetComponent<RectTransform>();
        float textWidth = TextRectTransform.rect.width;
        float parentWidth = textComponent.transform.parent.GetComponent<RectTransform>().rect.width;
        StartPosition = TextRectTransform.anchoredPosition;
        FinishLineValue = StartPosition.x - (textWidth + StartPosition.x + ScrollFinishLineAddValue - parentWidth);
        if (textWidth + StartPosition.x > parentWidth)
        {
            Scroll(this.GetCancellationTokenOnDestroy()).Forget();
        }
    }

    /// <summary>
    /// スクロール処理
    /// </summary>
    private async UniTaskVoid Scroll(CancellationToken token)
    {
        while (true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(WaitTime), cancellationToken: token);

            // 移動
            await TextRectTransform.DOAnchorPosX(FinishLineValue, ScrollSpeed).SetSpeedBased().SetEase(Ease.Linear).ToUniTask(cancellationToken: token);
            await UniTask.Delay(TimeSpan.FromSeconds(WaitTime), cancellationToken: token);

            // フェードアウト
            await CanvasGroup.DOFade(0, FadeDuration).SetEase(Ease.Linear);

            // 見えないうちに初期位置へ移動
            TextRectTransform.anchoredPosition = StartPosition;
            await UniTask.Delay(TimeSpan.FromSeconds(WaitTimeFade), cancellationToken: token);

            // フェードイン
            await CanvasGroup.DOFade(1, FadeDuration).SetEase(Ease.Linear).ToUniTask(cancellationToken: token);
        }
    }

}