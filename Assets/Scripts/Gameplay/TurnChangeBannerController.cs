using UnityEngine;
using TMPro;
using DG.Tweening;

public class TurnChangeBannerController : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] RectTransform _startPosition;
    [SerializeField] RectTransform _middlePosition;
    [SerializeField] RectTransform _endPosition;

    // Set the banner's text and send it down
    public void SetBannerText(string newText)
    {
        _text.text = newText;

        _rectTransform.localPosition = _startPosition.localPosition;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_rectTransform.DOLocalMove(_middlePosition.localPosition, 1f).SetEase(Ease.OutCubic));
        sequence.Append(_rectTransform.DOLocalMove(_endPosition.localPosition, 1f).SetEase(Ease.InCubic));
    }
}
