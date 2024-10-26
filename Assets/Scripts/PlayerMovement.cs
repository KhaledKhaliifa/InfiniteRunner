using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField][Tooltip("Forward speed of the object")] float forwardSpeed;

    [SerializeField][Tooltip("Horizontal speed of the object")] float horizontalSpeed;

    [SerializeField] float jumpForce = 0.1f;

    [SerializeField] TMP_Text scoreText;

    [SerializeField] TMP_Text speedText;

    [SerializeField] TMP_Text fuelText;

    [SerializeField] TMP_Text invincibilityText;

    [SerializeField] TMP_Text fallText;

    public AudioClip jump, slow, wall, refill, burn, invalid, fall, boost;

    public AudioSource audioSource;

    public static int score;
    float fuel;
    float fuelTime;
    float scoreTime;
    string playerSpeed;

    bool Die = false;
    bool isInvincible = false;
    bool canFall = true;
    bool falling = false;
    bool reachedBoundary = false;
    bool isGrounded = true;
    bool isInBurningTile = false;

    Rigidbody rb;


    void Start()
    {
        forwardSpeed = 5.0f;
        horizontalSpeed = 10.0f;
        fuel = 50;
        fuelTime = 0;
        score = 0;
        scoreTime = 0;
        rb = this.gameObject.GetComponent<Rigidbody>();        
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (!Die)
            {
                MovePlayer();
                UpdateSpeed();
                UpdateScore();
                UpdateFuel();
                CheatCheck();
            }
            else
            {
                SceneManager.LoadScene("GameOver");
            }
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
            reachedBoundary = false;
        }
        else if (x < 0 && rb.position.x > -8.4)
        {
            horizontalMove = transform.right * x * horizontalSpeed * Time.fixedDeltaTime;
            reachedBoundary = false;
        }
        else if(x != 0)
        {
            if (!reachedBoundary)
            {
                playInvalid();
            }
            reachedBoundary = true;
            
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
            isGrounded = false;
            if(rb.position.y < -3)
            {
                if (!falling)
                {
                    StartCoroutine(fallCoroutine());
                }
                falling = true;

            }
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
        else
        {
            playInvalid();
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
            if (!isInBurningTile) // Check if not already in the area
            {
                isInBurningTile = true;
                playBurn();
            }
            
        }
        else if (collision.gameObject.CompareTag("SuppliesTile"))
        {
            fuel = 50;
            playRefill();
        }
        else if (collision.gameObject.CompareTag("BoostTile"))
        {
            if(forwardSpeed != 10.0)
            {
                forwardSpeed *= 2;
                playBoost();
            }
        }
        else if (collision.gameObject.CompareTag("StickyTile"))
        {
            if (forwardSpeed == 10.0)
            {
                forwardSpeed /= 2;
                playSlow();
                
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            if (!isInvincible)
            {
                StartCoroutine(wallCorotuine());
                Die = true;
            }
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("BurningTile"))
        {
            isInBurningTile = false;
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
        if (forwardSpeed == 5.0f)
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
        if(fuel <= 0)
        {
            Die = true;
        }
        if (isInBurningTile)
        {
            fuel = fuel - Time.deltaTime * 10;
        }
        
        fuelText.text = "Fuel: " + ((int)fuel).ToString();
    }
    void CheatCheck()
    {
        ////////////////////// CHEATS RELATED FUNCTION //////////////////////

        // INVINCIBILITY CHEAT

        if (isInvincible)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                isInvincible = false;
                invincibilityText.text = "";
            }
            else
            {
                GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Wall");
                foreach (GameObject obstacle in obstacles)
                {
                    BoxCollider boxcollider = obstacle.GetComponent<BoxCollider>();
                    if (boxcollider != null)
                    {
                        boxcollider.enabled = false;
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                isInvincible = true;
                invincibilityText.text = "Invincible";
            }
            else
            {
                GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Wall");
                foreach (GameObject obstacle in obstacles)
                {
                    if (obstacle != null)
                    {
                        BoxCollider boxcollider = obstacle.GetComponent<BoxCollider>();
                        if (boxcollider != null)
                        {
                            boxcollider.enabled = true;
                        }
                    }
                }
            }

        }


        // SPEED CHEAT
        if (Input.GetKeyDown(KeyCode.H))
        {
            if(forwardSpeed != 8.0)
            {
                forwardSpeed /= 2;
            }
        }

        // FUEL CHEAT
        if (Input.GetKeyDown(KeyCode.F))
        {
            fuel = 50;
        }

        // FALL CHEAT
        if (canFall)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                canFall = false;
                fallText.text = "No Fall";
            }
            else
            {
                GameObject[] emptyTiles = GameObject.FindGameObjectsWithTag("EmptyTile");
                foreach (GameObject tile in emptyTiles)
                {
                    BoxCollider boxcollider = tile.GetComponent<BoxCollider>();
                    if (boxcollider != null)
                    {
                        boxcollider.enabled = false;
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                canFall = true;
                fallText.text = "";
            }
            else
            {
                GameObject[] emptyTiles = GameObject.FindGameObjectsWithTag("EmptyTile");
                foreach (GameObject tile in emptyTiles)
                {
                    if (tile != null)
                    {
                        BoxCollider boxcollider = tile.GetComponent<BoxCollider>();
                        if (boxcollider != null)
                        {
                            boxcollider.enabled = true;
                        }
                    }
                }
            }

        }
    }
    public void playJump()
    {
        audioSource.clip = jump;
        audioSource.Play();
    }
    public void playSlow()
    {
        audioSource.clip = slow;
        audioSource.Play();
    }
    public void playRefill()
    {
        audioSource.clip = refill;
        audioSource.Play();
    }
    public void playWall()
    {
        audioSource.clip = wall;
        audioSource.PlayOneShot(audioSource.clip, 1.4f);
    }
    public void playBoost()
    {
        audioSource.clip = boost;
        audioSource.Play();
    }
    public void playFall()
    {
        audioSource.clip = fall;
        audioSource.PlayOneShot(audioSource.clip);

    }
    public void playInvalid()
    {
        audioSource.clip = invalid;
        audioSource.PlayOneShot(audioSource.clip);
    }
    public void playBurn()
    {
        audioSource.clip = burn;
        audioSource.Play();
    }

    IEnumerator fallCoroutine()
    {
        playFall();
        yield return new WaitForSeconds(audioSource.clip.length);
        Die = true;
    }
    IEnumerator wallCorotuine()
    {
        playWall();
        yield return new WaitForSeconds(audioSource.clip.length);
        Die = true;
    }

}
