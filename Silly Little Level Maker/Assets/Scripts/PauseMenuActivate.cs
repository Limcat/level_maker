using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuActivate : MonoBehaviour
{
    //public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public void Pause()
    {
        if (pauseMenuUI.activeSelf == false)
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
        }
        //GameIsPaused = true;
    }
}
