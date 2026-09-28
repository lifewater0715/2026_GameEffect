using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class week03_Append : MonoBehaviour
{
    void OnEnable()
    {
        DoAction();
    }

    private void DoAction()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMoveX(5.0f,1.0f));
        sequence.Append(gameObject.GetComponent<RectTransform>().DOAnchorPosX(100.0f,1.0f));
        sequence.Append(transform.DOScale(5f,1.0f));
    }
}
