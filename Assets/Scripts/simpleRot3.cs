using UnityEngine;
using DG.Tweening;

public class simpleRot3 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.DORotate(new Vector3(0, 0, 330), 2f);
        }
    }
}
