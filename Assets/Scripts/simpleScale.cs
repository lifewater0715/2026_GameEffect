using UnityEngine;
using DG.Tweening;

public class simpleScale : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            transform.DOScale(new Vector3(transform.position.x +2f,transform.position.y +2f,transform.position.z +2f),2f);
        }
    }
}
