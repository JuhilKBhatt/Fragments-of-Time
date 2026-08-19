using UnityEngine;
using UnityEngine.InputSystem;

public class TempPlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D playerRigidBody;
    public float speed;
    public float JumpHeight;
    private float yVel;
    private float xVel;
    private bool isGrounded = false;

    private void Update()
    {
        playerRigidBody.linearVelocity = new Vector2(xVel * speed, playerRigidBody.linearVelocityY);
    }
    public void OnMove(InputAction.CallbackContext context)
    {

        Vector2 value = context.ReadValue<Vector2>();
        xVel = value.x;
        if(context.started) //does nothing right now
        {

        }

        if(context.canceled) //does nothing right now
        {

        }

    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if(isGrounded && context.started)
        {
        playerRigidBody.linearVelocityY = JumpHeight;
        isGrounded = false;
        }
    }

    public void  OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "FutureTileMap" || other.gameObject.tag == "PastTileMap" || other.gameObject.tag == "PushableBox")
        {
            isGrounded = true;
        }
    }
}
