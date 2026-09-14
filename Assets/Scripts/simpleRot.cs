using UnityEngine;
using DG.Tweening;

public class simpleRot : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.DORotate(new Vector3(0,0,360f),2f,RotateMode.FastBeyond360)
             .SetEase(Ease.Linear)
             .SetLoops(-1, LoopType.Restart);
    }
}
