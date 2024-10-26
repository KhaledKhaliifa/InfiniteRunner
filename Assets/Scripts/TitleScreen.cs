using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void ToOptionsScreen()
    {
        SceneManager.LoadScene("OptionsScreen");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
