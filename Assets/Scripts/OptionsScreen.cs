using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsScreen : MonoBehaviour
{
    [SerializeField] TMP_Text muteText;
    public GameObject optionsScreen;
    public GameObject titleScreen;
    bool isMuted = false;

    public void BackToTitle()
    {
        titleScreen.SetActive(true);
        optionsScreen.SetActive(false);
    }
    public void Mute()
    {
        if (isMuted)
        {
            // UNMUTE
            isMuted = false;
            AudioListener.volume = 1;
            muteText.text = "Mute";

        }
        else
        {
            // MUTE
            isMuted = true;
            AudioListener.volume = 0;
            muteText.text = "Unmute";
        }
    }
}
