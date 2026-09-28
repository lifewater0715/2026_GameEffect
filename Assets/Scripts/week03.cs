using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class week03 : MonoBehaviour
{
    [SerializeField] private GameObject Target1;
    [SerializeField] private SpriteRenderer Target2;
    [SerializeField] private SpriteRenderer Target3;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] Ease easetype = Ease.Linear;
    private bool toggle = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            float targetalpha = toggle ? 1.0f : 0.0f;
            canvasGroup.DOFade(targetalpha,1f).SetEase(easetype);
            toggle = !toggle;
        }
    }

    public void ButtenColorRGB(Color GetColor)
    {
        Target1.gameObject.GetComponent<SpriteRenderer>().color = GetColor;
        Target2.DOColor(GetColor,1f);
        Target3.DOColor(GetColor,1f);
    }

    public void Blue()
    {
        ButtenColorRGB(Color.blue);
    }

    public void Red()
    {
        ButtenColorRGB(Color.red);
    }

    public void Green()
    {
        ButtenColorRGB(Color.green);
    }

    public void Resetscene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
