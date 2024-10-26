using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsScreen : MonoBehaviour
{
    [SerializeField] TMP_Text muteText;
    bool isMuted = false;

    public void BackToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
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
