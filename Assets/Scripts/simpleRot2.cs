using UnityEngine;
using DG.Tweening;

public class simpleRot2 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.DORotate(

    transform.eulerAngles + new Vector3(0, 0, 360),
    2f,
    RotateMode.FastBeyond360
    ).SetEase(Ease.OutBack);
    }
}
