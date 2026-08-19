
using System.Net;
using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class ShadowPlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidBody;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float startJumpGravityScale = 1.5f;
    [SerializeField] private float endJumpGravityScale = 1;

    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isPushing = false;
    private Coroutine walkingRoutine = null;
    public bool inWater = false;
    public bool isWalking = false;
    bool jumpBuffer;
    public bool disableInput = false;
    private float currentVelocity = 0f;
    private bool allowPlayerToPull;
    private Rigidbody2D currentlyPushedObject;
    private GameObject cutsceneLever;

    private void FixedUpdate()
    {
        if (playerRigidBody.linearVelocityY > 0)
        {
            playerRigidBody.gravityScale = startJumpGravityScale;
        }
        else
        {
            playerRigidBody.gravityScale = endJumpGravityScale;
        }
        Mathf.Clamp(playerRigidBody.linearVelocityY, -jumpForce, jumpForce);

    }

    private bool getJumpPressed()
    {
        if (Keyboard.current.spaceKey.isPressed || Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            return true;
        }
        else return false;
    }

    public void jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            playerRigidBody.linearVelocity = new Vector2(playerRigidBody.linearVelocity.x, jumpForce);
            animator.SetBool("IsJumping", true);
            isGrounded = false;
            jumpBuffer = false;
            this.GetComponent<PlaySoundEffect>().PlaySFX(1);
            this.GetComponent<PlaySoundEffect>().RandomisePitch(1, 0.9f, 1.1f);
        }
    }

    public void move(float VelX)
    {
        playerRigidBody.linearVelocityX = VelX * speed;
        if (VelX < 0)
        {
            playerRigidBody.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            playerRigidBody.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (playerRigidBody.linearVelocityY > 0)
        {
            playerRigidBody.gravityScale = startJumpGravityScale;
        }
        else
        {
            playerRigidBody.gravityScale = endJumpGravityScale;
        }
        Mathf.Clamp(playerRigidBody.linearVelocityY, -jumpForce, jumpForce);

        // Handle animations
        animator.SetFloat("Speed", Mathf.Abs(VelX));
        //Jump
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        float xVel = value.x;
        if (animator.GetBool("IsTouchingBox") == true)
        {
            // animator.SetBool("IsPushing", true);
        }
    }
    public void playerMoveWalkForAnimations(Vector2 playerVelocity)
    {
        //
        playerRigidBody.linearVelocity = playerVelocity;
        animator.SetFloat("Speed", Mathf.Abs(1));
        isWalking = true;
        if (walkingRoutine == null)
        {
            walkingRoutine = StartCoroutine(walkingSounds());
        }
    }
    public void stopMoveAniamtions()
    {
        playerRigidBody.linearVelocity = new Vector2(0, 0);
        animator.SetFloat("Speed", Mathf.Abs(0));
        isWalking = false;
        walkingRoutine = null;
    }
    public void changedTimeStopPush()
    {
        animator.SetBool("IsTouchingBox", false);
        animator.SetBool("Pulling", false);
        //  animator.SetBool("IsPushing", false);
    }
    public void CutscenHasBegun()
    {
        disableInput = true;
        animator.SetFloat("Speed", Mathf.Abs(0));
        playerRigidBody.linearVelocityX = 0;
        if (walkingRoutine != null)
        {
            StopCoroutine(walkingRoutine);
            walkingRoutine = null;
            isWalking = false;
        }

    }

    private IEnumerator walkingSounds()
    {
        //yield return null;
        // Debug.LogWarning("We starting this now");
        while (isWalking)
        {
            //  Debug.LogWarning("The coroutine has started, and walking is true");
            this.GetComponent<PlaySoundEffect>().PlaySFX(0);
            this.GetComponent<PlaySoundEffect>().RandomisePitch(0, 0.95f, 1.05f);
            float time = Time.time + 0.3f;
            while (time > Time.time)
            {
                yield return null;
            }
            yield return null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;

            // Ground check
            if (collision.gameObject.CompareTag("FutureTileMap") || collision.gameObject.CompareTag("PastTileMap") || collision.gameObject.transform.parent.CompareTag("FutureTileMap") || collision.gameObject.transform.parent.CompareTag("PastTileMap"))
            {
                if (normal.y > 0.5f)
                {
                    isGrounded = true;
                    isJumping = false;
                    animator.SetBool("IsJumping", false);
                    CancelInvoke("WaitForGrounded");
                    if (inWater == true)
                    {
                        Debug.Log("can move again in water");
                    }
                    playerRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                else
                {
                    //Debug.LogWarning("Woop, the normal of y is less than 0.5");
                }
            }

            // PushableBox side check
            if (collision.gameObject.CompareTag("PushableBox") || collision.gameObject.CompareTag("RollableObject"))
            {
                // Consider it pushing only if the normal is from the side
                if (Mathf.Abs(normal.x) > 0.5f)
                {
                    isPushing = true;
                    animator.SetBool("IsTouchingBox", true);
                    if (collision.gameObject.CompareTag("PushableBox"))
                    {
                        if (Keyboard.current.gKey.isPressed)
                        {

                            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocityX = playerRigidBody.linearVelocityX;
                        }
                        else
                        {

                            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
                        }
                    }
                }

                // Allow standing on top to count as grounded
                if (normal.y > 0.5f)
                {
                    isGrounded = true;
                    CancelInvoke("WaitForGrounded");
                    isJumping = false;
                    animator.SetBool("IsJumping", false);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FutureTileMap") || collision.gameObject.CompareTag("PastTileMap") || collision.gameObject.transform.parent.CompareTag("FutureTileMap") || collision.gameObject.transform.parent.CompareTag("PastTileMap"))
        {
            Invoke("WaitForGrounded", 0.3f);
            isWalking = false;
            //isGrounded = false;
        }

        if (collision.gameObject.CompareTag("PushableBox") || collision.gameObject.CompareTag("RollableObject"))
        {
            if (collision.gameObject.CompareTag("PushableBox"))
            {
                if (animator.GetBool("IsTouchingBox") == true && Keyboard.current.gKey.isPressed && allowPlayerToPull)
                {

                }
                else
                {
                    isPushing = false;
                    Debug.Log("WE NOT LONGER PUSHING ");
                    animator.SetBool("IsTouchingBox", false);
                    animator.SetBool("Pulling", false);
                    //collision.gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
                }
            }
            else
            {
                isPushing = false;
                Debug.Log("WE NOT LONGER PUSHING ");
                animator.SetBool("IsTouchingBox", false);
                animator.SetBool("Pulling", false);
            }

        }
    }
    private void WaitForGrounded()
    {
        isGrounded = false;
        isWalking = false;
        if (walkingRoutine != null)
        {
            StopCoroutine(walkingRoutine);
            walkingRoutine = null;
        }
        // this.GetComponent<PlaySoundEffect>().LoopSound(0, false);
    }

    public void canPlayerPullBox(bool answer, Rigidbody2D objectToPush = null)
    {
        allowPlayerToPull = answer;
        if (answer == false)
        {
            currentlyPushedObject.linearVelocityX = 0;
        }
        currentlyPushedObject = objectToPush;
    }

    public void assignCutsceneLever(GameObject obj)
    {
        cutsceneLever = obj;
    }

    public GameObject getCutsceneLever()
    {
        return cutsceneLever;
    }

}