
using System.Net;
using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidBody;
    [SerializeField] private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float startJumpGravityScale = 1.5f;
    [SerializeField] private float endJumpGravityScale = 1;
    [SerializeField] private ParticleSystem particleSystemLeft, ParticleSystemRight;
    [SerializeField] private ParticleSystem LandEffectAnimation;

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
    private void Awake()
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        if (!disableInput)
        {
            float xVel = 0f;

            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                xVel = -1f;
                if (isGrounded && !isWalking)
                {
                    isWalking = true;
                    if (walkingRoutine == null)
                    {
                        walkingRoutine = StartCoroutine("walkingSounds");
                    }
                }
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                xVel = 1f;
                if (isGrounded && !isWalking)
                {
                    isWalking = true;
                    if (walkingRoutine == null)
                    {
                        walkingRoutine = StartCoroutine("walkingSounds");
                    }
                }
            }
            else
            {
                isWalking = false;
                if (walkingRoutine != null)
                {
                    StopCoroutine(walkingRoutine);
                    walkingRoutine = null;
                }
            }

            // Apply movement
            playerRigidBody.linearVelocity = new Vector2(xVel * speed, playerRigidBody.linearVelocity.y);
            if (currentlyPushedObject != null && allowPlayerToPull)
            {
                if (Keyboard.current.gKey.isPressed)
                {
                    currentlyPushedObject.constraints = RigidbodyConstraints2D.FreezeRotation;
                    playerRigidBody.linearVelocityX = playerRigidBody.linearVelocityX / 2;
                    currentlyPushedObject.linearVelocityX = playerRigidBody.linearVelocityX;
                    if (playerRigidBody.linearVelocityX < 0 && gameObject.GetComponent<SpriteRenderer>().flipX == true)
                    {
                        if (gameObject.GetComponent<SpriteRenderer>().flipX == true)
                        {
                            //  animator.SetBool("Pulling", true);
                        }
                        else
                        {
                            //   animator.SetBool("Pulling", false);
                        }

                    }
                    else if (playerRigidBody.linearVelocityX > 1)
                    {
                        if (gameObject.GetComponent<SpriteRenderer>().flipX == true)
                        {
                            //     animator.SetBool("Pulling", false);
                        }
                        else
                        {
                            //   animator.SetBool("Pulling", true);
                        }
                    }
                }
                else
                {
                    if (currentlyPushedObject.linearVelocityY == 0)
                    {
                        currentlyPushedObject.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    }
                    if (currentlyPushedObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Static)
                    {
                        currentlyPushedObject.linearVelocityX = 0;
                    }
                    animator.SetBool("Pulling", false);

                }
            }

            // Flip the Character based on movement direction
            if (xVel > 0)
            {
                if (isPushing == true && Keyboard.current.gKey.isPressed)
                {
                    if (gameObject.GetComponent<SpriteRenderer>().flipX == true)
                    {
                        animator.SetBool("Pulling", true);

                    }
                    else
                    {
                        animator.SetBool("Pulling", false);
                    }

                    Debug.Log("the player is trying to pull against the box Iguess");
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    if (isGrounded)
                    {

                    }
                }
            }
            //transform.localScale = new Vector3(3, 3, 3);  // Facing right
            //this.gameObject.transform.localScale = new Vector3(2, 2, 2);
            else if (xVel < 0)
            {
                //transform.localScale = new Vector3(-3, 3, 3); // Facing left
                if (isPushing == true && Keyboard.current.gKey.isPressed)
                {
                    Debug.Log("the player is trying to pull against the box Iguess");
                    if (gameObject.GetComponent<SpriteRenderer>().flipX == false)
                    {
                        animator.SetBool("Pulling", true);

                    }
                    else
                    {
                        animator.SetBool("Pulling", false);
                    }
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                    if (isGrounded)
                    {

                    }
                }
            }
            else if (xVel == 0)
            {
                animator.SetBool("Pulling", false);

            }



            //this.gameObject.transform.localScale = new Vector3(-2, 2, 2);
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
            animator.SetFloat("Speed", Mathf.Abs(xVel));
            //Jump
            if (isGrounded && getJumpPressed() && !isJumping && !inWater)
            {
                isJumping = true;
                playerRigidBody.linearVelocity = new Vector2(playerRigidBody.linearVelocity.x, jumpForce);
                animator.SetBool("IsJumping", true);
                isGrounded = false;
                jumpBuffer = false;
                
                this.GetComponent<PlaySoundEffect>().PlaySFX(1);
                this.GetComponent<PlaySoundEffect>().RandomisePitch(1, 0.9f, 1.1f);
            }
            if (!Keyboard.current.spaceKey.isPressed && jumpBuffer == false)
            {
                jumpBuffer = true;
                if (isJumping && playerRigidBody.linearVelocityY > 1)
                {
                    //playerRigidBody.linearVelocityY = 0;
                }
            }
            currentVelocity = xVel;
        }
    }

    private bool getJumpPressed()
    {
        if (Keyboard.current.spaceKey.isPressed || Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            return true;
        }
        else return false;
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
        playerRigidBody.linearVelocityX = playerVelocity.x;
        animator.SetFloat("Speed", Mathf.Abs(1));
        isWalking = true;
        if (playerVelocity.x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
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
    public void CutsceneHasEnded()
    {
        disableInput = false;

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
            if (playerRigidBody.linearVelocityX > 0)
            {
                particleSystemLeft.Play();
            }
            else if (playerRigidBody.linearVelocityX < 0)
            {
                ParticleSystemRight.Play();

            }
            float time = Time.time + 0.3f;
            while (time > Time.time)
            {
                yield return null;
            }
            yield return null;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FutureTileMap") || collision.gameObject.CompareTag("PastTileMap") || collision.gameObject.transform.parent.CompareTag("FutureTileMap") || collision.gameObject.transform.parent.CompareTag("PastTileMap"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector2 normal = contact.normal;
                if (normal.y > 0.5f && !isGrounded)
                {

                    LandAfterJump();
                    return;

                }

            }
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
            if (currentlyPushedObject != null)
            {
                currentlyPushedObject.linearVelocityX = 0;
            }
        }
        currentlyPushedObject = objectToPush;
    }
    public bool isPlayerGrounded()
    {
        return isGrounded;
    }
    public bool isPlayerJumping()
    {
        return isJumping;
    }

  

    public void LandAfterJump()
    {
        if (isGrounded == false)
        {
            LandEffectAnimation.Play();
            GetComponent<EchoAbility>().isGroundedAgain();
            if (animator.GetFloat("Speed") < 0.1f)
                animator.SetTrigger("JustLanded");

        }
    }

}