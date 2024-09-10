using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneLoader : MonoBehaviour
{
    public Tower tower;
    public GameObject endScreen;
    public void Quit()
    {
        Application.Quit();
    }
    public void LoadMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void Resume()
    {
        Time.timeScale = 1;
    }
    

    public void EndScreen()
    {
        if (tower.dead == true)
        {
            endScreen.SetActive(true);
        }
    }

    
    

   
}

