using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject optionsScreen;
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ToOptionsScreen()
    {
        titleScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
