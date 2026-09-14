using UnityEngine;
using DG.Tweening;

public class simpleMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            transform.DOMove(new Vector3(transform.position.x +10f,transform.position.y +10f,transform.position.z +10f),1f);
        }
    }
}
