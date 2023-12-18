using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    //Private Fields
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] Collider2D standingCollider;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] Transform overheadCheckCollider;
    [SerializeField] LayerMask groundLayer;

    const float groundCheckRadius = 0.2f;
    const float overheadCheckRadius = 0.2f;
    [SerializeField] float speed = 2;
    [SerializeField] float jumpPower = 500;
    float horizontalValue;
    float runSpeedModifier = 2f;
    float crouchSpeedModifier = 0.5f;

    bool isGrounded;
    bool isRunning;
    bool facingRight = true;
    bool jump;
    bool crouchPressed;

    [SerializeField] private Text CoinCounter;
    private int RCP;
    public AudioSource suaraCoin;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Coin Counter
        RCP = 0;
    }

    void Update()
    {
        //Set the yVelocity in the animator
        animator.SetFloat("yVelocity", rb.velocity.y);

        //Store the horizontal value
        horizontalValue = Input.GetAxisRaw("Horizontal");

        //If LShift is clicked enable isRunning
        if(Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = true;
        //If LShift is released disable isRunning
         if(Input.GetKeyUp(KeyCode.LeftShift))
         isRunning = false;

         //If we press Jump button enable jump
         if(Input.GetButtonDown("Jump"))
         {
            jump = true;
            animator.SetBool("Jump", true);
         }

        //otherwise disable it
        else if(Input.GetButtonUp("Jump"))
           jump = false;

         //If we press Crouch button enable crouch
         if(Input.GetButtonDown("Crouch"))
            crouchPressed = true;
        //otherwise disable it
        else if(Input.GetButtonUp("Crouch"))
           crouchPressed = false;

           //Coin Counter
           CoinCounter.text = "RCP : " + RCP;

    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, jump, crouchPressed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Duit"))
        {
            suaraCoin.Play();
            RCP++;
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Obstacle"))
        {
            SceneManager.LoadScene("Level1");
        }

        if (collision.CompareTag("JigSaw"))
        {
            SceneManager.LoadScene("Level2");
        }

         if (collision.CompareTag("Saw"))
        {
            SceneManager.LoadScene("Level3");
        }

        if (collision.CompareTag("Finish"))
        {
            SceneManager.LoadScene("Depan");
        }

        if (collision.CompareTag("Finishing"))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void GroundCheck()
    {
        isGrounded = false;
        //Check if the GroundCheckObject is collidinng with other
        //2D Colliders that are in the "Ground" Layer
        //If yes (isGround true) else (isGround false)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position,groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        isGrounded = true;

        //As long as we are grounded the "Jump" bool
        //in the animator is disabled
        animator.SetBool("Jump", !isGrounded);
    }

    void Move(float dir,bool jumpFlag,bool crouchFlag)
    {
        #region Jump & Crouch

        //If we are crouching and disabled crouching
        //Check overhead for collision with Ground items
        //If there are any, remain crouched, otherwise un-crouch
        if(crouchFlag)
        {
            if(Physics2D.OverlapCircle(overheadCheckCollider.position,overheadCheckRadius,groundLayer))
                crouchFlag = true;
        }
 
        //if we press Crouch disable standing collider + animate crouching
        //Reduce the speed
        //If released the original speed +
        //enable the standing collider+ disable crouch animation
        if(isGrounded)
        {
            standingCollider.enabled = !crouchFlag;

        //if the player is grounded and pressed space Jump
        if(jumpFlag)
        {
            isGrounded = false;
            jumpFlag = false;
            //Add jump force
            rb.AddForce(new Vector2(0f,jumpPower));
        }
        }

        animator.SetBool("Crouch", crouchFlag);

      
        #endregion

        #region Move & Run
        //Set value of x using dir and speed
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        //If we are running multiply with the running modifier
        if(isRunning)
        xVal *= runSpeedModifier;
        if(crouchFlag)
        xVal *= crouchSpeedModifier;
        //create vec2 the velocity
        Vector2 targetVelocity = new Vector2(xVal,rb.velocity.y);
        //Set the player's velocity
        rb.velocity = targetVelocity;

        //If looking left and clicked right (flip to the left)
        if(facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-3, 4, 1);
            facingRight = false;
        }
        //If looking left and clicked right (flip to the left)
        else if(!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(3, 4, 1);
            facingRight = true;
        }

        // 0 idle , 4 walking , 8 running
        //Set the float xVelocity according to the x value
        //of the RigidBody2 velocity
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        #endregion
    }
}
