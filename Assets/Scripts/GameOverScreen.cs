using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    int score;

    public void Start()
    {
        score = PlayerMovement.score;
        ViewScore();
    }
    void ViewScore()
    {
        scoreText.text = "Final Score: "+ score.ToString();
    }
    // Start is called before the first frame update
    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
