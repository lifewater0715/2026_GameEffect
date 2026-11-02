using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerController))]
public class PlayerController : MonoBehaviour
{
    public float movespeed = 5f;
    public float gravity = -20f;

    private CharacterController characterController;
    private float verticalvelcity;

    private void Awake()
    {
        characterController = gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(hor,0f,ver);
        move = Vector3.ClampMagnitude(move,1f);

        if(!characterController.isGrounded && verticalvelcity <= 0)
        {
            verticalvelcity = -2f;
        }

        Vector3 velocity = move * movespeed;
        velocity.y = verticalvelcity;

        characterController.Move(velocity * Time.deltaTime);
    }
}
