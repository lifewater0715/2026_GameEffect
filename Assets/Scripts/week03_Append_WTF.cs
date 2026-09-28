using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class week03_Append_WTF : MonoBehaviour
{
    [SerializeField] private GameObject Target;
    private bool toggle = false;
    void OnEnable()
    {
        //DoAction();
    }

    public void IsHoverDoAction()
    {
        float xrand = Random.Range(-250f, 250f);
        float yrand = Random.Range(-250f, 250f);

        Sequence sequence = DOTween.Sequence();
        //sequence.Append(transform.DOMoveX(5.0f,1.0f));
        sequence.Append(gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(xrand, yrand), 0.5f));
        //sequence.Append(transform.DOScale(5f,1.0f));
    }

    public void PikaDoAction()
    {
        if (!toggle)
        {
            DOTween.Kill(transform);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(Target.GetComponent<Image>().DOFade(1f, 0f));
            sequence.Join(Target.transform.DOScale(5f, 1.0f));
            sequence.Join(Target.GetComponent<Image>().DOColor(Color.coral, 1.0f));
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(Target.transform.DOScale(1.0f, 1.0f));
            sequence.Join(Target.GetComponent<Image>().DOColor(Color.white, 1.0f));
            sequence.Join(Target.GetComponent<Image>().DOFade(0.0f, 1.0f));

        }
        toggle = !toggle;

    }
}
