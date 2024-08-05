using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel, pauseMenuPanel;
    public Movement move;
    public FirstPersonCam cam;
    public PlayerAttack attack;
    public PlayerTimer timer;

    public bool gamePaused;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        move = FindObjectOfType<Movement>();
        cam = FindObjectOfType<FirstPersonCam>();
        attack = FindObjectOfType<PlayerAttack>();
        timer = FindObjectOfType<PlayerTimer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gamePaused) 
        {
            UnPauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !gamePaused)
        {
            PauseGame();
        }
    }

    public void EndGame()
    {
        Time.timeScale = 0f;
        move.enabled = false;
        cam.enabled = false;
        attack.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameOverPanel.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        move.enabled = false;
        cam.enabled = false;
        attack.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenuPanel.SetActive(true);
        gamePaused = true;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        move.enabled = true;
        cam.enabled = true;
        attack.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuPanel.SetActive(false);
        gamePaused = false;
    }

    public void QuickRestart()
    {
        SceneManager.LoadScene("Golf");
        Time.timeScale = 1f;
        move.enabled = true;
        cam.enabled = true;
        attack.enabled = true;
        timer.time = timer.maxTime;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameOverPanel.SetActive(false);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
