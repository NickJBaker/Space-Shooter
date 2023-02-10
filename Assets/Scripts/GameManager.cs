using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;
    [SerializeField]
    private GameObject _pauseMenu;
    private bool _isGamePaused;

    void Update()
    {
        if (_isGamePaused == false)
        {
            PauseMenu();
        }
        else
        {
            UnPauseMenu();
        }

        RestartGame();
        QuitApplication();
    }

    public void RestartGame()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1); // Current game scene
        }
    }

    public void QuitApplication()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _pauseMenu.SetActive(true);
            Animator anim = _pauseMenu.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetBool("isPaused", true);
            }
            _isGamePaused = true;
            Time.timeScale = 0;
        }
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _isGamePaused = false;
    }

    public void UnPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1;
            _pauseMenu.SetActive(false);
            _isGamePaused = false;
        }
    }

    public void IsNotPaused()
    {
        _isGamePaused = false;
    }
}
