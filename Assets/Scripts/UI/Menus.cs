using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    public GameObject deathMenu;
    public GameObject pauseMenu;
    public GameObject controlsImage;
    bool gameIsPaused = false;

    private void Awake()
    {
    }

    public void TurnOnDeathMenu()
    {
        deathMenu.SetActive(true);
    }

    public void TurnOnPauseMenu()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void ViewControls()
    {
        controlsImage.SetActive(true);
    }

    public void CloseControls()
    {
        controlsImage.SetActive(false);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMenu()
    {
        //SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
