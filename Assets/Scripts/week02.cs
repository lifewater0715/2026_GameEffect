using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class week02 : MonoBehaviour
{
    [SerializeField] private GameObject Target;
    [SerializeField] private SpriteRenderer Target2;
    [SerializeField] private SpriteRenderer Target3;

    void Start()
    {
        Debug.Log("죽어라 안해강!!!");
    }

    public void ButtenOutBck()
    {
        Target.transform.DORotate(new Vector3(0f, 0f, 330f), 2f, RotateMode.FastBeyond360).SetEase(Ease.OutBack);
    }
    public void ButtenInSine()
    {
        Target.transform.DORotate(new Vector3(0f, 0f, 330f), 2f, RotateMode.FastBeyond360).SetEase(Ease.InSine);
    }
    public void ButtenLinear()
    {
        Target.transform.DORotate(new Vector3(0f, 0f, 330f), 2f, RotateMode.FastBeyond360).SetEase(Ease.Linear);
    }
    public void ButtenOutBounce() 
    {
        Target.transform.DORotate(new Vector3(0f, 0f, 330f), 2f, RotateMode.FastBeyond360).SetEase(Ease.OutBounce);
    }
    public void ButtenScale()
    {
        Target.transform.DOScale(new Vector3(2f,2f,2f),2f);
    }
    public void ButtenPopUp()
    {
        Target.transform.localScale = Vector3.zero;
        Target.transform.DOScale(new Vector3(1f,1f,1f),0.5f)
        .SetEase(Ease.OutBack);
    }
    public void ButtenColor()
    {
        Target.gameObject.GetComponent<SpriteRenderer>().color = Color.aquamarine;
    }
    public void ButtenColorDOTO()
    {
        Target2.DOColor(Color.softRed,1f);
    }
    public void ButtenColorDOTOR()
    {
        Target3.DOColor(Color.softRed,1f);
    }
    public void Resetscene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
