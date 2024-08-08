using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float jumpPower = 200f;
    public float secondJumpPower = 300f;
    public Transform groundCheckPosition;
    public float radius = 0.3f;
    public LayerMask layerGround;

    private bool isGrounded;
    private bool playerJumped;
    private bool doubleJump;
    private bool gameStarted;

    public GameObject smokePosition;
    private Rigidbody playerRigidbody;
    private PlayerAnimation animationObject;
    private BGScroller bgScroller;
    private PlayerHealthDamage playerShoot;

    // Awake is called even if the script is disabled & before Start
    // Awake is used to initialise an object’s own references and variables
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        animationObject = GetComponent<PlayerAnimation>();
        bgScroller = GameObject.Find(Tags.BG_GAME_OBJ).GetComponent<BGScroller>();
        playerShoot = GetComponent<PlayerHealthDamage>();
    }

    // Start is called before the first frame update
    // Start is to use or create references to other objects and their components
    void Start()
    {
        StartCoroutine(StartGame());
    }

    // FixedUpdate is used for physics calculations & called once per 3-4 frames
    // Update called once per frame
    void FixedUpdate()
    {
        if(gameStarted)
        {
            PlayerMove();
            PlayerGrounded();
            PlayerJump();
        }
    }

    void PlayerMove()
    {
        playerRigidbody.velocity = new Vector3(movementSpeed, playerRigidbody.velocity.y, 0f);
    }

    void PlayerGrounded()
    {
        // check whether collision happened with the gameobject & layer speciified, returns an array of collisions happened
        isGrounded = Physics.OverlapSphere(groundCheckPosition.position, radius, layerGround).Length > 0;
        /*Debug.Log("Player Grouned: " + isGrounded);*/
    }

    void PlayerJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && !isGrounded && doubleJump)
        {
            doubleJump = false;
            playerRigidbody.AddForce(new Vector3(0, secondJumpPower, 0));
        }
        else if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)) && isGrounded)
        {
            animationObject.DidJump();
            playerRigidbody.AddForce(new Vector3(0, jumpPower, 0));
            playerJumped = true;
            doubleJump = true;
        }

    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
        gameStarted = true;
        animationObject.PlayerRun();
        smokePosition.SetActive(true);
        bgScroller.canScroll = true;
        playerShoot.canShoot = true;
        GameplayController.gamePlayInstance.canAddScore = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(Tags.PLATFORM_TAG))
        {
            if (playerJumped)
            {
                playerJumped = false;
                animationObject.DidLand();
            }
        }
    }
}
