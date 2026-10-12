using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class week04_BallownAnim : MonoBehaviour
{

    [SerializeField] private CanvasGroup canversGroup;
    RectTransform rectTransform;

    /* private void Start() {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;
        canversGroup.alpha = 0f;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOScale(1.2f,0.3f).SetEase(Ease.OutBack));
        sequence.Append(rectTransform.DOScale(1f,0.15f));
        sequence.Join(canversGroup.DOFade(1f,0.2f));
    } */

    public void StartEventDeffet()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;
        canversGroup.alpha = 0f;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOScale(1.2f,0.3f).SetEase(Ease.OutBack));
        sequence.Append(rectTransform.DOScale(1f,0.15f));
        sequence.Join(canversGroup.DOFade(1f,0.2f));
    }
}
