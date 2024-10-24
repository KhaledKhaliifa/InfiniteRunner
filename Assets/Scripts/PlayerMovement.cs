using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Tooltip("Forward speed of the object")] float forwardSpeed;

    [SerializeField][Tooltip("Horizontal speed of the object")] float horizontalSpeed;

    [SerializeField] float jumpForce = 0.1f;

    [SerializeField] TMP_Text scoreText;

    [SerializeField] TMP_Text speedText;

    [SerializeField] TMP_Text fuelText;

    bool Die = false;

    int score;
    int fuel;
    float fuelTime;
    float scoreTime;
    string playerSpeed;

    Rigidbody rb;

    bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Start");
        forwardSpeed = 10.0f;
        horizontalSpeed = 15.0f;
        fuel = 50;
        fuelTime = 0;
        score = 0;
        scoreTime = 0;
        rb = this.gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Die)
        {
            MovePlayer();
            UpdateSpeed();
            UpdateScore();
            UpdateFuel();
        }
        
    }
    void MovePlayer()
    {
        ////////////////////// MOVEMENT RELATED FUNCTION //////////////////////

        // Capturing input from the user
        float x = Input.GetAxis("Horizontal");

        // Initializing the sideways and forwards movement vectors
        Vector3 horizontalMove = new Vector3(0, 0, 0);
        Vector3 forwardMove = new Vector3(0, 0, forwardSpeed * Time.fixedDeltaTime);

        // Limiting the player's movement so they don't fall left or right of the platform
        if (x > 0 && rb.position.x < 8.4)
        {
            horizontalMove = transform.right * x * horizontalSpeed * Time.fixedDeltaTime;
        }
        else if (x < 0 && rb.position.x > -8.4)
        {
            horizontalMove = transform.right * x * horizontalSpeed * Time.fixedDeltaTime;
        }
        
        // Initiating movement 
        rb.MovePosition(rb.position + horizontalMove + forwardMove);


        // If user pressed space, invoke Jump function
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();

        }
        if(rb.position.y < 0)
        {
            Die = true;
        }
    }

    void Jump()
    {
        ////////////////////// JUMP RELATED FUNCTION //////////////////////

        // Check if the user is grounded, if yes, jump
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce);
            isGrounded = false;
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        ////////////////////// COLLISION RELATED FUNCTION //////////////////////

        // Whenever the user collides with any object, consider them grounded
        isGrounded = true;

        // Writing the logic for every tile
        if (collision.gameObject.CompareTag("BurningTile"))
        {
            fuel -= 10;
        }
        else if (collision.gameObject.CompareTag("SuppliesTile"))
        {
            fuel = 50;
        }
        else if (collision.gameObject.CompareTag("BoostTile"))
        {
            if(forwardSpeed == 10.0f)
            {
                forwardSpeed *= 2;
            }
        }
        else if (collision.gameObject.CompareTag("StickyTile"))
        {
            if(forwardSpeed == 20.0f)
            {
                forwardSpeed /= 2;
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Die = true;
        }

    }
    private void UpdateScore()
    {        
        ////////////////////// SCORE RELATED FUNCTION //////////////////////

        // Updating score by 1 each second
        scoreTime += Time.deltaTime;
        if (scoreTime >= 1.0)
        {
            scoreTime = 0;
            score += 1;
            scoreText.text = "Score: " + score.ToString();
        }
        
    }
    private void UpdateSpeed()
    {
        ////////////////////// SPEED RELATED FUNCTION //////////////////////

        // Changing the speed text in the ui
        if (forwardSpeed == 10.0f)
        {
            playerSpeed = "Normal";
        }
        else
        {
            playerSpeed = "Fast";
        }
        speedText.text = "Speed: " + playerSpeed;
    }
    private void UpdateFuel()
    {
        ////////////////////// FUEL RELATED FUNCTION //////////////////////

        // Changing the fuel and fuel text in the ui
        fuelTime += Time.deltaTime;
        if(fuelTime >= 1)
        {
            fuelTime = 0;
            fuel -= 1;
        }
        fuelText.text = "Fuel: " + fuel.ToString();
    }
}
