using UnityEngine;
using DG.Tweening;

public class simpleRot1 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.DORotate(new Vector3(0, 0, 360), 1f);
    }
}
