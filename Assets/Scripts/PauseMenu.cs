using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public GameObject pauseMenu;
    public static bool isPaused = false;

    public AudioSource audioSource;
    public AudioClip pauseAudio;
    private AudioClip gameAudio;
    private float timestamp; 



    void Start()
    {
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused) { 
                ResumeGame();

            }
            else
            {
                PauseGame();
                
            }
        }

    }
    public void PauseGame()
    {
        gameAudio = audioSource.clip;
        timestamp = audioSource.time;
        audioSource.Pause();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        audioSource.clip = pauseAudio;
        audioSource.Play();
    }
    public void ResumeGame()
    {
        audioSource.Stop();
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        audioSource.clip = gameAudio;
        audioSource.time = timestamp;
        audioSource.Play();
    }
    public void RestartGameFromPause()
    {
        SceneManager.LoadScene("GameScene");
        isPaused = false;
        Time.timeScale = 1f;

    }
    public void MainMenuFromPause()
    {
        SceneManager.LoadScene("TitleScreen");

    }
}
